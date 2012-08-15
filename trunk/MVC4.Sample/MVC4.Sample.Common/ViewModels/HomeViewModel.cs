using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Web;
using MVC.Core.Attributes;
using MVC4.Sample.Common.Entities;

namespace MVC4.Sample.Common.ViewModels
{
    public class HomeViewModel
    {
        [CustomRequired(ErrorMessageResourceName = "Student", ErrorMessageResourceClass = "MyResource", AllowEmptyStrings = true)]
        [CustomDisplayName("alsjflsajdf")]
        public string Student { get; set; }
        public string Employee { get; set; }
        public List<People> Peoples { get; set; }
    }
}
