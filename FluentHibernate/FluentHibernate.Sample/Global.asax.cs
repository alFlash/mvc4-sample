﻿using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Routing;
using FluentHibernate.Common.Helper;
using FluentHibernate.Sample.App_Start;
using Microsoft.Practices.Unity;

namespace FluentHibernate.Sample
{
    // Note: For instructions on enabling IIS6 or IIS7 classic mode, 
    // visit http://go.microsoft.com/?LinkId=9394801
    public class MvcApplication : HttpApplication
    {
        private IBootTraper _bootTraper;

        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();

            WebApiConfig.Register(GlobalConfiguration.Configuration);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);

            _bootTraper = new BootTraper(new UnityContainer());
            _bootTraper.Start();
        }
    }
}