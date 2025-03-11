using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using VotingApp.API.DTOs;
using VotingApp.API.Services.Interfaces;

namespace VotingApp.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class VotingSessionController : ControllerBase
    {
        private readonly IVoteSessionService _voteSessionService;

        public VotingSessionController(IVoteSessionService voteSessionService)
        {
            _voteSessionService = voteSessionService;
            
        }
        [HttpGet]
        public IActionResult GetVotingTimings()
        {
            var votingTimings= _voteSessionService.GetVoteTimings();
            var response = new ApiResponseDTO<VotingTimingDTO>(false, 200, "OK", votingTimings);
            return Ok(response);
        }

        [HttpPut]
        [Authorize]
        public IActionResult UpdateVotingTimings(VotingTimingDTO newTimings)
        {
            _voteSessionService.UpdateVoteTiming(newTimings);
            var response = new ApiResponseDTO<bool?>(false, 200, "OK", null, true);
            return Ok(response);
        }
    }
}
