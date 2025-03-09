using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using VotingApp.API.DTOs;
using VotingApp.API.Models;
using VotingApp.API.Services.Interfaces;

namespace VotingApp.API.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class PartyController : ControllerBase
    {
        private readonly IPartyService partyService;
        public PartyController(IPartyService partyService)
        {
            this.partyService = partyService;
        }


        
        [HttpPost]
        public async Task<IActionResult> AddParty(PartyModel partyModel)
        {
            var result = await partyService.AddPartyData(partyModel);
            if (result != null)
            {
                return Ok(new
                {
                    Message = "Party data added successfully",
                    Party = result
                });
            }

            return BadRequest("Data adding failed");
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetParty(Guid id)
        {
            var party = await partyService.GetPartyData(id);

            if (party != null)
                return Ok(party);

            return NotFound("Party not found");
        }

    }
}
