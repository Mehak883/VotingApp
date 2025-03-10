namespace VotingApp.API.DTOs.Candidate
{
    public class CandidateResponse
    {
        public Guid Id { get; set; }
        public string FullName { get; set; }
        public Guid StateId { get; set; }
        public string StateName { get; set; }
        public Guid PartyId { get; set; }
        public string PartyName { get; set; }
        public string Symbol { get; set; }
    }
}
