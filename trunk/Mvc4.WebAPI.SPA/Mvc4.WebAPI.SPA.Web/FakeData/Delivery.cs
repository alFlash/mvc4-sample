using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Mvc4.WebAPI.SPA.Web.FakeData
{
    public class Delivery
    {
        public int DeliveryId { get; set; }
        public virtual int CustomerId { get; set; }
        public virtual Customer Customer { get; set; }

        public string Description { get; set; }
        public bool IsDelivered { get; set; }
    }

    public class Customer
    {
        public int CustomerId { get; set; }
        public virtual int Name { get; set; }
        public virtual int Address { get; set; }
    }
}