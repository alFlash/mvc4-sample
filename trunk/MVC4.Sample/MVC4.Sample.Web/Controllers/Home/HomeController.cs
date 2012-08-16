using System;
using System.Collections;
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
                Welcome2 = "Welcome2 to HomePage",
                UserListViewModel = new UserListViewModel
                            {
                                Users = new List<UserInfo>(),
                                Text = "dsafasdfasdfsd"
                            },
                B = new ArrayList { new[] { "dafasdfasdf", "11111" } }
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
            var isValid = ModelState.IsModelValid(viewModel /*Validate All Groups*/);
            //ModelState.IsModelValid(viewModel, "Users Welcome") => Validate Groups: Users & Welcome
            if (isValid)
            {
                //TODO: DO BUSINESS HERE
            }
            return View(viewModel);
        }

        [HttpPost]
        [ActionCommand(ButtonName = "ChangeWelcomeText")]
        public ActionResult ChangeWelcomeText(HomeViewModel viewModel)
        {
            ModelState.Clear();

            //Make wrong data
            viewModel.Welcome = string.Empty;
            viewModel.Welcome2 = string.Empty;
            viewModel.UserListViewModel.Text = string.Empty;
            viewModel.UserListViewModel.Users[0].UserName = string.Empty;
            viewModel.UserListViewModel.Users[1].UserName = string.Empty;

            ModelState.IsModelValid(viewModel, "Welcome" /*Validate only Welcome Groups*/);
            return View("Index", viewModel);
        }

        [HttpPost]
        [ActionCommand(ButtonName = "SaveUsers")]
        public ActionResult SaveUsers(HomeViewModel viewModel)
        {
            ModelState.Clear();

            //Make wrong data
            viewModel.Welcome = string.Empty;
            viewModel.UserListViewModel.Users[0].UserName = "dafsadfsdfsafsadfsadfsadfsadf";
            viewModel.UserListViewModel.Users[1].UserName = "dafsadfsdfsafsadfsadfsadfsadf";

            //Validate
            var isValid = ModelState.IsModelValid(viewModel, "Users" /*Validate Group: Users*/);
            if (isValid)
            {
                //Do business
            }
            return View("Index", viewModel);
        }


    }
}
