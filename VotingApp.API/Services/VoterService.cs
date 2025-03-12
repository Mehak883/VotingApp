using Microsoft.EntityFrameworkCore;
using System.Text.RegularExpressions;
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
            if (!Regex.IsMatch(voterModelDTO.Id, @"^[A-Z0-9]{10}$"))
            {
                throw new BadRequestException("Voter ID must be exactly 10 characters long, contain only uppercase alphabets and numbers.");
            }

            bool voterExist = await _context.Voters.AnyAsync(v => v.Id.ToUpper() == voterModelDTO.Id.ToUpper());

            if (voterExist)
            {
                throw new ConflictException("Voter already exists");
            }
            bool stateExist = await _context.States.AnyAsync(s => s.Id == voterModelDTO.StateId);

            if (!stateExist)
            {
                throw new NotFoundException("No State exists");

            }
            var voter =new Voter{Id = voterModelDTO.Id.ToUpper(),StateId =voterModelDTO.StateId}; 
            await _context.Voters.AddAsync(voter);
            await _context.SaveChangesAsync();
            return voterModelDTO;
        }
    }
}
