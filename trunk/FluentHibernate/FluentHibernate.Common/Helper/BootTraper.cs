using System;
using System.Configuration;
using System.Linq;
using System.Web.Mvc;
using FluentHibernate.Common.IOC;
using FluentHibernate.DAO.Repositories;
using FluentNHibernate.Cfg;
using FluentNHibernate.Cfg.Db;
using Microsoft.Practices.Unity;
using NHibernate;

namespace FluentHibernate.Common.Helper
{
    public class BootTraper : IBootTraper
    {
        private readonly IUnityContainer _container;
        public BootTraper(IUnityContainer container)
        {
            _container = container;
        }

        /// <summary>
        /// Starts this instance.
        /// </summary>
        public void Start()
        {
            _container.RegisterType<ISessionFactory>(new ContainerControlledLifetimeManager(), new InjectionFactory(c => RegisterSessionFactory()));
            _container.RegisterType<ISession>(new ContainerControlledLifetimeManager(), new InjectionFactory(c =>
            {
                var sessionFactory = _container.Resolve<ISessionFactory>();
                return sessionFactory.OpenSession();
            }));
            _container.RegisterType(typeof(IRepository), typeof(Repository), new ContainerControlledLifetimeManager());
            DependencyResolver.SetResolver(new UnityDependencyResolver(_container));
        }

        /// <summary>
        /// Registers the session factory.
        /// </summary>
        /// <returns></returns>
        private static ISessionFactory RegisterSessionFactory()
        {
            return Fluently.Configure()
                .Database(MsSqlConfiguration.MsSql2008.ConnectionString(ConfigurationManager.ConnectionStrings["DefaultDb"].ConnectionString))
                .Mappings(AddAssemblies)
                .ExposeConfiguration(cfg => cfg.SetProperty(NHibernate.Cfg.Environment.CurrentSessionContextClass, "web"))
                .BuildSessionFactory();
        }

        private static void AddAssemblies(MappingConfiguration mappingConfiguration)
        {
            var assemblies = AppDomain.CurrentDomain.GetAssemblies()
                .Where(x => !string.IsNullOrWhiteSpace(x.FullName) && x.FullName.IndexOf("FluentHibernate.DAO", StringComparison.Ordinal) != -1)
                .ToList();
            if (assemblies.Count > 0)
            {
                foreach (var assembly in assemblies)
                {
                    mappingConfiguration.FluentMappings.AddFromAssembly(assembly);  
                }
            }
        }
    }
}
