using VotingApp.API.DTOs.Party;

namespace VotingApp.API.Services.Interfaces
{
    public interface IPartyService
    {
        Task<List<PartyResponse>> GetAllPartiesAsync();
        Task<PartyResponse?> AddPartyData(PartyRequest partyModel);
        Task<PartyResponse?> GetPartyData(Guid id);
        Task<bool?> UpdatePartyAsync(Guid Id, PartyRequest partyModel);

    }
}
