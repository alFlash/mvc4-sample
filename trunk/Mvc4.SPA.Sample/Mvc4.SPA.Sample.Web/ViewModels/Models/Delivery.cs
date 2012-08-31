using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;

namespace Mvc4.SPA.Sample.Web.ViewModels.Models
{
    public class Delivery
    {
        [Key]
        public int DeliveryId { get; set; }
        public virtual int CustomerId { get; set; }
        public virtual Customer Customer { get; set; }
        public string Description { get; set; }
        public bool IsDeliveried { get; set; }
    }
}