using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;
using MVC4.Sample.Common.Entities;

namespace MVC4.Sample.Common.ViewModels.Home
{
    public class HomeViewModel
    {
        public string Welcome { get; set; }
        public UserListViewModel UserListViewModel { get; set; }
        public Array A { get; set; }
        public ArrayList B { get; set; }
    }
}
