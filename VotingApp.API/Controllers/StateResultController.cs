using Microsoft.AspNetCore.Mvc;
using VotingApp.API.Services.Interfaces;
using VotingApp.API.DTOs;


namespace VotingApp.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StateResultController : Controller
    {
        private readonly IStateResults stateResultService;
        public StateResultController(IStateResults stateResultService)
        {
            this.stateResultService = stateResultService;
        }
        [HttpGet]
        public async Task<IActionResult> GetAllStateResults()
        {
            var stateResults = await stateResultService.GetStateResultsAsync();

if(stateResults == null)
            {
                var stringResponse = new ApiResponseDTO<String>(false, 200, "OK", (String)"Result is not declared yet");
                return Ok(stringResponse);
            }


          
            var response = new ApiResponseDTO<IEnumerable<StateResultModel>>(
                false, 200, "OK", stateResults
            );

            return Ok(response);
        }

    }
}
