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
        private readonly ILoggerService _logger;

        public VoterService(VotingAppDbContext context,ILoggerService logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<VoterModelDTO> AddVoterAsync(VoterModelDTO voterModelDTO)
        {
            _logger.LogInfo($"Add voter with ID: {voterModelDTO.Id}");
            if (!Regex.IsMatch(voterModelDTO.Id, @"^[A-Z0-9]{10}$"))
            {
                _logger.LogWarning($"Invalid Voter ID format: {voterModelDTO.Id}");

                throw new BadRequestException("Voter ID must be exactly 10 characters long, contain only uppercase alphabets and numbers.");
            }

            bool voterExist = await _context.Voters.AnyAsync(v => v.Id.ToUpper() == voterModelDTO.Id.ToUpper());

            if (voterExist)
            {
                _logger.LogWarning($"Voter already exists with ID: {voterModelDTO.Id}");

                throw new ConflictException("Voter already exists");
            }
            bool stateExist = await _context.States.AnyAsync(s => s.Id == voterModelDTO.StateId);

            if (!stateExist)
            {
                _logger.LogError($"State ID {voterModelDTO.StateId} does not exist.");

                throw new NotFoundException("No State exists");

            }
            var voter =new Voter{Id = voterModelDTO.Id.ToUpper(),StateId =voterModelDTO.StateId};
            _logger.LogInfo($"Adding voter with ID: {voter.Id}, State ID: {voter.StateId}");

            await _context.Voters.AddAsync(voter);
            await _context.SaveChangesAsync();
            _logger.LogInfo($"Successfully added voter with ID: {voter.Id}");

            return voterModelDTO;
        }
    }
}
