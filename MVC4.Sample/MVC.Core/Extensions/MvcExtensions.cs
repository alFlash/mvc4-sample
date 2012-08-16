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

        /// <summary>
        /// Determines whether [is group valid] [the specified model state].
        /// </summary>
        /// <typeparam name="TModel">The type of the model.</typeparam>
        /// <param name="modelState">State of the model.</param>
        /// <param name="viewModel">The view model.</param>
        /// <param name="validationGroups">The validation groups.</param>
        /// <param name="prefix">The prefix.</param>
        /// <returns>
        ///   <c>true</c> if [is group valid] [the specified model state]; otherwise, <c>false</c>.
        /// </returns>
        public static bool IsValidationGroupValid<TModel>(this ModelStateDictionary modelState, TModel viewModel, string validationGroups, string prefix = "")
        {
            var groups = new List<string>();
            groups.AddRange(validationGroups.Split(' '));
            return modelState.IsValidationGroupValid(viewModel, groups, prefix);
        }

        /// <summary>
        /// Determines whether [is group valid] [the specified model state].
        /// </summary>
        /// <typeparam name="TModel">The type of the model.</typeparam>
        /// <param name="modelState">State of the model.</param>
        /// <param name="viewModel">The view model.</param>
        /// <param name="validationGroups">The validation groups.</param>
        /// <param name="prefix">The prefix.</param>
        /// <returns>
        ///   <c>true</c> if [is group valid] [the specified model state]; otherwise, <c>false</c>.
        /// </returns>
        public static bool IsValidationGroupValid<TModel>(this ModelStateDictionary modelState, TModel viewModel, List<string> validationGroups, string prefix = "")
        {
            var result = true;
            if (viewModel.GetType().IsArray || viewModel.GetType().IsGenericType || viewModel is IEnumerable)
            {
                if (!modelState.IsEnumarableGroupValid(viewModel, validationGroups, prefix)) result = false;
            }
            else
            {
                var properties = viewModel.GetType().GetProperties(BindingFlags.Public | BindingFlags.DeclaredOnly | BindingFlags.Instance);
                foreach (var property in properties.Where(property => property.CanRead))
                {
                    if (!(property.PropertyType is IEnumerable || property.PropertyType.IsArray || property.PropertyType.IsGenericType)
                        && Attribute.IsDefined(property, typeof(ValidationGroupAttribute)) && !modelState.IsObjectValid(viewModel, validationGroups, property, prefix))
                    {
                        result = false;
                    }
                    var subPrefix = !string.IsNullOrWhiteSpace(prefix) ? string.Format("{0}.{1}", prefix, property.Name) : property.Name;
                    if (!modelState.IsGroupValidRecursive(viewModel, validationGroups, property, subPrefix)) result = false;
                }
            }

            return result;
        }

        /// <summary>
        /// Determines whether [is object valid] [the specified model state].
        /// </summary>
        /// <typeparam name="TModel">The type of the model.</typeparam>
        /// <param name="modelState">State of the model.</param>
        /// <param name="model">The model.</param>
        /// <param name="groupNames">The group names.</param>
        /// <param name="property">The property.</param>
        /// <param name="prefix">The prefix.</param>
        /// <returns>
        ///   <c>true</c> if [is object valid] [the specified model state]; otherwise, <c>false</c>.
        /// </returns>
        private static bool IsObjectValid<TModel>(this ModelStateDictionary modelState, TModel model, List<string> groupNames, PropertyInfo property, string prefix = "")
        {
            var attributes = property.GetCustomAttributes(false);
            var result = true;
            foreach (var attribute in attributes)
            {
                if (groupNames != null && ((attribute is ValidationGroupAttribute) && IsValidValidationGroup(((ValidationGroupAttribute)attribute).GroupName, groupNames)))
                {
                    var validationResult = new List<ValidationResult>();
                    var isValid = Validator.TryValidateObject(model, new ValidationContext(model, null, null), validationResult, true);
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
                }
            }
            return result;
        }

        /// <summary>
        /// Determines whether [is group valid recursive] [the specified model state].
        /// </summary>
        /// <typeparam name="TModel">The type of the model.</typeparam>
        /// <param name="modelState">State of the model.</param>
        /// <param name="model">The model.</param>
        /// <param name="groupNames">The group names.</param>
        /// <param name="property">The property.</param>
        /// <param name="prefix">The prefix.</param>
        /// <returns>
        ///   <c>true</c> if [is group valid recursive] [the specified model state]; otherwise, <c>false</c>.
        /// </returns>
        private static bool IsGroupValidRecursive<TModel>(this ModelStateDictionary modelState, TModel model, List<string> groupNames, PropertyInfo property, string prefix = "")
        {
            var instance = property.GetValue(model, null);
            if (instance != null)
            {
                var isValid = modelState.IsValidationGroupValid(instance, groupNames, prefix);
                if (!isValid)
                {
                    return false;
                }
            }
            return true;
        }

        /// <summary>
        /// Determines whether [is enumarable group valid] [the specified model state].
        /// </summary>
        /// <typeparam name="TModel">The type of the model.</typeparam>
        /// <param name="modelState">State of the model.</param>
        /// <param name="model">The model.</param>
        /// <param name="groupNames">The group names.</param>
        /// <param name="prefix">The prefix.</param>
        /// <returns>
        ///   <c>true</c> if [is enumarable group valid] [the specified model state]; otherwise, <c>false</c>.
        /// </returns>
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

        /// <summary>
        /// Determines whether [is valid validation group] [the specified item group names].
        /// </summary>
        /// <param name="itemGroupNames">The item group names.</param>
        /// <param name="groupNames">The group names.</param>
        /// <returns>
        ///   <c>true</c> if [is valid validation group] [the specified item group names]; otherwise, <c>false</c>.
        /// </returns>
        private static bool IsValidValidationGroup(string itemGroupNames, IEnumerable<string> groupNames)
        {
            var groups = itemGroupNames.Split(' ');
            return groups.Any(@group => groupNames.Any(groupName => @group == groupName));
        }
    }
}
