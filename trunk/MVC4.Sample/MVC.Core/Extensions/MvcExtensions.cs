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
    }
}
