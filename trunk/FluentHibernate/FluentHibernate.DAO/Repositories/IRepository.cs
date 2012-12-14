using System.Collections.Generic;

namespace FluentHibernate.DAO.Repositories
{
    public interface IRepository
    {
        IList<T> GetAll<T>();
        IList<T> GetByInstance<T>(T instance);
        void InsertOrUpdate<T>(T target);
    }
}
