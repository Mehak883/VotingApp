using System.ComponentModel.DataAnnotations;

namespace VotingApp.API.DTOs.Candidate
{
    public class CandidateRequest
    {
        [Required(ErrorMessage = "Candidate name is required")]
        public string FullName { get; set; }
        public Guid StateId { get; set; }
        public Guid PartyId { get; set; }
    }
}
