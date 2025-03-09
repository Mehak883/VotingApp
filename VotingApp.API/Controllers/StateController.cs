using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using VotingApp.API.DTOs.State;
using VotingApp.API.Services.Interfaces;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace VotingApp.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StateController : ControllerBase
    {
        private readonly IStateService _stateService;

        public StateController(IStateService stateService)
        {
            _stateService = stateService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllStates()
        {
            List<StateResponse> states = await _stateService.GetAllStatesAsync();
            //if (states.IsNullOrEmpty()) return NotFound();
            return Ok(states);
        }
        [HttpGet("{Id}")]
        public async Task<IActionResult> GetStateById(Guid Id)
        {
            var state = await _stateService.GetStateByIdAsync(Id);
            if (state == null) return NotFound();

            return Ok(state);
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> AddState(StateRequest stateRequest)
        {
            var state = await _stateService.AddStateAsync(stateRequest);
            return CreatedAtAction(nameof(GetStateById), new { id = state.Id }, state);
        }

        [Authorize]
        [HttpPatch]
        public async Task<IActionResult> UpdateState(Guid Id,StateRequest stateRequest)
        {
            bool result = await _stateService.UpdateStateAsync(Id, stateRequest);
            if (!result) return NotFound();
            return NoContent();
        }
    }
}
