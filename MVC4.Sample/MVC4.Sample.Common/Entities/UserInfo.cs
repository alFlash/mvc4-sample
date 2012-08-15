using System;
using System.ComponentModel.DataAnnotations;
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
