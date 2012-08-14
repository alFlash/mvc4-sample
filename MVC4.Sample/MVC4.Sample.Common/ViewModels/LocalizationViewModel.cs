using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using MVC.Core.Attributes;

namespace MVC4.Sample.Common.ViewModels
{
    public class LocalizationViewModel
    {
        //[Required(ErrorMessage = "daslkfjslajflsdjf")]
        //[CustomRequired(ErrorMessageResourceName = "Student", ErrorMessageResourceClass = "MyResource")]
        //[CustomRequired(ErrorMessage = "Error from ErrorMessage Properties")]
        [CustomRequired]
        [Display(Name = "Student", Order = 0)]
        public string Student { get; set; }
    }
}
