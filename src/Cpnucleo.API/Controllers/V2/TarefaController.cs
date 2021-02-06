using Cpnucleo.Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using Cpnucleo.Domain.UoW;
using System.Linq;

namespace Cpnucleo.API.Controllers.V2
{
    [Produces("application/json")]
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiController]
    [ApiVersion("2")]
    [Authorize]
    public class TarefaController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;

        public TarefaController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        /// <summary>
        /// Listar tarefas
        /// </summary>
        /// <remarks>
        /// # Listar tarefas
        /// 
        /// Lista tarefas da base de dados.
        /// </remarks>
        /// <param name="getDependencies">Listar dependências do objeto</param>        
        /// <response code="200">Retorna uma lista de tarefas</response>
        /// <response code="401">Acesso não autorizado</response>
        /// <response code="500">Erro no processamento da requisição</response>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public IEnumerable<Tarefa> Get(bool getDependencies = false)
        {
            var result = _unitOfWork.TarefaRepository.All(getDependencies);

            return PreencherDadosAdicionais(result);
        }

        /// <summary>
        /// Consultar tarefa
        /// </summary>
        /// <remarks>
        /// # Consultar tarefa
        /// 
        /// Consulta uma tarefa na base de dados.
        /// </remarks>
        /// <param name="id">Id do tarefa</param>        
        /// <response code="200">Retorna uma tarefa</response>
        /// <response code="404">Tarefa não encontrada</response>
        /// <response code="401">Acesso não autorizado</response>
        /// <response code="500">Erro no processamento da requisição</response>
        [HttpGet("{id}", Name = "GetTarefa")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult<Tarefa> Get(Guid id)
        {
            Tarefa tarefa = _unitOfWork.TarefaRepository.Get(id);

            if (tarefa == null)
            {
                return NotFound();
            }

            return Ok(tarefa);
        }

        /// <summary>
        /// Consultar tarefa por id recurso
        /// </summary>
        /// <remarks>
        /// # Consultar tarefa por id recurso
        /// 
        /// Consulta uma tarefa por id recurso na base de dados.
        /// </remarks>
        /// <param name="id">Id do Recurso</param>        
        /// <response code="200">Retorna uma tarefa</response>
        /// <response code="401">Acesso não autorizado</response>
        /// <response code="500">Erro no processamento da requisição</response>
        [HttpGet("GetByRecurso/{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public IEnumerable<Tarefa> GetByRecurso(Guid id)
        {
            var result = _unitOfWork.TarefaRepository.GetByRecurso(id);

            return PreencherDadosAdicionais(result);
        }

        /// <summary>
        /// Incluir tarefa
        /// </summary>
        /// <remarks>
        /// # Incluir tarefa
        /// 
        /// Inclui uma tarefa na base de dados.
        /// 
        /// # Sample request:
        ///
        ///     POST /tarefa
        ///     {
        ///        "nome": "Nova tarefa",
        ///        "dataInicio": "2019-09-21T15:24:35.117Z",
        ///        "dataTermino": "2019-09-21T15:24:35.117Z",
        ///        "qtdHoras": 8,
        ///        "idProjeto": "fffc0a28-b9e9-4ffd-0053-08d73d64fb91",
        ///        "idWorkflow": "fffc0a28-b9e9-4ffd-0053-08d73d64fb91",
        ///        "idRecurso": "fffc0a28-b9e9-4ffd-0053-08d73d64fb91",
        ///        "idTipoTarefa": "fffc0a28-b9e9-4ffd-0053-08d73d64fb91"
        ///     }
        /// </remarks>
        /// <param name="obj">Tarefa</param>        
        /// <response code="201">Tarefa cadastrada com sucesso</response>
        /// <response code="400">Objetos não preenchidos corretamente</response>
        /// <response code="409">Guid informado já consta na base de dados</response>
        /// <response code="401">Acesso não autorizado</response>
        /// <response code="500">Erro no processamento da requisição</response>
        [HttpPost]
        [ProducesResponseType(typeof(Tarefa), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        public ActionResult<Tarefa> Post([FromBody]Tarefa obj)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                obj = _unitOfWork.TarefaRepository.Add(obj);
            }
            catch (Exception)
            {
                if (ObjExists(obj.Id))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtAction("GetTarefa", new { id = obj.Id }, obj);
        }

        /// <summary>
        /// Alterar tarefa
        /// </summary>
        /// <remarks>
        /// # Alterar tarefa
        /// 
        /// Altera uma tarefa na base de dados.
        /// 
        /// # Sample request:
        ///
        ///     PUT /tarefa
        ///     {
        ///        "id": "fffc0a28-b9e9-4ffd-0053-08d73d64fb91",
        ///        "nome": "Nova tarefa - alterada",
        ///        "dataInicio": "2019-09-21T15:24:35.117Z",
        ///        "dataTermino": "2019-09-21T15:24:35.117Z",
        ///        "qtdHoras": 8,
        ///        "idProjeto": "fffc0a28-b9e9-4ffd-0053-08d73d64fb91",
        ///        "idWorkflow": "fffc0a28-b9e9-4ffd-0053-08d73d64fb91",
        ///        "idRecurso": "fffc0a28-b9e9-4ffd-0053-08d73d64fb91",
        ///        "idTipoTarefa": "fffc0a28-b9e9-4ffd-0053-08d73d64fb91",
        ///        "dataInclusao": "2019-09-21T19:15:23.519Z"
        ///     }
        /// </remarks>
        /// <param name="id">Id da tarefa</param>        
        /// <param name="obj">Tarefa</param>        
        /// <response code="204">Tarefa alterada com sucesso</response>
        /// <response code="400">ID informado não é válido</response>
        /// <response code="401">Acesso não autorizado</response>
        /// <response code="500">Erro no processamento da requisição</response>
        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public IActionResult Put(Guid id, [FromBody]Tarefa obj)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != obj.Id)
            {
                return BadRequest();
            }

            try
            {
                _unitOfWork.TarefaRepository.Update(obj);
            }
            catch (Exception)
            {
                if (!ObjExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        /// <summary>
        /// Alterar tarefa por id workflow
        /// </summary>
        /// <remarks>
        /// # Alterar tarefa por id workflow
        /// 
        /// Altera uma tarefa por id workflow na base de dados.
        /// 
        /// # Sample request:
        ///
        ///     PUT /tarefa/putbyworkflow
        ///     {
        ///        "idWorkflow": "fffc0a28-b9e9-4ffd-0053-08d73d64fb91"
        ///     }
        /// </remarks>
        /// <param name="idTarefa">Id da tarefa</param>        
        /// <param name="idWorkflow">Id do workflow</param>        
        /// <response code="204">Tarefa alterada com sucesso</response>
        /// <response code="400">ID informado não é válido</response>
        /// <response code="401">Acesso não autorizado</response>
        /// <response code="500">Erro no processamento da requisição</response>
        [HttpPut("PutByWorkflow/{idTarefa}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public IActionResult PutByWorkflow(Guid idTarefa, [FromBody]Guid idWorkflow)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                Tarefa tarefa = _unitOfWork.TarefaRepository.Get(idTarefa);
                Workflow workflow = _unitOfWork.WorkflowRepository.Get(idWorkflow);

                tarefa.IdWorkflow = idWorkflow;
                tarefa.Workflow = workflow;

                _unitOfWork.TarefaRepository.Update(tarefa);
            }
            catch (Exception)
            {
                if (!ObjExists(idTarefa))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        /// <summary>
        /// Remover tarefa
        /// </summary>
        /// <remarks>
        /// # Remover tarefa
        /// 
        /// Remove uma tarefa da base de dados.
        /// </remarks>
        /// <param name="id">Id da tarefa</param>        
        /// <response code="204">Tarefa removida com sucesso</response>
        /// <response code="404">Tarefa não encontrada</response>
        /// <response code="401">Acesso não autorizado</response>
        /// <response code="500">Erro no processamento da requisição</response>
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult Delete(Guid id)
        {
            Tarefa obj = _unitOfWork.TarefaRepository.Get(id);

            if (obj == null)
            {
                return NotFound();
            }

            _unitOfWork.TarefaRepository.Remove(id);

            return NoContent();
        }

        private bool ObjExists(Guid id)
        {
            return _unitOfWork.TarefaRepository.Get(id) != null;
        }

        private IEnumerable<Tarefa> PreencherDadosAdicionais(IEnumerable<Tarefa> lista)
        {
            int colunas = _unitOfWork.WorkflowRepository.GetQuantidadeColunas();

            foreach (Tarefa item in lista)
            {
                item.Workflow.TamanhoColuna = _unitOfWork.WorkflowRepository.GetTamanhoColuna(colunas);
                
                item.HorasConsumidas = _unitOfWork.ApontamentoRepository.GetTotalHorasPorRecurso(item.IdRecurso, item.Id);
                item.HorasRestantes = item.QtdHoras - item.HorasConsumidas;

                if (_unitOfWork.ImpedimentoTarefaRepository.GetByTarefa(item.Id).Count() > 0)
                {
                    item.TipoTarefa.Element = "warning-element";
                }
                else if (DateTime.Now.Date >= item.DataInicio && DateTime.Now.Date <= item.DataTermino)
                {
                    item.TipoTarefa.Element = "success-element";
                }
                else if (DateTime.Now.Date > item.DataTermino)
                {
                    item.TipoTarefa.Element = "danger-element";
                }
                else
                {
                    item.TipoTarefa.Element = "info-element";
                }
            }

            return lista;
        }        
    }
}
