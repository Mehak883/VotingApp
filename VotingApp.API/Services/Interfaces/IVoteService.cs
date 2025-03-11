using VotingApp.API.DTOs.Vote;

namespace VotingApp.API.Services.Interfaces
{
    public interface IVoteService
    {
        Task<bool> CastVoteAsync(VoteRequest voteModel);
    }
}
