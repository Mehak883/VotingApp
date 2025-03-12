using Microsoft.AspNetCore.Mvc;
using VotingApp.API.Services.Interfaces;
using VotingApp.API.DTOs;
using VotingApp.API.Services;

namespace VotingApp.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class NationalResultController :ControllerBase
    {
        private readonly INationalResult _nationalResult;
        public NationalResultController(INationalResult nationalResult)
        {
            _nationalResult = nationalResult;
        }
        [HttpGet]
        public async Task<IActionResult> GetStateResultsAsync()
        {
            var nationalResults = await _nationalResult.GetStateResultsAsync();
            if (nationalResults == null)
            {
                var stringResponse = new ApiResponseDTO<String>(false, 200, "OK", (String)"Result is not declared yet");
                return Ok(stringResponse);
            }
            if (nationalResults == null || !nationalResults.Any())
            {
                var emptyResponse = new
                {
                    error = false,
                    code = 200,
                    responseCode = "No results available"
                };

                return Ok(emptyResponse);
            }

            var response = new ApiResponseDTO<IEnumerable<NationalResultModel>>(
                false, 200, "OK", nationalResults
            );
            
            return Ok(response);
        }
    }
}
