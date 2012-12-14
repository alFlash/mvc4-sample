using System;

namespace FluentHibernate.DAO.Models
{
    public class BaseEntity
    {
        public virtual Guid Id { get; set; }
    }
}