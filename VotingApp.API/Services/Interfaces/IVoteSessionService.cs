using VotingApp.API.DTOs;

namespace VotingApp.API.Services.Interfaces
{
    public interface IVoteSessionService
    {
        VotingTimingDTO GetVoteTimings();
        bool UpdateVoteTiming(VotingTimingDTO votingTimingDTO);

        VotingTimingDTO LoadVotingTimings();
        void SaveVotingTimings(VotingTimingDTO timings);
    }
}
