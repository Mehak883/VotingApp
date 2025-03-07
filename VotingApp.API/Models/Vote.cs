using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace VotingApp.API.Models
{
    public class Vote
    {
        public Guid Id { get; set; }

        [Required]
        [ForeignKey("Voter")]

        public String VoterId { get; set; }
        [Required]
        [ForeignKey("Candidate")]
        public Guid CandidateId { get; set; }
        [Required]
        public DateTime DateTimeNow { get; set; } = DateTime.UtcNow;
}
}
