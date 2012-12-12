using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Routing;
using FluentNHibernate.Cfg;
using FluentNHibernate.Cfg.Db;
using NHibernate;
using NHibernate.Context;

namespace FluentHibernate.Sample
{
    // Note: For instructions on enabling IIS6 or IIS7 classic mode, 
    // visit http://go.microsoft.com/?LinkId=9394801
    public class MvcApplication : HttpApplication
    {
        public static ISessionFactory SessionFactory { get; private set; }
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();

            WebApiConfig.Register(GlobalConfiguration.Configuration);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);

            RegisterSessionFactory();
        }

        protected void Application_BeginRequest(object sender, EventArgs e)
        {
            var session = SessionFactory.OpenSession();
            CurrentSessionContext.Bind(session);
        }

        protected void Application_EndRequest(object sender, EventArgs e)
        {
            var session = CurrentSessionContext.Unbind(SessionFactory);
            session.Dispose();
        }

        private IList<Assembly> GetMappingAssemblies()
        {
            var result = new List<Assembly>();
            foreach (var assembly in AppDomain.CurrentDomain.GetAssemblies())
            {
                assembly.GetTypes();
                foreach (var attribute in assembly.GetCustomAttributes(true))
                {
                    //if (attribute is HibernatePersistenceAssembly)
                    {
                        result.Add(assembly);
                    }
                       
                }
            }
            return result;
        }

        private void RegisterSessionFactory()
        {
            var mappingAssemblies = GetMappingAssemblies();
            if (mappingAssemblies != null && mappingAssemblies.Count > 0)
            {
                SessionFactory = Fluently.Configure().Database(MsSqlConfiguration.MsSql2008.
                        ConnectionString(@"Data Source=.\SQLEXPRESS;Initial Catalog=Test;Persist Security Info=True;"))
                    .Mappings(m => AddMappingAssembly(m, mappingAssemblies))
                    .ExposeConfiguration(c => c.SetProperty("current_session_context_class", "web"))
                    .BuildSessionFactory();
            }
        }

        private static void AddMappingAssembly(MappingConfiguration mappingConfiguration, IEnumerable<Assembly> mappingAssemblies)
        {
            foreach (var assembly in mappingAssemblies)
            {
                mappingConfiguration.FluentMappings.AddFromAssembly(assembly);
            }
        }
    }
}