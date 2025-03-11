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
        public async Task<IActionResult> AddVoter(VoterModelDTO voterModelDTO)
        {
            var voter = await voterService.AddVoterAsync(voterModelDTO);         
            var response = new ApiResponseDTO<VoterModelDTO>(false, 200, "OK", voter);

            return Ok(response);
            

        }
    }
}
