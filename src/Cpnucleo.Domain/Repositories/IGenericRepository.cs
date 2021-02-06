using System;
using System.Collections.Generic;

namespace Cpnucleo.Domain.Repositories
{
    public interface IGenericRepository<TEntity> : IDisposable
    {
        TEntity Add(TEntity entity);

        bool Update(TEntity entity);
        
        TEntity Get(Guid id);

        IEnumerable<TEntity> All(bool getDependencies = false);

        bool Remove(Guid id);

        bool SaveChanges();
    }
}
