using Microsoft.AspNetCore.Authorization;

using Microsoft.AspNetCore.Mvc;
using VotingApp.API.DTOs.State;
using VotingApp.API.Services.Interfaces;

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
            return Ok(new { data = states });
        }
        [HttpGet("{Id}")]
        public async Task<IActionResult> GetStateById(Guid Id)
        {
            var state = await _stateService.GetStateByIdAsync(Id);
            if (state == null) return NotFound("State not found");

            return Ok(new { data=state });
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> AddState(StateRequest stateRequest)
        {
            var state = await _stateService.AddStateAsync(stateRequest);
            if (state == null)
            {
                return BadRequest(new { message = "State already exists" });
            }
            return Ok(new
            {
                message = "State added successfully",
                data = state
            });
        }

        [Authorize]
        [HttpPatch]
        public async Task<IActionResult> UpdateState(Guid Id,StateRequest stateRequest)
        {
            bool? result = await _stateService.UpdateStateAsync(Id, stateRequest);
            if(result==null)
            {
                return BadRequest(new { message = "State already exists" });

            }
            if ((bool)!result) return NotFound("State not found");
            return Ok(new {message = "State updated successfully"
        });
        }
    }
}
