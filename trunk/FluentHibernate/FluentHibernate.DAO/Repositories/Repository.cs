using System;
using System.Collections.Generic;
using NHibernate;
using NHibernate.Criterion;

namespace FluentHibernate.DAO.Repositories
{
    /// <summary>
    /// 
    /// </summary>
    public class Repository : IRepository
    {
        /// <summary>
        /// 
        /// </summary>
        private readonly ISession _session;

        /// <summary>
        /// Initializes a new instance of the <see cref="Repository"/> class.
        /// </summary>
        /// <param name="session">The session.</param>
        public Repository(ISession session)
        {
            _session = session;
        }

        /// <summary>
        /// Gets the by instance.
        /// </summary>
        /// <param name="exampleInstance">The example instance.</param>
        /// <returns></returns>
        public IList<T> GetByInstance<T>(T exampleInstance)
        {
            return _session.CreateCriteria(typeof(T))
                        .Add(Example.Create(exampleInstance))
                        .List<T>();
        }

        /// <summary>
        /// Gets all.
        /// </summary>
        /// <returns></returns>
        public IList<T> GetAll<T>()
        {
            return _session.CreateCriteria(typeof(T)).List<T>();
        }

        /// <summary>
        /// Inserts the or update.
        /// </summary>
        /// <param name="target">The target.</param>
        public void InsertOrUpdate<T>(T target)
        {
            var transaction = _session.BeginTransaction();
            try
            {
                var res = GetByInstance(target);
                if (res != null && res.Count > 0)
                    _session.SaveOrUpdate(target);
                else
                    _session.Save(target);
                transaction.Commit();
            }
            catch (Exception)
            {
                transaction.Rollback();
                throw;
            }
            finally
            {
                transaction.Dispose();
            }
        }
    }
}
