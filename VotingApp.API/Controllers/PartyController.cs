using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using VotingApp.API.DTOs;
using VotingApp.API.DTOs.State;
using VotingApp.API.Models;
using VotingApp.API.Services;
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

        [HttpGet]
        [Route("allParties")]
public async Task<IActionResult> GetAllParties()
        {
            var result = await partyService.GetAllPartiesAsync();
            return Ok(new { data = result});
        }
        [HttpPost]
        public async Task<IActionResult> AddParty(PartyModel partyModel)
        {
            var result = await partyService.AddPartyData(partyModel);
            if (result != null)
            {
                return Ok(new
                {
                    message = "Party data added successfully",
                    data = result
                });
            }

            return BadRequest(new { message = "Party already exists" });
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetParty(Guid id)
        {
            var party = await partyService.GetPartyData(id);

            if (party != null)
                return Ok(new{data=party});

            return NotFound("Party not found");
        }
        [Authorize]
        [HttpPatch]
        public async Task<IActionResult> UpdateState(Guid Id, PartyModel partyModel)
        {
            bool? result = await partyService.UpdatePartyAsync(Id,partyModel );
            if (result == null)
            {
                return BadRequest(new { message = "Party already exists" });

            }
            if ((bool)!result) return NotFound("Party not found");
            return Ok(new
            {
                message = "Party updated successfully"
            });
        }
    }
}
