using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Web.Mvc;

namespace MVC4.Sample.Common.Entities
{
    public class UserInfo
    {
        public Guid Id { get; set; }
        [Required]
        [ValidationGroup("Users")]
        public string UserName { get; set; }
    }
}
