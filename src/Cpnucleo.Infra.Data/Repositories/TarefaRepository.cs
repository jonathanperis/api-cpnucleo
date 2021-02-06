using Cpnucleo.Domain.Entities;
using Cpnucleo.Domain.Repositories;
using Cpnucleo.Infra.Data.Context;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Cpnucleo.Infra.Data.Repositories
{
    internal class TarefaRepository : GenericRepository<Tarefa>, ITarefaRepository
    {
        public TarefaRepository(CpnucleoContext context)
            : base(context)
        {

        }

        public IEnumerable<Tarefa> GetByRecurso(Guid idRecurso)
        {
            return All(true)
                .Select(Tarefa => new
                {
                    Tarefa,
                    // ListaRecursoTarefas = Tarefa.ListaRecursoTarefas
                    //     .Where(p => p.IdRecurso == idRecurso)
                })
                .Select(x => x.Tarefa)
                .ToList();
        }
    }
}
