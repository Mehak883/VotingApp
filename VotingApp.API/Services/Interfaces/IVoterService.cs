using Microsoft.AspNetCore.Mvc;
using VotingApp.API.DTOs;

namespace VotingApp.API.Services.Interfaces
{
    public interface IVoterService
    {
        Task<VoterModelDTO> AddVoterAsync(VoterModelDTO voterModel); 
    }
}
