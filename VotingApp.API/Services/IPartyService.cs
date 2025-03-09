using VotingApp.API.DTOs;

namespace VotingApp.API.Services
{
    public interface IPartyService
    {
        Task<bool> AddPartyData(PartyModel partyModel);
    }
}
