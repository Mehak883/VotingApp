using VotingApp.API.DTOs;

namespace VotingApp.API.Services.Interfaces
{
    public interface IPartyService
    {
        Task<PartyResponseDto?> AddPartyData(PartyModel partyModel);
        Task<PartyResponseDto?> GetPartyData(Guid id);

    }
}
