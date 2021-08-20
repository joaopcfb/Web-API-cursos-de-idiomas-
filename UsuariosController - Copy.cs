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
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

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




        public UsuariosController(AppDbContext context, ILogger<UsuariosController> logger, IUserService userService, GerenciadorDeToken gerenciadorDeToken)
        {
            _context = context;
            _logger = logger;
            _userService = userService;
            _gerenciadorDeToken = gerenciadorDeToken;
        }


        [AllowAnonymous]
        [HttpPost("login")]
        public async Task<ActionResult> LoginAsync([FromBody] Usuario usuario)
        {

            //if (!ModelState.IsValid)
            //{
            //    return BadRequest("Invalid Request");
            //}

            //if (!_userService.IsValidUser(usuario.NomeUsuario, usuario.Senha))
            //{
            //    return BadRequest("Invalid Request");
            //}

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

        //[AllowAnonymous]
        //[HttpGet("login")]
        //public async Task<ActionResult> Login2Async([FromBody] Usuario request, string NomeUsuario, string Senha)
        //{
        //    //if (!ModelState.IsValid)
        //    //{
        //    //    return BadRequest("Invalid Request");
        //    //}

        //    //if (!_userService.IsValidUser(request.NomeUsuario, request.Senha))
        //    //{
        //    //    return BadRequest("Invalid Request");
        //    //}
        //    var usuario = await _context.Usuarios.FindAsync(NomeUsuario);

        //    if (usuario == null)
        //    {
        //        return BadRequest("Usuario ou senha errados");
        //    }

        //    if (usuario.Senha != Senha)
        //    {
        //        return BadRequest("Usuario ou senha errados");
        //    }

        //    var claims = new[]
        //    {
        //        new Claim(ClaimTypes.Name,request.NomeUsuario)
        //    };

        //    var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_gerenciadorDeToken.Secret));
        //    var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
        //    var jwtToken = new JwtSecurityToken(
        //        _gerenciadorDeToken.Issuer,
        //        _gerenciadorDeToken.Audience,
        //        claims,
        //        expires: DateTime.Now.AddMinutes(_gerenciadorDeToken.AccessExpiration),
        //        signingCredentials: credentials);
        //    var token = new JwtSecurityTokenHandler().WriteToken(jwtToken);
        //    _logger.LogInformation($"User [{request.NomeUsuario}] logged in the system.");

        //    return Ok(new LoginResult
        //    {
        //        UserName = request.NomeUsuario,
        //        JwtToken = token
        //    });
        //}



        public class LoginRequest
        {
            /// <summary>
            /// 
            /// </summary>
            /// <example>admin</example>
            [Required]
            [JsonPropertyName("username")]
            public string UserName { get; set; }

            /// <summary>
            /// 
            /// </summary>
            /// <example>securePassword</example>
            [Required]
            [JsonPropertyName("password")]
            public string Password { get; set; }
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


        //


        // GET: api/Usuarios
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Usuario>>> GetUsuarios()
        {
            return await _context.Usuarios.ToListAsync();
        }

        // GET: api/Usuarios/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Usuario>> GetUsuario(string id)
        {
            var usuario = await _context.Usuarios.FindAsync(id);

            if (usuario == null)
            {
                return NotFound();
            }

            return usuario;
        }

        // PUT: api/Usuarios/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutUsuario(string id, Usuario usuario)
        {

            if (id != usuario.NomeUsuario)
            {
                return BadRequest();
            }

            _context.Entry(usuario).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!UsuarioExists(id))
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

        // POST: api/Usuarios
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Usuario>> PostUsuario(Usuario usuario)
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

            return CreatedAtAction("GetUsuario", new { id = usuario.NomeUsuario }, usuario);
        }

        // DELETE: api/Usuarios/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUsuario(string id)
        {
            var usuario = await _context.Usuarios.FindAsync(id);
            if (usuario == null)
            {
                return NotFound();
            }

            _context.Usuarios.Remove(usuario);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool UsuarioExists(string id)
        {
            return _context.Usuarios.Any(e => e.NomeUsuario == id);
        }
    }
}
