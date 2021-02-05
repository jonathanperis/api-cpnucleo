using Cpnucleo.Domain.Entities;
using Cpnucleo.Domain.Interfaces.Repositories;
using Cpnucleo.Infra.Data.Context;
using Cpnucleo.Infra.Data.Repositories;
using System;

namespace Cpnucleo.Infra.Data.UoW
{
    internal class UnitOfWork : IUnitOfWork
    {
        private readonly CpnucleoContext _context;

        public UnitOfWork(CpnucleoContext context)
        {
            _context = context;
        }

        private IApontamentoRepository _apontamentoRepository;
        private ICrudRepository<Impedimento> _impedimentoRepository;
        private IImpedimentoTarefaRepository _impedimentoTarefaRepository;
        private ICrudRepository<Projeto> _projetoRepository;
        private IRecursoRepository _recursoRepository;
        private IRecursoProjetoRepository _recursoProjetoRepository;
        private IRecursoTarefaRepository _recursoTarefaRepository;
        private ICrudRepository<Sistema> _sistemaRepository;
        private ITarefaRepository _tarefaRepository;
        private ICrudRepository<TipoTarefa> _tipoTarefaRepository;
        private ICrudRepository<Workflow> _workflowRepository;

        public IApontamentoRepository ApontamentoRepository
        {
            get
            {
                if (_apontamentoRepository == null)
                    _apontamentoRepository = new ApontamentoRepository(_context);

                return _apontamentoRepository;
            }
        }

        public ICrudRepository<Impedimento> ImpedimentoRepository
        {
            get
            {
                if (_impedimentoRepository == null)
                    _impedimentoRepository = new CrudRepository<Impedimento>(_context);

                return _impedimentoRepository;
            }
        }

        public IImpedimentoTarefaRepository ImpedimentoTarefaRepository
        {
            get
            {
                if (_impedimentoTarefaRepository == null)
                    _impedimentoTarefaRepository = new ImpedimentoTarefaRepository(_context);

                return _impedimentoTarefaRepository;
            }
        }

        public ICrudRepository<Projeto> ProjetoRepository
        {
            get
            {
                if (_projetoRepository == null)
                    _projetoRepository = new CrudRepository<Projeto>(_context);

                return _projetoRepository;
            }
        }

        public IRecursoRepository RecursoRepository
        {
            get
            {
                if (_recursoRepository == null)
                    _recursoRepository = new RecursoRepository(_context);

                return _recursoRepository;
            }
        }

        public IRecursoProjetoRepository RecursoProjetoRepository
        {
            get
            {
                if (_recursoProjetoRepository == null)
                    _recursoProjetoRepository = new RecursoProjetoRepository(_context);

                return _recursoProjetoRepository;
            }
        }        

        public IRecursoTarefaRepository RecursoTarefaRepository
        {
            get
            {
                if (_recursoTarefaRepository == null)
                    _recursoTarefaRepository = new RecursoTarefaRepository(_context);

                return _recursoTarefaRepository;
            }
        }     

        public ICrudRepository<Sistema> SistemaRepository
        {
            get
            {
                if (_sistemaRepository == null)
                    _sistemaRepository = new CrudRepository<Sistema>(_context);

                return _sistemaRepository;
            }
        }

        public ITarefaRepository TarefaRepository
        {
            get
            {
                if (_tarefaRepository == null)
                    _tarefaRepository = new TarefaRepository(_context);

                return _tarefaRepository;
            }
        }           

        public ICrudRepository<TipoTarefa> TipoTarefaRepository
        {
            get
            {
                if (_tipoTarefaRepository == null)
                    _tipoTarefaRepository = new CrudRepository<TipoTarefa>(_context);

                return _tipoTarefaRepository;
            }
        }

        public ICrudRepository<Workflow> WorkflowRepository
        {
            get
            {
                if (_workflowRepository == null)
                    _workflowRepository = new CrudRepository<Workflow>(_context);

                return _workflowRepository;
            }
        }
        
        public bool Commit()
        {
            return _context.SaveChanges() > 0;
        }

        public void Dispose()
        {
            _context.Dispose();
            GC.SuppressFinalize(this);
        }
    }
}
