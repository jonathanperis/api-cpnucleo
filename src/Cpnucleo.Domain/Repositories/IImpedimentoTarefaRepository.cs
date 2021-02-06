using Cpnucleo.Domain.Entities;
using System;
using System.Collections.Generic;

namespace Cpnucleo.Domain.Repositories
{
    public interface IImpedimentoTarefaRepository : IGenericRepository<ImpedimentoTarefa>
    {
        IEnumerable<ImpedimentoTarefa> GetByTarefa(Guid idTarefa);
    }
}