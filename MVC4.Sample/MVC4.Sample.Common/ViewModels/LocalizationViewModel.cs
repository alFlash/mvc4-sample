using MVC.Core.Attributes;

namespace MVC4.Sample.Common.ViewModels
{
    public class LocalizationViewModel
    {
        [CustomRequired(ErrorMessageResourceName = "StudentRequired", ErrorMessageResourceClass = "MyResource")]
        [CustomDisplayNameAttribute("Student", "MyResource")]
        public string Student { get; set; }

        [CustomRequired]
        [CustomDisplayNameAttribute("AnotherStudent", "MyResource")]
        public string AnotherStudent { get; set; }
    }
}
