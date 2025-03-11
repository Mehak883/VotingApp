using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using VotingApp.API.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using VotingApp.API.DTOs;
using VotingApp.API.Services;


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

            var response = new ApiResponseDTO<IEnumerable<stateResultModel>>(
                false, 200, "OK", stateResults
            );

            return Ok(response);
        }

    }
}
