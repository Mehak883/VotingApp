using Microsoft.EntityFrameworkCore;
using VotingApp.API.Data;
using VotingApp.API.DTOs.Vote;
using VotingApp.API.Models;
using VotingApp.API.Services.Interfaces;

namespace VotingApp.API.Services
{
    public class VoteService:IVoteService
    {
        private readonly VotingAppDbContext _context;
        private readonly DateTime _sessionStart;
        private readonly DateTime _sessionEnd;

        public VoteService(VotingAppDbContext context,IConfiguration configuration)
        {
            _context = context;
            _sessionStart = DateTime.SpecifyKind(DateTime.Parse(configuration["VotingSession:Start"]), DateTimeKind.Local);
            _sessionEnd = DateTime.SpecifyKind(DateTime.Parse(configuration["VotingSession:End"]), DateTimeKind.Local);
        }
        public async Task<VoteResponseDTO> CastVoteAsync(VoteModel voteModel)
        {
            DateTime currentLocal = DateTime.Now;

            if (currentLocal < _sessionStart || currentLocal > _sessionEnd)
            {
                return new VoteResponseDTO
                {
                    Message = $"Voting is not allowed at this time. Voting session{currentLocal} is active between {_sessionStart} and {_sessionEnd}."
                };
            }


            bool voteCasted = await _context.Votes.AnyAsync(v => v.VoterId == voteModel.VoterId);

            if (voteCasted)
            {
                return new VoteResponseDTO { Message = "Voter has already cast a vote." };
            }

            var voter = await _context.Voters.FindAsync(voteModel.VoterId);
            var candidate = await _context.Candidates.FindAsync(voteModel.CandidateId);

            if (voter == null)
            {

                return new VoteResponseDTO { Message = "Voter Id is not valid" };
            }

            if (candidate == null)
            {
                return new VoteResponseDTO { Message = "Candidate not found." };
            }

            if (voter.StateId != candidate.StateId)
            {
                return new VoteResponseDTO { Message = "Voter's state does not match candidate's state." };
            }

            Vote vote = new Vote { VoterId = voteModel.VoterId, CandidateId = voteModel.CandidateId ,DateTimeNow=currentLocal};
            await _context.Votes.AddAsync(vote);
            await _context.SaveChangesAsync();

            return new VoteResponseDTO
            {
                Id = vote.Id,
                Message= "Vote cast successfully!"
            };
        }
    }
}
