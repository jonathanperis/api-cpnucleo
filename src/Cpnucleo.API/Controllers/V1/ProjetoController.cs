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
    public class ProjetoController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;

        public ProjetoController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        /// <summary>
        /// Listar projetos
        /// </summary>
        /// <remarks>
        /// # Listar projetos
        /// 
        /// Lista projetos da base de dados.
        /// </remarks>
        /// <response code="200">Retorna uma lista de projetos</response>
        /// <response code="401">Acesso não autorizado</response>
        /// <response code="500">Erro no processamento da requisição</response>
        [HttpGet]
        [ProducesResponseType(200)]
        public IEnumerable<Projeto> Get()
        {
            return _unitOfWork.ProjetoRepository.All();
        }

        /// <summary>
        /// Consultar projeto
        /// </summary>
        /// <remarks>
        /// # Consultar projeto
        /// 
        /// Consulta um projeto na base de dados.
        /// </remarks>
        /// <param name="id">Id do projeto</param>        
        /// <response code="200">Retorna um projeto</response>
        /// <response code="404">Projeto não encontrado</response>
        /// <response code="401">Acesso não autorizado</response>
        /// <response code="500">Erro no processamento da requisição</response>
        [HttpGet("{id}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(404)]
        public ActionResult<Projeto> Get(Guid id)
        {
            Projeto projeto = _unitOfWork.ProjetoRepository.Get(id);

            if (projeto == null)
            {
                return NotFound();
            }

            return Ok(projeto);
        }

        /// <summary>
        /// Incluir projeto
        /// </summary>
        /// <remarks>
        /// # Incluir projeto
        /// 
        /// Inclui um projeto na base de dados.
        /// 
        /// # Sample request:
        ///
        ///     POST /projeto
        ///     {
        ///        "nome": "Novo projeto",
        ///        "idSistema": "fffc0a28-b9e9-4ffd-0053-08d73d64fb91"
        ///     }
        /// </remarks>
        /// <param name="obj">Projeto</param>        
        /// <response code="201">Projeto cadastrado com sucesso</response>
        /// <response code="400">Objetos não preenchidos corretamente</response>
        /// <response code="409">Guid informado já consta na base de dados</response>
        /// <response code="401">Acesso não autorizado</response>
        /// <response code="500">Erro no processamento da requisição</response>
        [HttpPost]
        [ProducesResponseType(201)]
        [ProducesResponseType(400)]
        [ProducesResponseType(409)]
        public ActionResult<Projeto> Post([FromBody]Projeto obj)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                _unitOfWork.ProjetoRepository.Add(obj);
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
        /// Alterar projeto
        /// </summary>
        /// <remarks>
        /// # Alterar projeto
        /// 
        /// Altera um projeto na base de dados.
        /// 
        /// # Sample request:
        ///
        ///     PUT /projeto
        ///     {
        ///        "id": "fffc0a28-b9e9-4ffd-0053-08d73d64fb91",
        ///        "nome": "Novo projeto - alterado",
        ///        "idSistema": "fffc0a28-b9e9-4ffd-0053-08d73d64fb91",
        ///        "dataInclusao": "2019-09-21T19:15:23.519Z"
        ///     }
        /// </remarks>
        /// <param name="id">Id do projeto</param>        
        /// <param name="obj">Projeto</param>        
        /// <response code="204">Projeto alterado com sucesso</response>
        /// <response code="400">ID informado não é válido</response>
        /// <response code="401">Acesso não autorizado</response>
        /// <response code="500">Erro no processamento da requisição</response>
        [HttpPut("{id}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        public IActionResult Put(Guid id, [FromBody]Projeto obj)
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
                _unitOfWork.ProjetoRepository.Update(obj);
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
        /// Remover projeto
        /// </summary>
        /// <remarks>
        /// # Remover projeto
        /// 
        /// Remove um projeto da base de dados.
        /// </remarks>
        /// <param name="id">Id do projeto</param>        
        /// <response code="204">Projeto removido com sucesso</response>
        /// <response code="404">Projeto não encontrado</response>
        /// <response code="401">Acesso não autorizado</response>
        /// <response code="500">Erro no processamento da requisição</response>
        [HttpDelete("{id}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        public IActionResult Delete(Guid id)
        {
            Projeto obj = _unitOfWork.ProjetoRepository.Get(id);

            if (obj == null)
            {
                return NotFound();
            }

            _unitOfWork.ProjetoRepository.Remove(id);

            return NoContent();
        }

        private bool ObjExists(Guid id)
        {
            return _unitOfWork.ProjetoRepository.Get(id) != null;
        }
    }
}
