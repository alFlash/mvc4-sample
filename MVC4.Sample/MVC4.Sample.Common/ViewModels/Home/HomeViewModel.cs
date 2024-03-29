﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;
using MVC.Core.Attributes;
using MVC4.Sample.Common.Entities;

namespace MVC4.Sample.Common.ViewModels.Home
{
    public class HomeViewModel
    {
        [CustomRequired]
        [ValidationGroup("Welcome")]
        public string Welcome { get; set; }
        [CustomRequired]
        [ValidationGroup("Welcome")]
        public string Welcome2 { get; set; }
        public UserListViewModel UserListViewModel { get; set; }
    }
}
