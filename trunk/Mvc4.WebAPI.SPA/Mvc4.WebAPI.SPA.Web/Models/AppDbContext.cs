using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using Mvc4.WebAPI.SPA.Web.FakeData;

namespace Mvc4.WebAPI.SPA.Web.Models
{
    public partial class AppDbContext : DbContext
    {
        public DbSet<Customer> Customers { get; set; }
        public DbSet<Delivery> Deliveries { get; set; }
    }
}