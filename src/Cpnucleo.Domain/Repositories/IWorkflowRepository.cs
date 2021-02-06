using Cpnucleo.Domain.Entities;

namespace Cpnucleo.Domain.Repositories
{
    public interface IWorkflowRepository : IGenericRepository<Workflow>
    {
        int GetQuantidadeColunas();

        string GetTamanhoColuna(int quantidadeColunas);
    }
}