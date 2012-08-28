using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MVC4.Sample.Common.Entities;
using MVC4.Sample.Common.ViewModels.Home;

namespace MVC4.Sample.Web.Controllers.Home
{
    public class JQueryAjaxController : Controller
    {
        //
        // GET: /JQueryAjax/

        public ActionResult Index()
        {
            return View("~/Views/Home/JQueryAjaxView.cshtml");
        }

        [HttpPost]
        public JsonResult GetUserInfo(List<UserInfo> userInfos)
        {
            return Json(userInfos, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult GetOneUser(UserInfo userInfo)
        {
            return Json(userInfo, JsonRequestBehavior.AllowGet);
        }
    }
}
