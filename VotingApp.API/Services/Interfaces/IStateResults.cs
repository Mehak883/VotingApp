using System.Collections.Generic;
using System.Threading.Tasks;
using VotingApp.API.Models;
using VotingApp.API.DTOs;

namespace VotingApp.API.Services.Interfaces
{
    public interface IStateResults
    {
        Task<IEnumerable<StateResultModel>> GetStateResultsAsync();
    }
}
