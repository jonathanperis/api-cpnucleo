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
    public class RecursoController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;

        public RecursoController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        /// <summary>
        /// Listar recursos
        /// </summary>
        /// <remarks>
        /// # Listar recursos
        /// 
        /// Lista recursos da base de dados.
        /// </remarks>
        /// <response code="200">Retorna uma lista de recursos</response>
        /// <response code="401">Acesso não autorizado</response>
        /// <response code="500">Erro no processamento da requisição</response>
        [HttpGet]
        [ProducesResponseType(200)]
        public IEnumerable<Recurso> Get()
        {
            return _unitOfWork.RecursoRepository.All();
        }

        /// <summary>
        /// Consultar recurso
        /// </summary>
        /// <remarks>
        /// # Consultar recurso
        /// 
        /// Consulta um recurso na base de dados.
        /// </remarks>
        /// <param name="id">Id do recurso</param>        
        /// <response code="200">Retorna um recurso</response>
        /// <response code="404">Recurso não encontrado</response>
        /// <response code="401">Acesso não autorizado</response>
        /// <response code="500">Erro no processamento da requisição</response>
        [HttpGet("{id}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(404)]
        public ActionResult<Recurso> Get(Guid id)
        {
            Recurso recurso = _unitOfWork.RecursoRepository.Get(id);

            if (recurso == null)
            {
                return NotFound();
            }

            return Ok(recurso);
        }

        /// <summary>
        /// Incluir recurso
        /// </summary>
        /// <remarks>
        /// # Incluir recurso
        /// 
        /// Inclui um recurso na base de dados.
        /// 
        /// # Sample request:
        ///
        ///     POST /recurso
        ///     {
        ///        "nome": "Novo recurso",
        ///        "login": "usuario.teste",
        ///        "senha": "12345678",
        ///        "confirmarSenha": "12345678"
        ///     }
        /// </remarks>
        /// <param name="obj">Recurso</param>        
        /// <response code="201">Recurso cadastrado com sucesso</response>
        /// <response code="400">Objetos não preenchidos corretamente</response>
        /// <response code="409">Guid informado já consta na base de dados</response>
        /// <response code="401">Acesso não autorizado</response>
        /// <response code="500">Erro no processamento da requisição</response>
        [HttpPost]
        [ProducesResponseType(201)]
        [ProducesResponseType(400)]
        [ProducesResponseType(409)]
        public ActionResult<Recurso> Post([FromBody]Recurso obj)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                _unitOfWork.RecursoRepository.Add(obj);
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

            return CreatedAtAction(nameof(Get), new { id = obj.Id }, obj);
        }

        /// <summary>
        /// Alterar recurso
        /// </summary>
        /// <remarks>
        /// # Alterar recurso
        /// 
        /// Altera um recurso na base de dados.
        /// 
        /// # Sample request:
        ///
        ///     PUT /recurso
        ///     {
        ///        "id": "fffc0a28-b9e9-4ffd-0053-08d73d64fb91",
        ///        "nome": "Novo recurso - alterado",
        ///        "login": "usuario.teste",
        ///        "senha": "12345678",
        ///        "confirmarSenha": "12345678",
        ///        "dataInclusao": "2019-09-21T19:15:23.519Z"
        ///     }
        /// </remarks>
        /// <param name="id">Id do recurso</param>        
        /// <param name="obj">Recurso</param>        
        /// <response code="204">Recurso alterado com sucesso</response>
        /// <response code="400">ID informado não é válido</response>
        /// <response code="401">Acesso não autorizado</response>
        /// <response code="500">Erro no processamento da requisição</response>
        [HttpPut("{id}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        public IActionResult Put(Guid id, [FromBody]Recurso obj)
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
                _unitOfWork.RecursoRepository.Update(obj);
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
        /// Remover recurso
        /// </summary>
        /// <remarks>
        /// # Remover recurso
        /// 
        /// Remove um recurso da base de dados.
        /// </remarks>
        /// <param name="id">Id do recurso</param>        
        /// <response code="204">Recurso removido com sucesso</response>
        /// <response code="404">Recurso não encontrado</response>
        /// <response code="401">Acesso não autorizado</response>
        /// <response code="500">Erro no processamento da requisição</response>
        [HttpDelete("{id}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        public IActionResult Delete(Guid id)
        {
            Recurso obj = _unitOfWork.RecursoRepository.Get(id);

            if (obj == null)
            {
                return NotFound();
            }

            _unitOfWork.RecursoRepository.Remove(id);

            return NoContent();
        }

        private bool ObjExists(Guid id)
        {
            return _unitOfWork.RecursoRepository.Get(id) != null;
        }
    }
}
