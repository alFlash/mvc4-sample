﻿using System.ComponentModel.DataAnnotations;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using MVC.Core.Attributes;
using MVC.Core.Validators;
using MVC4.Sample.Web.App_Start;

namespace MVC4.Sample.Web
{
    // Note: For instructions on enabling IIS6 or IIS7 classic mode, 
    // visit http://go.microsoft.com/?LinkId=9394801
    public class MvcApplication : HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            DataAnnotationsModelValidatorProvider.RegisterAdapter(typeof(CustomRequiredAttribute), typeof(CustomRequiredValidator));
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
        }
    }
}