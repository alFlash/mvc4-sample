using System.Runtime.Serialization;

namespace Mvc4.SPA.Sample.Web.ViewModels.Models
{
    [DataContract]
    public class Delivery
    {
        [DataMember]
        public int DeliveryId { get; set; }
        [DataMember]
        public virtual int CustomerId { get; set; }
        [DataMember]
        public virtual Customer Customer { get; set; }
        [DataMember]
        public string Description { get; set; }
        [DataMember]
        public bool IsDeliveried { get; set; }
    }
}