using System.Collections.Generic;

namespace FluentHibernate.DAO.Models
{
    public class People : BaseEntity
    {
        public virtual string FirstName { get; set; }
        public virtual string LastName { get; set; }
        public virtual IList<People> Children { get; set; } 
    }
}