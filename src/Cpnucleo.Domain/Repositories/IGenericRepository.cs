using System;
using System.Collections.Generic;

namespace Cpnucleo.Domain.Repositories
{
    public interface IGenericRepository<TEntity> : IDisposable
    {
        TEntity Add(TEntity entity);

        TEntity Update(TEntity entity);
        
        TEntity Get(Guid id);

        IEnumerable<TEntity> All(bool getDependencies = false);

        void Remove(Guid id);

        void SaveChanges();
    }
}
