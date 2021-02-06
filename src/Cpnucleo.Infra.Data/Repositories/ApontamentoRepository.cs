using Cpnucleo.Domain.Entities;
using Cpnucleo.Domain.Repositories;
using Cpnucleo.Infra.Data.Context;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Cpnucleo.Infra.Data.Repositories
{
    internal class ApontamentoRepository : GenericRepository<Apontamento>, IApontamentoRepository
    {
        public ApontamentoRepository(CpnucleoContext context)
            : base(context)
        {

        }

        public IEnumerable<Apontamento> GetByRecurso(Guid idRecurso)
        {
            return All(true)
                .Where(x => x.IdRecurso == idRecurso && x.DataApontamento.Value > DateTime.Now.AddDays(-30))
                .ToList();
        }

        public int GetTotalHorasPorRecurso(Guid idRecurso, Guid idTarefa)
        {
            return All(true)
                .Where(x => x.IdRecurso == idRecurso && x.IdTarefa == idTarefa)
                .Sum(x => x.QtdHoras);
        }
    }
}
