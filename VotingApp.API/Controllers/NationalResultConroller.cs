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
    public class NationalResultConroller
    {
        private readonly INationalResult _nationalResult;
        public NationalResultConroller(INationalResult nationalResult)
        {
            _nationalResult = nationalResult;
        }
        [HttpGet]
        public async Task<IEnumerable<NationalResultModel>> GetStateResultsAsync()
        {
            return await _nationalResult.GetStateResultsAsync();
        }
    }
}
