using Cpnucleo.Domain.Entities;
using System;
using System.Collections.Generic;

namespace Cpnucleo.Domain.Repositories
{
    public interface IApontamentoRepository : IGenericRepository<Apontamento>
    {
        int GetTotalHorasPorRecurso(Guid idRecurso, Guid idTarefa);

        IEnumerable<Apontamento> GetByRecurso(Guid idRecurso);
    }
}