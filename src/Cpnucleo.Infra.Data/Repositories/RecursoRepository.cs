using Cpnucleo.Domain.Entities;
using Cpnucleo.Domain.Repositories;
using Cpnucleo.Infra.Data.Context;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace Cpnucleo.Infra.Data.Repositories
{
    internal class RecursoRepository : GenericRepository<Recurso>, IRecursoRepository
    {
        public RecursoRepository(CpnucleoContext context)
            : base(context)
        {

        }

        public Recurso GetByLogin(string login)
        {
            return _context.Set<Recurso>()
                .AsQueryable()
                .Include(_context.GetIncludePaths(typeof(Recurso)))
                .FirstOrDefault(x => x.Login == login && x.Ativo);
        }
    }
}
