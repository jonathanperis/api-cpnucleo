using Cpnucleo.Domain.Entities;
using System;
using System.Collections.Generic;

namespace Cpnucleo.Domain.Repositories
{
    public interface IRecursoTarefaRepository : IGenericRepository<RecursoTarefa>
    {
        IEnumerable<RecursoTarefa> GetByTarefa(Guid idTarefa);
    }
}