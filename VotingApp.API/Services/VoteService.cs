using Microsoft.EntityFrameworkCore;
using VotingApp.API.Data;
using VotingApp.API.DTOs;
using VotingApp.API.DTOs.Vote;
using VotingApp.API.Exceptions;
using VotingApp.API.Models;
using VotingApp.API.Services.Interfaces;

namespace VotingApp.API.Services
{
    public class VoteService:IVoteService
    {
        private readonly VotingAppDbContext _context;
        private readonly ILoggerService _logger;
        private readonly IVoteSessionService _voteSessionService;
        public VoteService(VotingAppDbContext context,IConfiguration configuration, IVoteSessionService voteSessionService,ILoggerService logger)
        {
            _logger = logger;
            _voteSessionService = voteSessionService;            
            _context = context;
           
        }
        public async Task<bool> CastVoteAsync(VoteRequest voteRequest)
        {
            _logger.LogInfo($"Voter casting vote {voteRequest.VoterId}");
            VotingTimingDTO votingTiming= _voteSessionService.LoadVotingTimings();
            DateTime currentLocal = DateTime.Now;
            if (!DateTime.TryParse(votingTiming.StartTime, out DateTime startTime))
                throw new BadRequestException("Invalid start time format.");
            if (!DateTime.TryParse(votingTiming.EndTime, out DateTime endTime))
                throw new BadRequestException("Invalid end time format.");
            if (currentLocal < startTime || currentLocal > endTime)
            {
                throw new BadRequestException($"Voting is not allowed at this time. Voting session is active between {startTime} and {endTime}.");
            }


            bool voteCasted = await _context.Votes.AnyAsync(v => v.VoterId == voteRequest.VoterId);

            if (voteCasted)
            {
                _logger.LogWarning($"Duplicate vote attempt by Voter ID: {voteRequest.VoterId}");

                throw new ConflictException("Voter has already cast a vote.");

            }

            var voter = await _context.Voters.FindAsync(voteRequest.VoterId);
            var candidate = await _context.Candidates.FindAsync(voteRequest.CandidateId);

            if (voter == null)
            {
                _logger.LogError($"Voter ID {voteRequest.VoterId} not found.");

                throw new NotFoundException("Voter Id is not valid"); 
            }

            if (candidate == null)
            {
                _logger.LogError($"Candidate ID {voteRequest.CandidateId} not found.");

                throw new NotFoundException("Candidate not found.");
            }

            if (voter.StateId != candidate.StateId)
            {
                _logger.LogWarning($"Voter ID {voteRequest.VoterId} attempted to vote for a candidate from a different state.");

                throw new ConflictException ("Voter's state does not match candidate's state." );
            }

            Vote vote = new Vote { VoterId = voteRequest.VoterId, CandidateId = voteRequest.CandidateId ,DateTimeNow=currentLocal};
            await _context.Votes.AddAsync(vote);
            await _context.SaveChangesAsync();
            _logger.LogInfo($"Vote successfully cast by Voter ID: {voteRequest.VoterId} for Candidate ID: {voteRequest.CandidateId}");

            return true;
        }
    }
}
