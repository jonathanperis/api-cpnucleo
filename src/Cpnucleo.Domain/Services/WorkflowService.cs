﻿using Cpnucleo.Domain.Entities;
using Cpnucleo.Domain.Interfaces.Repositories;
using Cpnucleo.Domain.Interfaces.Services;
using System;
using System.Linq;

namespace Cpnucleo.Domain.Services
{
    public class WorkflowService : CrudService<Workflow>, IWorkflowService
    {
        public WorkflowService(ICrudRepository<Workflow> workflowRepository, IUnitOfWork unitOfWork)
            : base(workflowRepository, unitOfWork)
        {

        }

        public new IQueryable<Workflow> Listar()
        {
            IQueryable<Workflow> lista = base.Listar();

            int quantidadeColunas = ObterQuantidadeColunas();

            foreach (Workflow item in lista)
            {
                item.TamanhoColuna = ObterTamanhoColuna(quantidadeColunas);
            }

            return lista;
        }

        public IQueryable<Workflow> ObterPorTarefa(Guid idTarefa)
        {
            throw new NotImplementedException();
        }

        public int ObterQuantidadeColunas()
        {
            return base.Listar().Count();
        }

        public string ObterTamanhoColuna(int quantidadeColunas)
        {
            quantidadeColunas = quantidadeColunas == 1 ? 2 : quantidadeColunas;

            int i = 12 / quantidadeColunas;
            return i.ToString();
        }
    }
}