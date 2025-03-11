﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using VotingApp.API.DTOs.Candidate;
using VotingApp.API.DTOs.Party;
using VotingApp.API.Services;
using VotingApp.API.Services.Interfaces;

namespace VotingApp.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CandidateController : ControllerBase
    {
        private readonly ICandidateService candidateService;
        public CandidateController(ICandidateService candidateService)
        {
            this.candidateService = candidateService;
        }

        [HttpGet]
        [Route("allCandidates")]
        public async Task<IActionResult> GetAllCandidates()
        {
            var result = await candidateService.GetAllCandidateData();
            return Ok(new { data = result });
        }


        [Authorize]
        [HttpPost]
        public async Task<IActionResult> AddCandidate(CandidateRequest candidateRequest)
        {
            var result = await candidateService.AddCandidateData(candidateRequest);

            if (result == null)
            {
                return BadRequest(new { message = "A candidate from this party already exists in this state." });
            }

            return Ok(new
            {
                message = "Candidate data added successfully",
                data = result
            });
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetCandidate(Guid id)
        {
            var candidate = await candidateService.GetCandidateData(id);

            if (candidate != null)
                return Ok(new { data = candidate });

            return NotFound("Candidate not found");
        }

        [Authorize]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCandidate(Guid id)
        {
            var isDeleted = await candidateService.DeleteCandidateData(id);

            if (!isDeleted)
            {
                return NotFound(new { message = "Candidate not found." });
            }

            return Ok(new { message = "Candidate removed" });
        }

   
        [HttpPatch("{id}")]
        public async Task<IActionResult> UpdateCandidate(Guid id, CandidateRequest candidateRequest)
        {
            bool? result = await candidateService.UpdateCandidateData(id, candidateRequest);

            if (result == null)
            {
                return BadRequest(new { message = "Candidate with the same details already exists." });
            }

            if ((bool)!result)
            {
                return NotFound(new { message = "Candidate not found." });
            }

            return Ok(new { message = "Candidate updated successfully." });
        }


    }
}
