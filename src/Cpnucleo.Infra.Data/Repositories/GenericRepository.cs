using Cpnucleo.Domain.Entities;
using Cpnucleo.Domain.Repositories;
using Cpnucleo.Infra.Data.Context;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Cpnucleo.Infra.Data.Repositories
{
    internal class GenericRepository<TEntity> : IGenericRepository<TEntity> where TEntity : BaseEntity
    {
        protected readonly CpnucleoContext _context;

        public GenericRepository(CpnucleoContext context)
        {
            _context = context;
        }

        public virtual TEntity Add(TEntity entity)
        {
            if (entity.Id == Guid.Empty)
            {
                entity.Id = Guid.NewGuid();
            }

            entity.Ativo = true;
            entity.DataInclusao = DateTime.Now;

            entity = _context.Add(entity).Entity;

            SaveChanges();

            return entity;
        }

        public virtual bool Update(TEntity entity)
        {
            entity.DataAlteracao = DateTime.Now;

            entity = _context.Update(entity).Entity;

            return SaveChanges();
        }

        public virtual TEntity Get(Guid id)
        {
            return _context.Set<TEntity>()
                .AsQueryable()
                .Include(_context.GetIncludePaths(typeof(TEntity)))
                .FirstOrDefault(x => x.Id == id && x.Ativo);
        }

        public virtual IEnumerable<TEntity> All(bool getDependencies = false)
        {
            IQueryable<TEntity> obj = _context.Set<TEntity>();

            if (getDependencies)
            {
                obj = obj.Include(_context.GetIncludePaths(typeof(TEntity)));
            }

            return obj.OrderBy(x => x.DataInclusao)
                .Where(x => x.Ativo);            
        }

        public bool Remove(Guid id)
        {
            TEntity entity = Get(id);

            entity.Ativo = false;
            entity.DataExclusao = DateTime.Now;

            return Update(entity);            
        }

        public bool SaveChanges()
        {
            return _context.SaveChanges() > 0;
        }

        public void Dispose()
        {
            _context.Dispose();
            GC.SuppressFinalize(this);
        }
    }
}