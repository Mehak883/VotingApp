using Microsoft.EntityFrameworkCore;
using VotingApp.API.Data;
using VotingApp.API.DTOs;
using VotingApp.API.Exceptions;
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

        public async Task<VoterModelDTO> AddVoterAsync(VoterModelDTO voterModelDTO)
        {
            bool voterExist = await _context.Voters.AnyAsync(v=>v.Id== voterModelDTO.Id);
            if (voterExist)
            {
                throw new ConflictException("Voter already exists");
            }
            bool stateExist = await _context.States.AnyAsync(s => s.Id == voterModelDTO.StateId);

            if (!stateExist)
            {
                throw new NotFoundException("No State exists");

            }
            var voter =new Voter{Id = voterModelDTO.Id,StateId =voterModelDTO.StateId}; 
            await _context.Voters.AddAsync(voter);
            await _context.SaveChangesAsync();
            return voterModelDTO;
        }
    }
}
