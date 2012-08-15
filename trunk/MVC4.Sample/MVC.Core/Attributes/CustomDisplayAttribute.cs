using System;
using System.ComponentModel;
using System.Web;

namespace MVC.Core.Attributes
{
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false, Inherited = true)]
    public class CustomDisplayNameAttribute : DisplayNameAttribute
    {
        public CustomDisplayNameAttribute(string name)
            : base(name)
        { }

        public CustomDisplayNameAttribute(string resourceKey, string resourceClassName)
            : base(GetMessageFromResource(resourceKey, resourceClassName))
        { }

        private static string GetMessageFromResource(string resourceKey, string resourceClassName)
        {
            if (!string.IsNullOrWhiteSpace(resourceClassName))
            {
                var globalResourceObject = HttpContext.GetGlobalResourceObject(resourceClassName, resourceKey);
                if (globalResourceObject != null)
                {
                    return globalResourceObject.ToString();
                }
            }
            return resourceKey;
        }
    }
}
