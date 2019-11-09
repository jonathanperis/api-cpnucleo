﻿using Cpnucleo.Application.AutoMapper;
using Cpnucleo.Application.Interfaces;
using Cpnucleo.Application.Services;
using Cpnucleo.Domain.Interfaces;
using Cpnucleo.Domain.Models;
using Cpnucleo.Infra.CrossCutting.Communication.Interfaces;
using Cpnucleo.Infra.CrossCutting.Communication.Services;
using Cpnucleo.Infra.CrossCutting.Identity;
using Cpnucleo.Infra.CrossCutting.Identity.Interfaces;
using Cpnucleo.Infra.CrossCutting.Util;
using Cpnucleo.Infra.CrossCutting.Util.Interfaces;
using Cpnucleo.Infra.CrossCutting.Util.ViewModels;
using Cpnucleo.Infra.Data.Context;
using Cpnucleo.Infra.Data.Repository;
using Cpnucleo.Infra.Data.UoW;
using Cpnucleo.Infra.Security;
using Cpnucleo.Infra.Security.Interfaces;
using Microsoft.Extensions.DependencyInjection;

namespace Cpnucleo.Infra.CrossCutting.IoC
{
    public static class IServiceCollectionExtension
    {
        public static IServiceCollection AddCpnucleoSetup(this IServiceCollection services)
        {
            // Application
            services
                .AddScoped<ICrudAppService<SistemaViewModel>, CrudAppService<Sistema, SistemaViewModel>>()
                .AddScoped<ICrudAppService<ProjetoViewModel>, CrudAppService<Projeto, ProjetoViewModel>>()
                .AddScoped<ICrudAppService<TarefaViewModel>, CrudAppService<Tarefa, TarefaViewModel>>()
                .AddScoped<ICrudAppService<ApontamentoViewModel>, CrudAppService<Apontamento, ApontamentoViewModel>>()
                .AddScoped<ICrudAppService<WorkflowViewModel>, CrudAppService<Workflow, WorkflowViewModel>>()
                .AddScoped<ICrudAppService<RecursoViewModel>, CrudAppService<Recurso, RecursoViewModel>>()
                .AddScoped<ICrudAppService<ImpedimentoViewModel>, CrudAppService<Impedimento, ImpedimentoViewModel>>()
                .AddScoped<ICrudAppService<ImpedimentoTarefaViewModel>, CrudAppService<ImpedimentoTarefa, ImpedimentoTarefaViewModel>>()
                .AddScoped<ICrudAppService<RecursoProjetoViewModel>, CrudAppService<RecursoProjeto, RecursoProjetoViewModel>>()
                .AddScoped<ICrudAppService<RecursoTarefaViewModel>, CrudAppService<RecursoTarefa, RecursoTarefaViewModel>>()
                .AddScoped<ICrudAppService<TipoTarefaViewModel>, CrudAppService<TipoTarefa, TipoTarefaViewModel>>();

            services
                .AddScoped<ISistemaAppService, SistemaAppService>()
                .AddScoped<IProjetoAppService, ProjetoAppService>()
                .AddScoped<ITarefaAppService, TarefaAppService>()
                .AddScoped<IApontamentoAppService, ApontamentoAppService>()
                .AddScoped<IWorkflowAppService, WorkflowAppService>()
                .AddScoped<IRecursoAppService, RecursoAppService>()
                .AddScoped<IImpedimentoAppService, ImpedimentoAppService>()
                .AddScoped<IImpedimentoTarefaAppService, ImpedimentoTarefaAppService>()
                .AddScoped<IRecursoProjetoAppService, RecursoProjetoAppService>()
                .AddScoped<IRecursoTarefaAppService, RecursoTarefaAppService>()
                .AddScoped<ITipoTarefaAppService, TipoTarefaAppService>();

            services.AddAutoMapperSetup();

            // Infra - Data
            services
                .AddScoped<ICrudRepository<Sistema>, CrudRepository<Sistema>>()
                .AddScoped<ICrudRepository<Projeto>, CrudRepository<Projeto>>()
                .AddScoped<ICrudRepository<Tarefa>, CrudRepository<Tarefa>>()
                .AddScoped<ICrudRepository<Apontamento>, CrudRepository<Apontamento>>()
                .AddScoped<ICrudRepository<Workflow>, CrudRepository<Workflow>>()
                .AddScoped<ICrudRepository<Recurso>, CrudRepository<Recurso>>()
                .AddScoped<ICrudRepository<Impedimento>, CrudRepository<Impedimento>>()
                .AddScoped<ICrudRepository<ImpedimentoTarefa>, CrudRepository<ImpedimentoTarefa>>()
                .AddScoped<ICrudRepository<RecursoProjeto>, CrudRepository<RecursoProjeto>>()
                .AddScoped<ICrudRepository<RecursoTarefa>, CrudRepository<RecursoTarefa>>()
                .AddScoped<ICrudRepository<TipoTarefa>, CrudRepository<TipoTarefa>>();

            services
                .AddScoped<ISistemaRepository, SistemaRepository>()
                .AddScoped<IProjetoRepository, ProjetoRepository>()
                .AddScoped<ITarefaRepository, TarefaRepository>()
                .AddScoped<IApontamentoRepository, ApontamentoRepository>()
                .AddScoped<IWorkflowRepository, WorkflowRepository>()
                .AddScoped<IRecursoRepository, RecursoRepository>()
                .AddScoped<IImpedimentoRepository, ImpedimentoRepository>()
                .AddScoped<IImpedimentoTarefaRepository, ImpedimentoTarefaRepository>()
                .AddScoped<IRecursoProjetoRepository, RecursoProjetoRepository>()
                .AddScoped<IRecursoTarefaRepository, RecursoTarefaRepository>()
                .AddScoped<ITipoTarefaRepository, TipoTarefaRepository>();

            services.AddScoped<IUnitOfWork, UnitOfWork>();
            services.AddScoped<CpnucleoContext>();

            // Infra - Security
            services.AddScoped<ICryptographyManager, CryptographyManager>();
            services.AddScoped<IJwtManager, JwtManager>();

            // Infra - CrossCutting - Identity
            services.AddScoped<IClaimsManager, ClaimsManager>();

            // Infra - CrossCutting - Util
            services.AddScoped<ISystemConfiguration, SystemConfiguration>();

            // Infra - CrossCutting - Communication
            services
                .AddScoped<ISistemaApiService, SistemaApiService>()
                .AddScoped<IProjetoApiService, ProjetoApiService>()
                .AddScoped<ITarefaApiService, TarefaApiService>()
                .AddScoped<IApontamentoApiService, ApontamentoApiService>()
                .AddScoped<IWorkflowApiService, WorkflowApiService>()
                .AddScoped<IRecursoApiService, RecursoApiService>()
                .AddScoped<IImpedimentoApiService, ImpedimentoApiService>()
                .AddScoped<IImpedimentoTarefaApiService, ImpedimentoTarefaApiService>()
                .AddScoped<IRecursoProjetoApiService, RecursoProjetoApiService>()
                .AddScoped<IRecursoTarefaApiService, RecursoTarefaApiService>()
                .AddScoped<ITipoTarefaApiService, TipoTarefaApiService>();

            return services;
        }
    }
}
