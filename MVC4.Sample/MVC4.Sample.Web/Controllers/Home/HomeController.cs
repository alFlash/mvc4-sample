using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MVC.Core.Attributes;
using MVC4.Sample.Common.Entities;
using MVC4.Sample.Common.ViewModels.Home;

namespace MVC4.Sample.Web.Controllers.Home
{
    public class HomeController : Controller
    {
        //
        // GET: /Home/

        public ActionResult Index()
        {
            var viewModel = new HomeViewModel
            {
                Welcome = "Welcome to HomePage",
                UserListViewModel = new UserListViewModel
                            {
                                Users = new List<UserInfo>()
                            }
            };
            viewModel.UserListViewModel.Users.Add(new UserInfo
            {
                Id = Guid.NewGuid(),
                UserName = "Le Huu Hoang Gia"
            });
            viewModel.UserListViewModel.Users.Add(new UserInfo
            {
                Id = Guid.NewGuid(),
                UserName = "Khoa Tran Viet"
            });
            viewModel.UserListViewModel.Users.Add(new UserInfo
            {
                Id = Guid.NewGuid(),
                UserName = "Duy Truong Nguyen Bao"
            });
            viewModel.UserListViewModel.Users.Add(new UserInfo
            {
                Id = Guid.NewGuid(),
                UserName = "Hai Nguyen Tuan"
            });
            return View(viewModel);
        }

        [HttpPost]
        //[ActionCommand(ButtonName = "SaveWholePage")]
        public ActionResult Index(HomeViewModel viewModel)
        {
            return View(viewModel);
        }

        [HttpPost]
        [ActionCommand(ButtonName = "ChangeWelcomeText")]
        public ActionResult ChangeWelcomeText(HomeViewModel viewModel)
        {
            return View("Index", viewModel);
        }

        [HttpPost]
        [ActionCommand(ButtonName = "SaveUsers")]
        public ActionResult SaveUsers(HomeViewModel viewModel)
        {
            return View("Index", viewModel);
        }

    }
}
