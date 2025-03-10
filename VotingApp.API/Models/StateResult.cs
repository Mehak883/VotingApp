using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace VotingApp.API.Models
{
    public class StateResult
    {
       
        public Guid Id { get; set; }

        [ForeignKey("State")] 
        public Guid StateId { get; set; }

        [ForeignKey("Candidate")]
        public Guid CandidateId { get; set; }

        public int VoteCount { get; set; }

        public State State { get; set; }
        public Candidate Candidate { get; set; }
    }
}
