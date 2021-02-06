﻿using Cpnucleo.Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using Cpnucleo.Domain.UoW;

namespace Cpnucleo.API.Controllers.V1
{
    [Produces("application/json")]
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiController]
    [ApiVersion("1", Deprecated = true)]
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
        /// <response code="200">Retorna uma lista de workflows</response>
        /// <response code="401">Acesso não autorizado</response>
        /// <response code="500">Erro no processamento da requisição</response>
        [HttpGet]
        [ProducesResponseType(200)]
        public IEnumerable<Workflow> Get()
        {
            return _unitOfWork.WorkflowRepository.All();
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
        [HttpGet("{id}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(404)]
        public ActionResult<Workflow> Get(Guid id)
        {
            Workflow workflow = _unitOfWork.WorkflowRepository.Get(id);

            if (workflow == null)
            {
                return NotFound();
            }

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
        [ProducesResponseType(201)]
        [ProducesResponseType(400)]
        [ProducesResponseType(409)]
        public ActionResult<Workflow> Post([FromBody]Workflow obj)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                _unitOfWork.WorkflowRepository.Add(obj);
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

            return CreatedAtRoute(nameof(Get), new { id = obj.Id }, obj);
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
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
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
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
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
