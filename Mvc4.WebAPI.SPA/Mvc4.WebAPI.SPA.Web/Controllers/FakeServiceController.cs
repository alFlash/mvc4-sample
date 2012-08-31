using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http.Data.EntityFramework;
using System.Web.Mvc;
using Mvc4.WebAPI.SPA.Web.FakeData;
using Mvc4.WebAPI.SPA.Web.Models;
using System.Web.Http;

namespace Mvc4.WebAPI.SPA.Web.Controllers
{
    //DbDataController<AppDbContext>
    public class FakeServiceController : DbDataController<AppDbContext>
    {
        //
        // GET: /FakeService/
        public IQueryable<Delivery> GetDeliveries()
        {
            return DbContext.Deliveries.OrderBy(x => x.DeliveryId);
        }

        //public void InsertDelivery(Delivery delivery) { InsertEntity(delivery); }
        //public void DeleteDelivery(Delivery delivery) { DeleteEntity(delivery); }
        //public void UpdateDelivery(Delivery delivery) { UpdateEntity(delivery); }
    }
}
