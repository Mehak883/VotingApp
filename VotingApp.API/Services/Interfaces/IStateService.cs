using VotingApp.API.DTOs.State;

namespace VotingApp.API.Services.Interfaces
{
    public interface IStateService
    {
        Task<List<StateResponse>> GetAllStatesAsync();
        Task<StateResponse> GetStateByIdAsync(Guid id);
        Task<StateResponse> AddStateAsync(StateRequest stateRequest);
        Task<bool> UpdateStateAsync(Guid Id, StateRequest stateRequest);
        //Task<bool> DeleteStateAsync(Guid Id);
    }
}
