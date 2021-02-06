using Cpnucleo.Domain.Entities;
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
    public class RecursoProjetoController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;

        public RecursoProjetoController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        /// <summary>
        /// Listar recursos de projeto
        /// </summary>
        /// <remarks>
        /// # Listar recursos de projeto
        /// 
        /// Lista recursos de projeto da base de dados.
        /// </remarks>
        /// <response code="200">Retorna uma lista de recursos de projeto</response>
        /// <response code="401">Acesso não autorizado</response>
        /// <response code="500">Erro no processamento da requisição</response>
        [HttpGet]
        [ProducesResponseType(200)]
        public IEnumerable<RecursoProjeto> Get()
        {
            return _unitOfWork.RecursoProjetoRepository.All();
        }

        /// <summary>
        /// Consultar recurso de projeto
        /// </summary>
        /// <remarks>
        /// # Consultar recurso de projeto
        /// 
        /// Consulta um recurso de projeto na base de dados.
        /// </remarks>
        /// <param name="id">Id do recurso de projeto</param>        
        /// <response code="200">Retorna um recurso de projeto</response>
        /// <response code="404">Recurso de projeto não encontrado</response>
        /// <response code="401">Acesso não autorizado</response>
        /// <response code="500">Erro no processamento da requisição</response>
        [HttpGet("{id}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(404)]
        public ActionResult<RecursoProjeto> Get(Guid id)
        {
            RecursoProjeto recursoProjeto = _unitOfWork.RecursoProjetoRepository.Get(id);

            if (recursoProjeto == null)
            {
                return NotFound();
            }

            return Ok(recursoProjeto);
        }

        /// <summary>
        /// Incluir recurso de projeto
        /// </summary>
        /// <remarks>
        /// # Incluir recurso de projeto
        /// 
        /// Inclui um recurso de projeto na base de dados.
        /// 
        /// # Sample request:
        ///
        ///     POST /recursoProjeto
        ///     {
        ///        "idRecurso": "fffc0a28-b9e9-4ffd-0053-08d73d64fb91",
        ///        "idProjeto": "fffc0a28-b9e9-4ffd-0053-08d73d64fb91"
        ///     }
        /// </remarks>
        /// <param name="obj">Recurso de projeto</param>        
        /// <response code="201">Recurso de projeto cadastrado com sucesso</response>
        /// <response code="400">Objetos não preenchidos corretamente</response>
        /// <response code="409">Guid informado já consta na base de dados</response>
        /// <response code="401">Acesso não autorizado</response>
        /// <response code="500">Erro no processamento da requisição</response>
        [HttpPost]
        [ProducesResponseType(201)]
        [ProducesResponseType(400)]
        [ProducesResponseType(409)]
        public ActionResult<RecursoProjeto> Post([FromBody]RecursoProjeto obj)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                _unitOfWork.RecursoProjetoRepository.Add(obj);
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
        /// Alterar recurso de projeto
        /// </summary>
        /// <remarks>
        /// # Alterar recurso de projeto
        /// 
        /// Altera um recurso de projeto na base de dados.
        /// 
        /// # Sample request:
        ///
        ///     PUT /recursoProjeto
        ///     {
        ///        "id": "fffc0a28-b9e9-4ffd-0053-08d73d64fb91",
        ///        "idRecurso": "fffc0a28-b9e9-4ffd-0053-08d73d64fb91",
        ///        "idProjeto": "fffc0a28-b9e9-4ffd-0053-08d73d64fb91",
        ///        "dataInclusao": "2019-09-21T19:15:23.519Z"
        ///     }
        /// </remarks>
        /// <param name="id">Id do recurso de projeto</param>        
        /// <param name="obj">Recurso de projeto</param>        
        /// <response code="204">Recurso de projeto alterado com sucesso</response>
        /// <response code="400">ID informado não é válido</response>
        /// <response code="401">Acesso não autorizado</response>
        /// <response code="500">Erro no processamento da requisição</response>
        [HttpPut("{id}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        public IActionResult Put(Guid id, [FromBody]RecursoProjeto obj)
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
                _unitOfWork.RecursoProjetoRepository.Update(obj);
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
        /// Remover recurso de projeto
        /// </summary>
        /// <remarks>
        /// # Remover recurso de projeto
        /// 
        /// Remove um recurso de projeto da base de dados.
        /// </remarks>
        /// <param name="id">Id do recurso de projeto</param>        
        /// <response code="204">Recurso de projeto removido com sucesso</response>
        /// <response code="404">Recurso de projeto não encontrado</response>
        /// <response code="401">Acesso não autorizado</response>
        /// <response code="500">Erro no processamento da requisição</response>
        [HttpDelete("{id}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        public IActionResult Delete(Guid id)
        {
            RecursoProjeto obj = _unitOfWork.RecursoProjetoRepository.Get(id);

            if (obj == null)
            {
                return NotFound();
            }

            _unitOfWork.RecursoProjetoRepository.Remove(id);

            return NoContent();
        }

        private bool ObjExists(Guid id)
        {
            return _unitOfWork.RecursoProjetoRepository.Get(id) != null;
        }
    }
}
