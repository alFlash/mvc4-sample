using System.Collections.Generic;

namespace MVP.Base.BaseRepository
{
    public interface IRepository<TEntity> : IBaseRepository
        where TEntity: class 
    {
        void Add(TEntity entity);
        void Update(TEntity entity);
        void Delete(TEntity entity);
        List<TEntity> GetAll();
    }
}
