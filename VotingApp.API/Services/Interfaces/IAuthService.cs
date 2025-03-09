using Microsoft.AspNetCore.Identity;
using VotingApp.API.DTOs;

namespace VotingApp.API.Services.Interfaces
{
    public interface IAuthService
    {
        Task<IdentityResult> RegisterAsync(RegisterModel model);
        Task<string?> LoginAsync(LoginModel model);
    }
}
