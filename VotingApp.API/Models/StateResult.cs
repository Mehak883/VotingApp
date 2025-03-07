using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace VotingApp.API.Models
{
    public class StateResult
    {
        [Key] // Marks StateId as the primary key
        [ForeignKey("State")] // Foreign key to State table
        public Guid StateId { get; set; }

        [Required]
        [ForeignKey("Candidate")]
        public Guid CandidateId { get; set; }

        public int VoteCount { get; set; }

        // Navigation property
        public State State { get; set; }
    }
}
