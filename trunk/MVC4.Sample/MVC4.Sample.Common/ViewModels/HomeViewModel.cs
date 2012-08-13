using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using MVC4.Sample.Common.Entities;

namespace MVC4.Sample.Common.ViewModels
{
    public class HomeViewModel
    {
        [Required]
        [Display(Name = "Student", Order = 0)]
        public string Student { get; set; }
        public string Employee { get; set; }
        public List<People> Peoples { get; set; }
    }
}
