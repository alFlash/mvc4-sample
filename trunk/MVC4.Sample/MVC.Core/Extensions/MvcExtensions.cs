using System;
using System.Linq.Expressions;
using System.Web.Mvc;
using System.Web.Mvc.Html;

namespace MVC.Core.Extensions
{
    public static class MvcHtmlExtension
    {
        public static MvcHtmlString PartialFor<TModel, TProperty>(this HtmlHelper<TModel> helper, 
            Expression<Func<TModel, TProperty>> expression, 
            string partialViewName)
        {
            var name = ExpressionHelper.GetExpressionText(expression);
            var model = ModelMetadata.FromLambdaExpression(expression, helper.ViewData).Model;
            var namePrefix = helper.ViewData != null && helper.ViewData.TemplateInfo != null &&
                   !string.IsNullOrWhiteSpace(helper.ViewData.TemplateInfo.HtmlFieldPrefix)
                   ? string.Format("{0}.{1}", helper.ViewData.TemplateInfo.HtmlFieldPrefix, name) : name;
            var viewData = new ViewDataDictionary(helper.ViewData)
            {
                TemplateInfo = new TemplateInfo
                {
                    HtmlFieldPrefix = namePrefix
                }
            };

            return helper.Partial(partialViewName, model, viewData);

        }

        public static bool IsValidGroupRecursive<TModel>(this ModelStateDictionary modelState, TModel model, string groupNames)
        {
            var result = true;

            var properties = model.GetType().GetProperties();
            if (properties.Length > 0)
            {
                for (var i = 0; i < properties.Length; i++ )
                {
                    var propertyInfo = properties[i];
                    var hasValidationGroupAttrib = Attribute.IsDefined(propertyInfo, typeof(ValidationGroupAttribute));
                    var param = propertyInfo.GetIndexParameters().Length;
                    if (hasValidationGroupAttrib)
                    {
                        result = modelState.IsGroupValid(model, groupNames);
                        if (!result)
                        {
                            break;
                        }
                    }
                    else
                    {
                        if (propertyInfo.PropertyType.GetProperties().Length > 0)
                        {
                            result = modelState.IsValidGroupRecursive(propertyInfo.GetValue(model, param > 0 ? new object[] { i } : null), groupNames);
                            if (!result)
                            {
                                break;
                            }
                        }
                    }

                }
            }
            return result;
        }
    }
}
