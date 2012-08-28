using System;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;
using MVC.Core.Attributes;

namespace MVC4.Sample.Common.Entities
{
    [Serializable]
    public class UserInfo
    {
        public Guid Id { get; set; }
        [CustomRequired]
        [StringLength(10, MinimumLength = 5)]
        [ValidationGroup("Users")]
        public string UserName { get; set; }

        public string Password { get; set; }
    }
}
