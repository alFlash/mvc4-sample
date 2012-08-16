using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;

namespace MVC.Core.Entities
{
    public class CustomValidationResult
    {
        public List<ValidationResult> ValidationResults { get; set; }
        public bool IsValid { get; set; }
    }
}
