using Microsoft.AspNetCore.Identity;

namespace VotingApp.API.Models
{
    public class AuthUser :IdentityUser
    {
        public required string FullName { get; set; }
        
    }

}
