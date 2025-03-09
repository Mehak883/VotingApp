using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using VotingApp.API.DTOs;
using VotingApp.API.Services;

namespace VotingApp.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PartyController : ControllerBase
    {
        private readonly IPartyService partyService;
        public PartyController(IPartyService partyService)
        {
            this.partyService = partyService;
        }

        [HttpPost("add")]
        //[Authorize(Roles ="Admin")]
        public async Task<IActionResult> AddParty([FromBody] PartyModel partyModel)
        {
            var result = await partyService.AddPartyData(partyModel);
            if (result)
                return Ok(new { Message = "Party data added" });

            return BadRequest("Data adding failed");
        }
    }
}
