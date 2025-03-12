using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;
using VotingApp.API.Data;
using VotingApp.API.DTOs;
using VotingApp.API.Exceptions;
using VotingApp.API.Services.Interfaces;
namespace VotingApp.API.Services
{
    public class StateResults : IStateResults
    {
        private readonly VotingAppDbContext _context;
        private readonly IVoteSessionService _voteSessionService;
        public StateResults(VotingAppDbContext context,IVoteSessionService voteSessionService)
        {
            _voteSessionService = voteSessionService;
            _context = context;
        }
        public async Task<IEnumerable<stateResultModel>> GetStateResultsAsync()
        {

            VotingTimingDTO votingTiming= _voteSessionService.LoadVotingTimings();

            DateTime currentLocal = DateTime.Now;
            if (!DateTime.TryParse(votingTiming.StartTime, out DateTime startTime))
                throw new BadRequestException("Invalid start time format.");
            if (!DateTime.TryParse(votingTiming.EndTime, out DateTime endTime))
                throw new BadRequestException("Invalid end time format.");

            if (currentLocal < endTime)
            {
                return null;
              }

            var votes = await _context.Votes
            .Include(v => v.Candidate.Party)
            .Include(v => v.Voter.State)
            .Select(v => new
            {
                StateId = v.Voter.State.Id,
                StateName = v.Voter.State.Name,
                CandidateId = v.Candidate.Id,
                CandidateName = v.Candidate.FullName,
                PartyName = v.Candidate.Party.Name,
                PartySymbol = v.Candidate.Party.Symbol,
                VoterId = v.VoterId 
            })
            .ToListAsync();
   
            var voteResults = votes
                .GroupBy(v => new { v.StateId, v.StateName, v.CandidateId, v.CandidateName, v.PartyName, v.PartySymbol })
                .Select(g => new stateResultModel
                {
                    StateId = g.Key.StateId,
                    StateName = g.Key.StateName,
                    CandidateId = g.Key.CandidateId,
                    CandidateName = g.Key.CandidateName,
                    PartyName = g.Key.PartyName,
                    PartySymbol = g.Key.PartySymbol,
                    VoteCount = g.Select(v => v.VoterId).Distinct().Count() 
                })
                .ToList();



            var groupedResults = voteResults
                .GroupBy(r => new { r.StateId, r.StateName})
                .Select(stateGroup =>
                {
                    var maxVotes = stateGroup.Max(r => r.VoteCount);
                    var topCandidates = stateGroup.Where(r => r.VoteCount == maxVotes).ToList();

                    if (topCandidates.Count > 1) 
                    {
                        return new stateResultModel
                        {
                            StateId = stateGroup.Key.StateId,
                            StateName = stateGroup.Key.StateName,
                            TieMessage = $"Tie between {string.Join(" and ", topCandidates.Select(c => c.PartyName))}",
                            TiedCandidates = topCandidates
                        };
                    }
                    else 
                    {
                        return topCandidates.First();
                    }
                })
                .OrderByDescending(r => r.VoteCount) 
                .ToList();

            return groupedResults;
        }
    }
}