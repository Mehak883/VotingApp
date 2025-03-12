using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using VotingApp.API.DTOs;
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
        [Route("allStates")]
        [HttpGet]
        public async Task<IActionResult> GetAllStates()
        {
            List<StateResponse> states = await _stateService.GetAllStatesAsync();
            var response = new ApiResponseDTO<List<StateResponse>>(false, 200, "OK", states);
            return Ok(response);
        }
        [HttpGet("{Id}")]
        public async Task<IActionResult> GetStateById(Guid Id)
        {
            var state = await _stateService.GetStateByIdAsync(Id);
        
            var response = new ApiResponseDTO<StateResponse>(false, 200, "OK", state);
            return Ok(response);
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> AddState(StateRequest stateRequest)
        {
            var state = await _stateService.AddStateAsync(stateRequest);
            var response = new ApiResponseDTO<StateResponse>(false, 200, "OK", state);
            return Ok(response);
         
        }

        [Authorize]
        [HttpPatch("{Id}")]
        public async Task<IActionResult> UpdateState(Guid Id, StateRequest stateRequest)
        {
            var result = await _stateService.UpdateStateAsync(Id, stateRequest);
            var response = new ApiResponseDTO<bool?>(false, 200, "OK", null,result);
            return Ok(response);
        }
    }
}
