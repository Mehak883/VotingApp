using Microsoft.Identity.Client;

namespace VotingApp.API.DTOs
{
    public class NationalResultModel
    {
        public Guid PartyId{ get; set; }
        public string? PartyName { get; set; }
        public string PartySymbol { get; set; }
        public int StateWinCount { get; set; }
    }
}