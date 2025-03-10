using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace VotingApp.API.Models
{
    public class Vote
    {
        public Guid Id { get; set; }

        
        [ForeignKey("Voter")]
        public required String VoterId { get; set; }
       
        [ForeignKey("Candidate")]
        public Guid CandidateId { get; set; }
        [Required]
        public DateTime DateTimeNow { get; set; } = DateTime.UtcNow;

        public Voter Voter { get; set; }
        public Candidate Candidate { get; set; }
}
}
