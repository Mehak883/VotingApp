﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using VotingApp.API.DTOs;
using VotingApp.API.DTOs.State;
using VotingApp.API.DTOs.Vote;
using VotingApp.API.Models;
using VotingApp.API.Services.Interfaces;

namespace VotingApp.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class VoteController : ControllerBase
    {
        private readonly IVoteService _voteService;
        public VoteController(IVoteService voteService)
        {
            _voteService = voteService;
        }

        [HttpPost("cast")]
        public async Task<IActionResult> CastVote(VoteModel voteModel)
        {

            var result = await _voteService.CastVoteAsync(voteModel);

           
            var response = new ApiResponseDTO<bool?>(false, 200, "OK",null,result);

            return Ok(response);
            }
        }
    }



