using Cpnucleo.Domain.Entities;
using System;
using System.Collections.Generic;

namespace Cpnucleo.Domain.Repositories
{
    public interface IRecursoProjetoRepository : IGenericRepository<RecursoProjeto>
    {
        IEnumerable<RecursoProjeto> GetByProjeto(Guid idProjeto);
    }
}