using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;

namespace VotingApp.API.DTOs
{
    public class VoterModelDTO
    {
        [Required(ErrorMessage =" Voter Id is required")]
        [Key]
        public required String Id { get; set; }

        [Required]
        public Guid StateId { get; set; }
    }
}
