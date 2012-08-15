using System;
using System.Collections.Generic;
using System.Web.Mvc;
using MVC.Core.Attributes;
using MVC.Core.Extensions;
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
                UserName = "Gia Le"
            });
            viewModel.UserListViewModel.Users.Add(new UserInfo
            {
                Id = Guid.NewGuid(),
                UserName = "Khoa Tran"
            });
            viewModel.UserListViewModel.Users.Add(new UserInfo
            {
                Id = Guid.NewGuid(),
                UserName = "Duy Truong"
            });
            viewModel.UserListViewModel.Users.Add(new UserInfo
            {
                Id = Guid.NewGuid(),
                UserName = "Hai Nguyen"
            });
            return View(viewModel);
        }

        [HttpPost]
        [ActionCommand(ButtonName = "SaveWholePage")]
        public ActionResult Index(HomeViewModel viewModel)
        {
            ModelState.Clear();
            if (ModelState.IsValid)
            {
                //TODO: Business here!
                viewModel.Welcome = "Saved Welcome Text";
                foreach (var userInfo in viewModel.UserListViewModel.Users)
                {
                    userInfo.UserName = "Saved";
                }
            }
            return View(viewModel);
        }

        [HttpPost]
        [ActionCommand(ButtonName = "ChangeWelcomeText")]
        public ActionResult ChangeWelcomeText(HomeViewModel viewModel)
        {
            ModelState.Clear();
            viewModel.Welcome = "Welcome Text has Changed";
            return View("Index", viewModel);
        }

        [HttpPost]
        [ActionCommand(ButtonName = "SaveUsers")]
        public ActionResult SaveUsers(HomeViewModel viewModel)
        {
            ModelState.Clear();
            //viewModel.UserListViewModel.Users[0].UserName = string.Empty;
            //if (ModelState.IsGroupValid(viewModel.UserListViewModel, "Users"))
            //if (ModelState.IsValidField("UserName"))
            if (ModelState.IsValidGroupRecursive(viewModel, "Users"))
            {
                //TODO: Business here!
                foreach (var userInfo in viewModel.UserListViewModel.Users)
                {
                    userInfo.UserName = "Changed";
                }
            }
            return View("Index", viewModel);
        }

    }
}
