using Cpnucleo.API.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;

namespace Cpnucleo.API.Controllers.V1
{
    [Produces("application/json")]
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiController]
    [ApiVersion("1", Deprecated = true)]
    public class AuthController : ControllerBase
    {
        private const string userT = "USERTESTE";
        private const string passT = "SENHATESTE";

        private readonly IConfiguration _configuration;

        public AuthController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        /// <summary>
        /// Obter Token
        /// </summary>
        /// <remarks>
        /// # Obter Token
        /// 
        /// Obtém um token válido por 60 minutos para utilização na API.
        /// </remarks>
        /// <param name="user">Usuário padrão: USERTESTE</param>
        /// <param name="pass">Senha padrão: SENHATESTE</param>
        /// <response code="200">Retorna um token válido por 60 minutos</response>
        /// <response code="400">Usuário ou senha inválidos</response>
        /// <response code="500">Erro no processamento da requisição</response>
        [HttpGet("Autenticar")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        public IActionResult Autenticar(string user, string pass)
        {
            string token;

            if (user == userT && pass == passT)
            {
                int jwtExpires;
                int.TryParse(_configuration["Jwt:Expires"], out jwtExpires);

                token = TokenService.GenerateToken(user, _configuration["Jwt:Key"], _configuration["Jwt:Issuer"], jwtExpires);
            }
            else
            {
                return BadRequest();
            }

            return Ok(token);
        }

        /// <summary>
        /// Testar autorização
        /// </summary>
        /// <remarks>
        /// # Testar autorização
        /// 
        /// Testa a autorização efetuada no método de testes para autenticaar.
        /// </remarks>
        /// <response code="200">Ácesso autorizado com sucesso</response>
        /// <response code="401">Acesso não autorizado</response>
        /// <response code="500">Erro no processamento da requisição</response>
        [HttpGet("TestarAutorizacao")]
        [ProducesResponseType(200)]
        [Authorize]
        public IActionResult TestarAutorizacao()
        {
            return Ok();
        }
    }
}