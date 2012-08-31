using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using System.Web.Http.Data;
using System.Web.Http.Data.EntityFramework;
using System.Web.Mvc;
using Mvc4.SPA.Sample.Web.ViewModels.Models;
using Mvc4.SPA.Sample.Web.ViewModels.Repositories;

namespace Mvc4.SPA.Sample.Web.Controllers
{
    public class DeliveryServiceController : DataController
    {
        //
        // GET: /DeliveryService/
        public List<Delivery> GetDeliveries()
        {
            var dbContext = new RepositoryContext();
            dbContext.Configuration.ProxyCreationEnabled = false;
            var result = dbContext.Deliveries.Include("Customer").OrderBy(x => x.DeliveryId).ToList();
            return result;
        }

        public bool InsertDeliveries()
        {
            try
            {
                var dbContext = new RepositoryContext();
                var count = dbContext.Deliveries.Count();
                var entity1 = new Delivery
                    {
                        DeliveryId = count + 1,
                        Customer = new Customer
                            {
                                CustomerId = count + 1,
                                Address = "HCM",
                                Name = string.Format("Entity {0}", count + 1)
                            },
                        Description = string.Format("MVC4 Ebook {0}", count + 1),
                        IsDeliveried = false
                    };
                
                dbContext.Deliveries.Add(entity1);
                dbContext.SaveChanges();
            }
            catch (Exception ex)
            {
                
            }
            return true;
        }
    }
}
