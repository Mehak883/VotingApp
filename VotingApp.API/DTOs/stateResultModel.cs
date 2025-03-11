namespace VotingApp.API.DTOs
{
    public class stateResultModel
    {
        public Guid StateId { get; set; }
        public string StateName { get; set; }
        public Guid CandidateId { get; set; }
        public string CandidateName { get; set; }
        public int VoteCount { get; set; }
    }
}
