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
        private readonly IVoteSessionService _voteSessionService;
        public VoteService(VotingAppDbContext context,IConfiguration configuration, IVoteSessionService voteSessionService)
        {
            _voteSessionService = voteSessionService;            
            _context = context;
            //_sessionStart = DateTime.SpecifyKind(DateTime.Parse(configuration["VotingSession:Start"]), DateTimeKind.Local);
            //_sessionEnd = DateTime.SpecifyKind(DateTime.Parse(configuration["VotingSession:End"]), DateTimeKind.Local);
            //this.voteSessionService = voteSessionService;
        }
        public async Task<bool> CastVoteAsync(VoteModel voteModel)
        {
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


            bool voteCasted = await _context.Votes.AnyAsync(v => v.VoterId == voteModel.VoterId);

            if (voteCasted)
            {
                throw new ConflictException("Voter has already cast a vote.");

            }

            var voter = await _context.Voters.FindAsync(voteModel.VoterId);
            var candidate = await _context.Candidates.FindAsync(voteModel.CandidateId);

            if (voter == null)
            {
                throw new NotFoundException("Voter Id is not valid"); 
            }

            if (candidate == null)
            {
                throw new NotFoundException("Candidate not found.");
            }

            if (voter.StateId != candidate.StateId)
            {
                throw new ConflictException ("Voter's state does not match candidate's state." );
            }

            Vote vote = new Vote { VoterId = voteModel.VoterId, CandidateId = voteModel.CandidateId ,DateTimeNow=currentLocal};
            await _context.Votes.AddAsync(vote);
            await _context.SaveChangesAsync();

            return true;
        }
    }
}
