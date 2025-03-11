using Microsoft.Identity.Client;

namespace VotingApp.API.DTOs
{
    public class NationalResultModel
    {
        public string PartyName { get; set; }
        public string PartySymbol { get; set; }
    }
}