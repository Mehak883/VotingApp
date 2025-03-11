using Microsoft.AspNetCore.Authorization;

using Microsoft.AspNetCore.Mvc;
using VotingApp.API.DTOs.State;
using VotingApp.API.Exceptions;
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
        

            return Ok(new { data=state });
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> AddState(StateRequest stateRequest)
        {
            var state = await _stateService.AddStateAsync(stateRequest);
            return Ok(new
            {
                message = "State added successfully",
                data = state
            });
        }

        [Authorize]
        [HttpPatch("{Id}")]
        public async Task<IActionResult> UpdateState(Guid Id, StateRequest stateRequest)
        {
            var result = await _stateService.UpdateStateAsync(Id, stateRequest);
            return Ok(new { message = "State updated successfully" });
        }
    }
}
