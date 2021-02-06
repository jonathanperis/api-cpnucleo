using Cpnucleo.Domain.Entities;
using Cpnucleo.Domain.Repositories;
using Cpnucleo.Infra.Data.Context;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Cpnucleo.Infra.Data.Repositories
{
    internal class RecursoProjetoRepository : GenericRepository<RecursoProjeto>, IRecursoProjetoRepository
    {
        public RecursoProjetoRepository(CpnucleoContext context)
            : base(context)
        {

        }

        public IEnumerable<RecursoProjeto> GetByProjeto(Guid idProjeto)
        {
            return All(true)
                .Where(x => x.IdProjeto == idProjeto)
                .ToList();
        }
    }
}
