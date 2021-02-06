using Cpnucleo.Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using Cpnucleo.Domain.UoW;

namespace Cpnucleo.API.Controllers.V2
{
    [Produces("application/json")]
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiController]
    [ApiVersion("2")]
    [Authorize]
    public class WorkflowController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;

        public WorkflowController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        /// <summary>
        /// Listar workflows
        /// </summary>
        /// <remarks>
        /// # Listar workflows
        /// 
        /// Lista workflows da base de dados.
        /// </remarks>
        /// <param name="getDependencies">Listar dependências do objeto</param>        
        /// <response code="200">Retorna uma lista de workflows</response>
        /// <response code="401">Acesso não autorizado</response>
        /// <response code="500">Erro no processamento da requisição</response>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public IEnumerable<Workflow> Get(bool getDependencies = false)
        {
            var result = _unitOfWork.WorkflowRepository.All(getDependencies);

            int colunas = _unitOfWork.WorkflowRepository.GetQuantidadeColunas();

            foreach (Workflow item in result)
            {
                item.TamanhoColuna = _unitOfWork.WorkflowRepository.GetTamanhoColuna(colunas);
            }

            return result;
        }

        /// <summary>
        /// Consultar workflow
        /// </summary>
        /// <remarks>
        /// # Consultar workflow
        /// 
        /// Consulta um workflow na base de dados.
        /// </remarks>
        /// <param name="id">Id do workflow</param>        
        /// <response code="200">Retorna um workflow</response>
        /// <response code="404">Workflow não encontrado</response>
        /// <response code="401">Acesso não autorizado</response>
        /// <response code="500">Erro no processamento da requisição</response>
        [HttpGet("{id}", Name = "GetWorkflow")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult<Workflow> Get(Guid id)
        {
            Workflow workflow = _unitOfWork.WorkflowRepository.Get(id);

            if (workflow == null)
            {
                return NotFound();
            }

            int colunas = _unitOfWork.WorkflowRepository.GetQuantidadeColunas();

            workflow.TamanhoColuna = _unitOfWork.WorkflowRepository.GetTamanhoColuna(colunas);            

            return Ok(workflow);
        }

        /// <summary>
        /// Incluir workflow
        /// </summary>
        /// <remarks>
        /// # Incluir workflow
        /// 
        /// Inclui um workflow na base de dados.
        /// 
        /// # Sample request:
        ///
        ///     POST /workflow
        ///     {
        ///        "nome": "Novo workflow",
        ///        "ordem": "3"
        ///     }
        /// </remarks>
        /// <param name="obj">Workflow</param>        
        /// <response code="201">Workflow cadastrado com sucesso</response>
        /// <response code="400">Objetos não preenchidos corretamente</response>
        /// <response code="409">Guid informado já consta na base de dados</response>
        /// <response code="401">Acesso não autorizado</response>
        /// <response code="500">Erro no processamento da requisição</response>
        [HttpPost]
        [ProducesResponseType(typeof(Workflow), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        public ActionResult<Workflow> Post([FromBody]Workflow obj)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                obj = _unitOfWork.WorkflowRepository.Add(obj);
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

            return CreatedAtAction("GetWorkflow", new { id = obj.Id }, obj);
        }

        /// <summary>
        /// Alterar workflow
        /// </summary>
        /// <remarks>
        /// # Alterar workflow
        /// 
        /// Altera um workflow na base de dados.
        /// 
        /// # Sample request:
        ///
        ///     PUT /workflow
        ///     {
        ///        "id": "fffc0a28-b9e9-4ffd-0053-08d73d64fb91",
        ///        "nome": "Novo workflow - alterado",
        ///        "ordem": "3,
        ///        "dataInclusao": "2019-09-21T19:15:23.519Z"
        ///     }
        /// </remarks>
        /// <param name="id">Id do workflow</param>        
        /// <param name="obj">Workflow</param>        
        /// <response code="204">Workflow alterado com sucesso</response>
        /// <response code="400">ID informado não é válido</response>
        /// <response code="401">Acesso não autorizado</response>
        /// <response code="500">Erro no processamento da requisição</response>
        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public IActionResult Put(Guid id, [FromBody]Workflow obj)
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
                _unitOfWork.WorkflowRepository.Update(obj);
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
        /// Remover workflow
        /// </summary>
        /// <remarks>
        /// # Remover workflow
        /// 
        /// Remove um workflow da base de dados.
        /// </remarks>
        /// <param name="id">Id do workflow</param>        
        /// <response code="204">Workflow removido com sucesso</response>
        /// <response code="404">Workflow não encontrado</response>
        /// <response code="401">Acesso não autorizado</response>
        /// <response code="500">Erro no processamento da requisição</response>
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult Delete(Guid id)
        {
            Workflow obj = _unitOfWork.WorkflowRepository.Get(id);

            if (obj == null)
            {
                return NotFound();
            }

            _unitOfWork.WorkflowRepository.Remove(id);

            return NoContent();
        }

        private bool ObjExists(Guid id)
        {
            return _unitOfWork.WorkflowRepository.Get(id) != null;
        }
    }
}
