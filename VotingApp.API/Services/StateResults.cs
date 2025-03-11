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
            var result = await _context.Votes
                 .GroupBy(v => new { StateName =v.Voter.State.Name,  CandidateName = v.Candidate.FullName })
                 .Select(g => new stateResultModel
                 {
                     StateName = g.Key.StateName,
                     CandidateName = g.Key.CandidateName,
                     VoteCount = g.Select(v => v.VoterId).Distinct().Count()
                 }).OrderByDescending(g => g.VoteCount).ToListAsync();
            return result;
        }
                 
        
    }
}

