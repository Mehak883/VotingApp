using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace VotingApp.API.Models
{
    public class Voter
    {
        [Required]
        [Key]
        public required String  Id { get; set; }

        [Required]
        [ForeignKey("State")]
        public Guid StateId { get; set; }

        public State State { get; set; }

    }
}
