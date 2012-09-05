using System.Collections.Generic;

namespace MVP.Base.BaseRepository
{
    public class BaseRepository<TEntity> : IRepository<TEntity>
        where TEntity: class 
    {
        #region Implementation of IRepository<TEntity>

        public void Add(TEntity entity)
        {
            //TODO:
        }

        void IRepository<TEntity>.Update(TEntity entity)
        {
            //TODO:
        }

        void IRepository<TEntity>.Delete(TEntity entity)
        {
            //TODO:
        }

        public List<TEntity> GetAll()
        {
            //TODO:
            return null;
        }

        #endregion
    }
}
