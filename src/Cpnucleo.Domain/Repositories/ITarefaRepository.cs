using Cpnucleo.Domain.Entities;
using System;
using System.Collections.Generic;

namespace Cpnucleo.Domain.Repositories
{
    public interface ITarefaRepository : IGenericRepository<Tarefa>
    {
        IEnumerable<Tarefa> GetByRecurso(Guid idRecurso);
    }
}