using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace MVC.Core.Validators
{
    public class CustomBaseValidator<TAttribute> : DataAnnotationsModelValidator<TAttribute>
        where TAttribute : ValidationAttribute
    {
        readonly string _message;
        public CustomBaseValidator(ModelMetadata metadata, ControllerContext context, TAttribute attribute) : base(metadata, context, attribute)
        {
            var displayName = !string.IsNullOrWhiteSpace(metadata.DisplayName)
                                  ? metadata.DisplayName
                                  : metadata.PropertyName;
            _message = !string.IsNullOrWhiteSpace(attribute.ErrorMessage) ? attribute.ErrorMessage : attribute.FormatErrorMessage(displayName);
        }

        public override IEnumerable<ModelClientValidationRule> GetClientValidationRules()
        {
            var validationType = GetType().Name.Replace("Custom", string.Empty);
            validationType = validationType.Replace("Validator", string.Empty);
            var rule = new ModelClientValidationRule
            {
                ErrorMessage = _message,
                ValidationType = validationType.ToLower()
            };
            return new[] { rule };
        }
    }
}
