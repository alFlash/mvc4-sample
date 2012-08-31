using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using Mvc4.SPA.Sample.Web.ViewModels.Models;

namespace Mvc4.SPA.Sample.Web.ViewModels.Repositories
{
    public partial class RepositoryContext : DbContext
    {
        public DbSet<Customer> Customers { get; set; }
        public DbSet<Delivery> Deliveries { get; set; }
    }
}