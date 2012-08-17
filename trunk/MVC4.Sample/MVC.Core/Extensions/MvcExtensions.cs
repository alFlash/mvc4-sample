using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Web.Mvc;
using System.Web.Mvc.Html;

namespace MVC.Core.Extensions
{
    public static class MvcHtmlExtension
    {
        /// <summary>
        /// Partials for.
        /// </summary>
        /// <typeparam name="TModel">The type of the model.</typeparam>
        /// <typeparam name="TProperty">The type of the property.</typeparam>
        /// <param name="helper">The helper.</param>
        /// <param name="expression">The expression.</param>
        /// <param name="partialViewName">Partial name of the view.</param>
        /// <returns></returns>
        public static MvcHtmlString PartialFor<TModel, TProperty>(this HtmlHelper<TModel> helper,
            Expression<Func<TModel, TProperty>> expression,
            string partialViewName)
        {
            var name = ExpressionHelper.GetExpressionText(expression);
            var model = ModelMetadata.FromLambdaExpression(expression, helper.ViewData).Model;
            var namePrefix = helper.ViewData != null && helper.ViewData.TemplateInfo != null &&
                   !String.IsNullOrWhiteSpace(helper.ViewData.TemplateInfo.HtmlFieldPrefix)
                   ? String.Format("{0}.{1}", helper.ViewData.TemplateInfo.HtmlFieldPrefix, name) : name;
            var viewData = new ViewDataDictionary(helper.ViewData)
            {
                TemplateInfo = new TemplateInfo
                {
                    HtmlFieldPrefix = namePrefix
                }
            };

            return helper.Partial(partialViewName, model, viewData);

        }

        public static bool IsModelValid<TModel>(this ModelStateDictionary modelState, TModel viewModel, string validationGroups = "", string prefix = "")
        {
            if (string.IsNullOrWhiteSpace(validationGroups))
            {
                return modelState.IsValidationGroupValid(viewModel, prefix: prefix);
            }
            var groups = validationGroups.Split(' ');
            return modelState.IsValidationGroupValid(viewModel, new List<string>(groups), prefix);
        }

        public static bool IsValidationGroupValid<TModel>(this ModelStateDictionary modelState, TModel viewModel, List<string> validationGroups = null, string prefix = "")
        {
            var result = true;
            //List
            if (viewModel.GetType().IsArray || viewModel.GetType().IsGenericType || viewModel is IEnumerable)
            {
                if (!modelState.IsEnumarableGroupValid(viewModel, validationGroups, prefix)) result = false;
            }
            else //Property
            {
                var properties = viewModel.GetType().GetProperties(BindingFlags.Public | BindingFlags.DeclaredOnly | BindingFlags.Instance);
                result = ValidateProperties(modelState, viewModel, validationGroups, prefix, properties);
                var isValid = TraverseRecursive(modelState, viewModel, validationGroups, prefix, properties);
                if (!isValid)
                {
                    result = false;
                }
            }
            return result;
        }

        private static bool TraverseRecursive<TModel>(ModelStateDictionary modelState, TModel viewModel, List<string> validationGroups,
                                                      string prefix, IEnumerable<PropertyInfo> properties)
        {
            var result = true;
            foreach (var p in properties.Where(p => p.CanRead))
            {
                if (p.PropertyType.Name != "String" && !p.PropertyType.IsValueType)
                {
                    //List/Array/Object...
                    var subPrefix = !string.IsNullOrWhiteSpace(prefix)
                                        ? string.Format("{0}.{1}", prefix, p.Name)
                                        : p.Name;
                    if (!modelState.IsGroupValidRecursive(viewModel, validationGroups, p, subPrefix)) result = false;
                }
            }
            return result;
        }

