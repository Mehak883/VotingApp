using VotingApp.API.DTOs;
namespace VotingApp.API.Services.Interfaces
{
    public interface INationalResult
    {
        Task<IEnumerable<NationalResultModel>> GetStateResultsAsync();

    }
}
