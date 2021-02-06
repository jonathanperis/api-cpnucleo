using Cpnucleo.Domain.Entities;
using Cpnucleo.Domain.Repositories;
using Cpnucleo.Infra.Data.Context;
using System.Linq;

namespace Cpnucleo.Infra.Data.Repositories
{
    internal class WorkflowRepository : GenericRepository<Workflow>, IWorkflowRepository
    {
        public WorkflowRepository(CpnucleoContext context)
            : base(context)
        {

        }

        public int GetQuantidadeColunas()
        {
            return All().Count();
        }

        public string GetTamanhoColuna(int colunas)
        {
            colunas = colunas == 1 ? 2 : colunas;

            int i = 12 / colunas;
            return i.ToString();
        }        
    }
}
