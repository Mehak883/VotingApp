namespace VotingApp.API.DTOs
{
    public class CandidateResultModel
    {
        public Guid CandidateId { get; set; }
        public string CandidateName { get; set; }
        public string PartyName { get; set; }
        public string PartySymbol { get; set; }
        public int VoteCount { get; set; }
    }
}
