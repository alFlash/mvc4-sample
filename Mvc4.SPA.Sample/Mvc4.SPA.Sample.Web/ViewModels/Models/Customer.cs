using System.Runtime.Serialization;

namespace Mvc4.SPA.Sample.Web.ViewModels.Models
{
    [DataContract]
    public class Customer
    {
        [DataMember]
        public int CustomerId { get; set; }
        [DataMember]
        public string Name { get; set; }
        [DataMember]
        public string Address { get; set; }
    }
}