﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using MVC4.Sample.Common.Entities;

namespace MVC4.Sample.Common.ViewModels.Home
{
    public class UserListViewModel
    {
        [ValidationGroup("Users")]
        public List<UserInfo> Users { get; set; }

        [Required]
        [ValidationGroup("Users")]
        public string Text { get; set; }
    }
}
