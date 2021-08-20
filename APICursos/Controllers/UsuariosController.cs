using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using APICursos.Models;
using Microsoft.Extensions.Logging;
using APICursos.Services;
using APICursos.Infrastructure.Jwt;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using System.IdentityModel.Tokens.Jwt;

namespace APICursos.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsuariosController : ControllerBase
    {
        private readonly AppDbContext _context;

        private readonly ILogger<UsuariosController> _logger;
        private readonly IUserService _userService;
        private readonly GerenciadorDeToken _gerenciadorDeToken;

        public UsuariosController(AppDbContext context, ILogger<UsuariosController> logger, GerenciadorDeToken gerenciadorDeToken)
        {
            _context = context;
            _logger = logger;
            _gerenciadorDeToken = gerenciadorDeToken;
        }


        [AllowAnonymous]
        [HttpPost("login")]
        public async Task<ActionResult> LoginAsync([FromBody] Usuario usuario)
        {

            _context.Usuarios.Add(usuario);
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                if (UsuarioExists(usuario.NomeUsuario))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }


            var claims = new[]
            {
                new Claim(ClaimTypes.Name,usuario.NomeUsuario)
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_gerenciadorDeToken.Secret));
            var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var jwtToken = new JwtSecurityToken(
                _gerenciadorDeToken.Issuer,
                _gerenciadorDeToken.Audience,
                claims,
                expires: DateTime.Now.AddMinutes(_gerenciadorDeToken.AccessExpiration),
                signingCredentials: credentials);
            var token = new JwtSecurityTokenHandler().WriteToken(jwtToken);
            _logger.LogInformation($"User [{usuario.NomeUsuario}] logged in the system.");
            return Ok(new LoginResult
            {
                UserName = usuario.NomeUsuario,
                JwtToken = token
            });
        }

        public class LoginResult
        {
            /// <summary>
            /// 
            /// </summary>
            /// <example>admin</example>
            public string UserName { get; set; }
            public string JwtToken { get; set; }
        }

        

        private bool UsuarioExists(string id)
        {
            return _context.Usuarios.Any(e => e.NomeUsuario == id);
        }
    }
}
