using VotingApp.API.Data;
using VotingApp.API.DTOs;
using VotingApp.API.Models;
using VotingApp.API.Services.Interfaces;

namespace VotingApp.API.Services
{
    public class VoterService:IVoterService
    {
        private readonly VotingAppDbContext _context;
       public VoterService(VotingAppDbContext context)
        {
            _context = context;
        }

        public async Task<VoterModel> AddVoterAsync(VoterModel voterModel)
        {
            var voter =new Voter{Id = voterModel.Id,StateId =voterModel.StateId}; 
            await _context.Voters.AddAsync(voter);
            await _context.SaveChangesAsync();
            return voterModel;
        }
    }
}
