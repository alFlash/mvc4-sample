using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;

namespace Mvc4.SPA.Sample.Web.ViewModels.Models
{
    public class Customer
    {
        [Key]
        public int CustomerId { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
    }
}