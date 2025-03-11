using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using VotingApp.API.DTOs;
using VotingApp.API.DTOs.Party;

using VotingApp.API.Services.Interfaces;

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

        [HttpGet]
        [Route("allParties")]
    public async Task<IActionResult> GetAllParties()
        {
            var result = await partyService.GetAllPartiesAsync();
            var response = new ApiResponseDTO<List<PartyResponseDto>>(false, 200, "OK", result);

            return Ok(response);
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> AddParty(PartyModel partyModel)
        {
            var result = await partyService.AddPartyData(partyModel);
            var response = new ApiResponseDTO<PartyResponseDto>(false, 200, "OK", result);
            return Ok(response);


        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetParty(Guid id)
        {
            var party = await partyService.GetPartyData(id);

            var response = new ApiResponseDTO<PartyResponseDto>(false, 200, "OK", party);
            return Ok(response);

        }
        [Authorize]
        [HttpPatch]
        public async Task<IActionResult> UpdateParty(Guid Id, PartyModel partyModel)
        {
            bool? result = await partyService.UpdatePartyAsync(Id,partyModel );
            var response = new ApiResponseDTO<bool?>(false, 200, "OK",null, (bool)result);
            return Ok(response);
        }
    }
}
