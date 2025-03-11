using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using VotingApp.API.DTOs;
using VotingApp.API.Models;
using VotingApp.API.Services.Interfaces;

namespace VotingApp.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class VoterController : ControllerBase
    {
        private readonly IVoterService voterService;
        public VoterController(IVoterService voterService)
        {
            this.voterService = voterService;
        }

        [HttpPost]
        public async Task<IActionResult> AddVoter(VoterModel voterModel)
        {
            var voter = await voterService.AddVoterAsync(voterModel);
            if (voter == null)
            {
                return BadRequest(new { message = "Voter already exists" });
            }

          
                return Ok(new
                {
                    message = "Voter added successfully",
                    data = voter
                });
            

        }
    }
}
