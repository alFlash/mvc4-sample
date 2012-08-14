using System.Web.Mvc;
using MVC.Core.Attributes;

namespace MVC.Core.Validators
{
    public class CustomRequiredValidator : CustomBaseValidator<CustomRequiredAttribute>
    {
        public CustomRequiredValidator(ModelMetadata metadata, ControllerContext context, CustomRequiredAttribute attribute) : base(metadata, context, attribute)
        {
        }
    }
}
