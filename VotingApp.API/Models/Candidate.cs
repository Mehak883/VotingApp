using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace VotingApp.API.Models
{
    public class Candidate
    {
        public Guid Id { get; set; }

        [Required]
        public required string FullName { get; set;}
        [Required]
        [ForeignKey("State")]
        public Guid StateId { get; set; }
        [Required]
        [ForeignKey("Party")]

        public Guid PartyId { get; set; }
        
        public State State { get; set; }

        public Party Party { get; set; }
    }
}
