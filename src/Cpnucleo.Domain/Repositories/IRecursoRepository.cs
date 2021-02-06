using Cpnucleo.Domain.Entities;
using System.Collections.Generic;

namespace Cpnucleo.Domain.Repositories
{
    public interface IRecursoRepository : IGenericRepository<Recurso>
    {
        Recurso GetByLogin(string login);
    }
}