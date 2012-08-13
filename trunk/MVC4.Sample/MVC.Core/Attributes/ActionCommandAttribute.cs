using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Web.Mvc;

namespace MVC.Core.Attributes
{
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = false, Inherited = true)]
    public class ActionCommandAttribute : ActionNameSelectorAttribute
    {
        public string ButtonName { get; set; }
        public string ButtonValue { get; set; }
        public override bool IsValidName(ControllerContext controllerContext, string actionName, MethodInfo methodInfo)
        {
            return controllerContext.HttpContext.Request[ButtonName] != null &&
                controllerContext.HttpContext.Request[ButtonName] == ButtonValue;
        }
    }
}
