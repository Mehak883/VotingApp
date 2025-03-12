using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using VotingApp.API.DTOs;
using VotingApp.API.DTOs.Candidate;
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
            var response = new ApiResponseDTO<List<CandidateResponse>>(false, 200, "OK", result);
            return Ok(response);
        }


        [Authorize]
        [HttpPost]
        public async Task<IActionResult> AddCandidate(CandidateRequest candidateRequest)
        {
            var result = await candidateService.AddCandidateData(candidateRequest);

            var response = new ApiResponseDTO<CandidateResponse>(false, 200, "OK", result);
            return Ok(response);
           
        }

        [HttpGet("{Id}")]
        public async Task<IActionResult> GetCandidate(Guid Id)
        {
            var candidate = await candidateService.GetCandidateData(Id);
            var response = new ApiResponseDTO<CandidateResponse>(false, 200, "OK", candidate);
            return Ok(response);
            

        }

        [HttpGet("by-state/{StateId}")]

        public async Task<IActionResult> GetCandidateByStateId(Guid StateId)
        {
            var candidates = await candidateService.GetCandidateDataByStateId(StateId);
            var response = new ApiResponseDTO<List<CandidateResponse>>(false, 200, "OK", candidates);
            return Ok(response);
        }

        //[HttpGet("by-party/{PartyId}")]

        //public async Task<IActionResult> GetCandidateByPartyId(Guid PartyId)
        //{
        //    var candidates = await candidateService.GetCandidateDataByPartyId(PartyId);
        //    var response = new ApiResponseDTO<List<CandidateResponse>>(false, 200, "OK", candidates);
        //    return Ok(response);
        //}

        [Authorize]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCandidate(Guid id)
        {
            var isDeleted = await candidateService.DeleteCandidateData(id);
            var response = new ApiResponseDTO<bool?>(false, 200, "OK", null,isDeleted);
            return Ok(response);
        }

        [Authorize]
        [HttpPatch("{id}")]
        public async Task<IActionResult> UpdateCandidate(Guid id, CandidateRequest candidateRequest)
        {
            bool? result = await candidateService.UpdateCandidateData(id, candidateRequest);
            var response = new ApiResponseDTO<bool?>(false, 200, "OK", null, (bool)result);
            return Ok(response);
        }


    }
}
