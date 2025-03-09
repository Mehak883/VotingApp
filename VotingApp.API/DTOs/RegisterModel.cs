using System.ComponentModel.DataAnnotations;

namespace VotingApp.API.DTOs
{
   
        public class RegisterModel
        {
        [Required(ErrorMessage = "FullName is required.")]
        public required string  FullName { get; set; }

        public required string Email { get; set; }
        public required string Password { get; set; }
        }
   
}
