using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.DataAnnotations;
using System.Web;

namespace MVC.Core.Attributes
{
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false, Inherited = true)]
    public class CustomRequiredAttribute<T> : T
        where T: ValidationAttribute
    //public class CustomRequiredAttribute : RequiredAttribute
    {
        public string ErrorMessageResourceClass { get; set; }

        public override string FormatErrorMessage(string name)
        {
            var result =(!string.IsNullOrWhiteSpace(ErrorMessageResourceClass) && ErrorMessageResourceType != null )
                || !string.IsNullOrWhiteSpace(ErrorMessage)
                ? base.FormatErrorMessage(name) : !string.IsNullOrWhiteSpace(name) ? string.Format("The field {0} is required", name): "This field is required"; //base.FormatErrorMessage(name);
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