        private static bool ValidateProperties<TModel>(ModelStateDictionary modelState, TModel viewModel, List<string> validationGroups,
                                                       string prefix, IEnumerable<PropertyInfo> properties)
        {
            return properties.Where(property => property.CanRead)
                .Where(property => (!(property.PropertyType is IEnumerable) && !property.PropertyType.IsArray && !property.PropertyType.IsGenericType))
                .All(property => ValidateProperty(modelState, viewModel, validationGroups, prefix, property));
        }

        private static bool IsEnumarableGroupValid<TModel>(this ModelStateDictionary modelState, TModel model, List<string> groupNames, string prefix = "")
        {
            var result = true;
            var list = ((IEnumerable)model).GetEnumerator();
            var index = 0;

            while (list.MoveNext())
            {
                var subPrefix = !string.IsNullOrWhiteSpace(prefix) ? string.Format("{0}[{1}]", prefix, index) : prefix;
                var isValid = modelState.IsValidationGroupValid(list.Current, groupNames, subPrefix);
                if (!isValid)
                {
                    result = false;
                }
                index++;
            }
            return result;
        }

        private static bool IsGroupValidRecursive<TModel>(this ModelStateDictionary modelState, TModel model, List<string> groupNames, PropertyInfo property, string prefix = "")
        {
            var instance = property.GetValue(model, null);
            if (instance != null && !string.IsNullOrWhiteSpace(instance.ToString()))
            {
                var isValid = modelState.IsValidationGroupValid(instance, groupNames, prefix);
                if (!isValid)
                {
                    return false;
                }
            }
            return true;
        }

        private static bool ValidateProperty<TModel>(ModelStateDictionary modelState, TModel viewModel, List<string> validationGroups,
                                                     string prefix, PropertyInfo property)
        {
            var result = true;
            if (validationGroups == null || validationGroups.Count < 0)
            {
                if (!ValidateModel(modelState, viewModel, prefix))
                {
                    result = false;
                }
            }
            else if (Attribute.IsDefined(property, typeof (ValidationGroupAttribute)))
            {
                var attributes = property.GetCustomAttributes(false);
                if (attributes.Where(attribute => ((attribute is ValidationGroupAttribute)) && IsValidValidationGroupName(((ValidationGroupAttribute) attribute).GroupName, validationGroups)).Any(attribute => !ValidateModel(modelState, viewModel, prefix)))
                {
                    result = false;
                }
            }
            return result;
        }

        private static bool ValidateModel<TModel>(ModelStateDictionary modelState, TModel viewModel, string prefix)
        {
            var result = true;
            var validationResult = new List<ValidationResult>();
            var isValid = Validator.TryValidateObject(viewModel,
                                                      new ValidationContext(viewModel, null, null),
                                                      validationResult, true);
            if (!isValid)
            {
                result = false;
                foreach (var vResult in validationResult)
                {
                    foreach (var name in vResult.MemberNames)
                    {
                        var subPrefix = !string.IsNullOrWhiteSpace(prefix)
                                            ? string.Format("{0}.{1}", prefix, name)
                                            : name;
                        modelState.AddModelError(subPrefix, vResult.ErrorMessage);
                    }
                }
            }
            return result;
        }

        /// <summary>
        /// Determines whether [is valid validation group] [the specified item group names].
        /// </summary>
        /// <param name="itemGroupNames">The item group names.</param>
        /// <param name="groupNames">The group names.</param>
        /// <returns>
        ///   <c>true</c> if [is valid validation group] [the specified item group names]; otherwise, <c>false</c>.
        /// </returns>
        private static bool IsValidValidationGroupName(string itemGroupNames, ICollection<string> groupNames)
        {
            var result = true;
            if (groupNames != null && groupNames.Count > 0)
            {
                if (string.IsNullOrWhiteSpace(itemGroupNames))
                {
                    result = false;
                }
                else if (!string.IsNullOrWhiteSpace(itemGroupNames))
                {
                    var groups = itemGroupNames.Split(' ');
                    result = groups.Any(@group => groupNames.Any(groupName => @group == groupName));
                }
            }

            return result;
        }
    }
}
