using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using VotingApp.API.DTOs;
using VotingApp.API.Models;
using VotingApp.API.Services.Interfaces;

namespace VotingApp.API.Services
{
    public class AuthService:IAuthService
    {
        private readonly UserManager<AuthUser> _userManager;
        private readonly IConfiguration _config;
        private ILoggerService _logger;
        public AuthService(UserManager<AuthUser> userManager, IConfiguration config,ILoggerService logger)
        {
            _userManager = userManager;
            _logger = logger;
            _config = config;
        }

        public async Task<IdentityResult> RegisterAsync(RegisterModel model)
        {
            var user = new AuthUser { UserName = model.Email, Email = model.Email, FullName = model.FullName };
            return await _userManager.CreateAsync(user, model.Password);
        }

        public async Task<string?> LoginAsync(LoginModel model)
        {
            var user = await _userManager.FindByEmailAsync(model.Email);
            if (user == null || !(await _userManager.CheckPasswordAsync(user, model.Password)))
            {
                _logger.LogWarning("No user found");
                return null;
            }
            _logger.LogDebug("Token genreated");
            return GenerateJwtToken(user);
        }


        private string GenerateJwtToken(AuthUser user)
        {
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var claims = new[]
            {
            new Claim(JwtRegisteredClaimNames.Sub, user.Id),
            new Claim(JwtRegisteredClaimNames.Email, user.Email)
        };
            var token = new JwtSecurityToken(_config["Jwt:Issuer"], _config["Jwt:Audience"], claims, expires: DateTime.UtcNow.AddHours(2), signingCredentials: creds);
            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
