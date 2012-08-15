using System;
using System.ComponentModel.DataAnnotations;
using System.Web;

namespace MVC.Core.Attributes
{
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false, Inherited = true)]
    public class CustomRequiredAttribute : RequiredAttribute
    {
        public string ErrorMessageResourceClass { get; set; }

        public override string FormatErrorMessage(string name)
        {
            var defaultErrorMessage = !string.IsNullOrWhiteSpace(name)
                                      ? string.Format("The field \"{0}\" is required.", name)
                                      : "This field is required.";
            var result =(!string.IsNullOrWhiteSpace(ErrorMessageResourceClass) && ErrorMessageResourceType != null )
                || !string.IsNullOrWhiteSpace(ErrorMessage)
                ? base.FormatErrorMessage(name) : defaultErrorMessage;
            if (!string.IsNullOrWhiteSpace(ErrorMessageResourceClass) && !string.IsNullOrWhiteSpace(ErrorMessageResourceName))
            {
                var globalResourceObject = HttpContext.GetGlobalResourceObject(ErrorMessageResourceClass, ErrorMessageResourceName);
                if (globalResourceObject != null)
                {
                    result = string.Format(globalResourceObject.ToString(), name);
                }
            }
            return result;
        }
    }
}
