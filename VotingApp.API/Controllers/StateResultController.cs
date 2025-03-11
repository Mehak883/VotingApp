using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using VotingApp.API.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
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
            return Ok(new { data = stateResults });
        }

    }
}
