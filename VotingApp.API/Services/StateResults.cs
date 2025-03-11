using Microsoft.EntityFrameworkCore;
using VotingApp.API.Data;
using VotingApp.API.DTOs;
using VotingApp.API.Services.Interfaces;
namespace VotingApp.API.Services
{
    public class StateResults : IStateResults
    {
        private readonly VotingAppDbContext _context;
        public StateResults(VotingAppDbContext context)
        {
            _context = context;
        }
        public async Task<IEnumerable<stateResultModel>> GetStateResultsAsync()
        {
            var voteResults = await _context.Votes
            .Include(v => v.Candidate.Party)
            .Include(v => v.Voter.State)
            .Select(v => new stateResultModel
            {
                StateId = v.Voter.State.Id,
                StateName = v.Voter.State.Name,
                CandidateId = v.Candidate.Id,
                CandidateName = v.Candidate.FullName,
                PartyName = v.Candidate.Party.Name,
                PartySymbol = v.Candidate.Party.Symbol,
                VoteCount = _context.Votes
                    .Where(vote => vote.CandidateId == v.Candidate.Id && vote.Voter.State.Id == v.Voter.State.Id)
                    .Select(vote => vote.VoterId)
                    .Distinct()
                    .Count()
            })
            .OrderByDescending(r => r.VoteCount)
            .ToListAsync();
            // Use DistinctBy (requires System.Linq)
            var result = voteResults
                .DistinctBy(r => r.StateId) // Keep only the top candidate per state
                .ToList();
            return result;
        }
    }
}