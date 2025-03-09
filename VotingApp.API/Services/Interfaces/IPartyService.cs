using VotingApp.API.DTOs;

namespace VotingApp.API.Services.Interfaces
{
    public interface IPartyService
    {
        Task<List<PartyResponseDto>> GetAllPartiesAsync();
        Task<PartyResponseDto?> AddPartyData(PartyModel partyModel);
        Task<PartyResponseDto?> GetPartyData(Guid id);
        Task<bool?> UpdatePartyAsync(Guid Id, PartyModel partyModel);


    }
}
