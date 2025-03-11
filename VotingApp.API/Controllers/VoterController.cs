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
            var response = new ApiResponseDTO<VoterModel>(false, 200, "OK", voter);

            return Ok(response);
            

        }
    }
}
