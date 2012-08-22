using System;
using System.Linq;
using System.Reflection;
using System.Web.Mvc;

namespace MVC.Core.Attributes
{
    /// <summary>
    /// Multiple buttons on a form.
    /// </summary>
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = false, Inherited = true)]
    public class ActionCommandAttribute : ActionNameSelectorAttribute
    {
        public string Name { get; set; }
        
        public override bool IsValidName(ControllerContext controllerContext, string actionName, MethodInfo methodInfo)
        {
            var value = controllerContext.Controller.ValueProvider.GetValue(Name);
            var result = !string.IsNullOrWhiteSpace(controllerContext.HttpContext.Request[Name]) && !string.IsNullOrWhiteSpace(value.AttemptedValue)
                && controllerContext.HttpContext.Request[Name] == value.AttemptedValue;
            return result;
        }
    }
}
