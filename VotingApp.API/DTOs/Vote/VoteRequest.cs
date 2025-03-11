using System.ComponentModel.DataAnnotations;

namespace VotingApp.API.DTOs.Vote
{
    public class VoteRequest
    {
        [Required(ErrorMessage = "Voter Id is required")]
        public required String VoterId { get; set; }
        [Required(ErrorMessage ="Candidate Id is required")]
        public Guid CandidateId { get; set; }

    }
}
