using Cpnucleo.Domain.Entities;
using Cpnucleo.Domain.Repositories;
using Cpnucleo.Infra.Data.Context;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Cpnucleo.Infra.Data.Repositories
{
    internal class ImpedimentoTarefaRepository : GenericRepository<ImpedimentoTarefa>, IImpedimentoTarefaRepository
    {
        public ImpedimentoTarefaRepository(CpnucleoContext context)
            : base(context)
        {

        }

        public IEnumerable<ImpedimentoTarefa> GetByTarefa(Guid idTarefa)
        {
            return All(true)
                .Where(x => x.IdTarefa == idTarefa)
                .ToList();
        }
    }
}
