﻿using Cpnucleo.API.Services;
using Cpnucleo.Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using Cpnucleo.Domain.UoW;
using Cpnucleo.Infra.CrossCutting.Security.Interfaces;
using Microsoft.Extensions.Configuration;

namespace Cpnucleo.API.Controllers.V2
{
    [Produces("application/json")]
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiController]
    [ApiVersion("2")]
    public class RecursoController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ICryptographyManager _cryptographyManager;
        private readonly IConfiguration _configuration;

        public RecursoController(IUnitOfWork unitOfWork, 
                                ICryptographyManager cryptographyManager,
                                IConfiguration configuration)
        {
            _unitOfWork = unitOfWork;
            _cryptographyManager = cryptographyManager;
            _configuration = configuration;
        }

        /// <summary>
        /// Listar recursos
        /// </summary>
        /// <remarks>
        /// # Listar recursos
        /// 
        /// Lista recursos da base de dados.
        /// </remarks>
        /// <param name="getDependencies">Listar dependências do objeto</param>        
        /// <response code="200">Retorna uma lista de recursos</response>
        /// <response code="401">Acesso não autorizado</response>
        /// <response code="500">Erro no processamento da requisição</response>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [Authorize]
        public IEnumerable<Recurso> Get(bool getDependencies = false)
        {
            return _unitOfWork.RecursoRepository.All(getDependencies);
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
        [HttpGet("{id}", Name = "GetRecurso")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [Authorize]
        public ActionResult<Recurso> Get(Guid id)
        {
            Recurso recurso = _unitOfWork.RecursoRepository.Get(id);

            recurso.Senha = null;
            recurso.Salt = null;

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
        [ProducesResponseType(typeof(Recurso), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        [Authorize]
        public ActionResult<Recurso> Post([FromBody]Recurso obj)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                _cryptographyManager.CryptPbkdf2(obj.Senha, out string senhaCrypt, out string salt);

                obj.Senha = senhaCrypt;
                obj.Salt = salt;

                obj = _unitOfWork.RecursoRepository.Add(obj);
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

            return CreatedAtRoute("GetRecurso", new { id = obj.Id }, obj);
        }

        /// <summary>
        /// Autenticar
        /// </summary>
        /// <remarks>
        /// # Autenticar
        /// 
        /// Autentica e devolve um token válido por 60 minutos para utilização na API.
        /// 
        /// # Sample request:
        ///
        ///     POST /recurso/autenticar
        ///     {
        ///        "login": "usuario.teste",
        ///        "senha": "12345678"
        ///     }
        /// </remarks>
        /// <param name="obj">Recurso</param>
        /// <response code="200">Retorna um recurso</response>
        /// <response code="400">Objetos não preenchidos corretamente</response>
        /// <response code="404">Recurso não encontrado</response>
        /// <response code="500">Erro no processamento da requisição</response>
        [HttpPost("Auth")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult<Recurso> Auth([FromBody]Recurso obj)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            bool valido = false;

            Recurso recurso = _unitOfWork.RecursoRepository.GetByLogin(obj.Login);

            if (recurso == null)
            {
                return NotFound();
            }

            valido = _cryptographyManager.VerifyPbkdf2(obj.Senha, recurso.Senha, recurso.Salt);

            recurso.Senha = null;
            recurso.Salt = null;

            if (!valido)
            {
                return NotFound();
            }
            else
            {
                int jwtExpires;
                int.TryParse(_configuration["Jwt:Expires"], out jwtExpires);

                recurso.Token = TokenService.GenerateToken(recurso.Id.ToString(), _configuration["Jwt:Key"], _configuration["Jwt:Issuer"], jwtExpires);

                return Ok(recurso);
            }
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
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [Authorize]
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
                _cryptographyManager.CryptPbkdf2(obj.Senha, out string senhaCrypt, out string salt);

                obj.Senha = senhaCrypt;
                obj.Salt = salt;

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
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [Authorize]
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
