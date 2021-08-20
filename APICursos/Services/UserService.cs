using APICursos.Models;
using Microsoft.Extensions.Logging;

namespace APICursos.Services
{
    public interface IUserService
    {
        bool IsValidUser(string userName, string password);
    }

    public class UserService : IUserService
    {
        private readonly ILogger<UserService> _logger;
        private readonly AppDbContext _context;

        // inject database for user validation
        public UserService(ILogger<UserService> logger, AppDbContext context)
        {
            _logger = logger;
            _context = context;
        }

        public bool IsValidUser(string userName, string password)
        {
            _logger.LogInformation($"Validating user [{userName}]");
            if (string.IsNullOrWhiteSpace(userName))
            {
                return false;
            }

            if (string.IsNullOrWhiteSpace(password))
            {
                return false;
            }

            var usuario = _context.Usuarios.FindAsync(userName);

            if (usuario == null)
            {
                return false;
            }
            





            return true;
        }
    }
}