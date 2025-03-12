using VotingApp.API.DTOs;
using VotingApp.API.Services.Interfaces;
namespace VotingApp.API.Services
{
    public class NationalResult : INationalResult
    {
        private readonly IStateResults _stateResult;

        public NationalResult(IStateResults stateResult)
        {
            _stateResult = stateResult;
        }

        public async Task<IEnumerable<NationalResultModel>> GetStateResultsAsync()
        {
            var stateResults = await _stateResult.GetStateResultsAsync();

            
            var partyWinCounts = stateResults
                .GroupBy(r => r.PartyName)
                .Select(g => new
                {
                    PartyName = g.Key,
                    PartySymbol = g.First().PartySymbol,
                    StateWinCount = g.Count() 
                })
                .OrderByDescending(p => p.StateWinCount) 
                .ToList();

            if (partyWinCounts.Count() == 0)
            {
                return null; 
            }

            int maxWins = partyWinCounts.First().StateWinCount;

            var topParties = partyWinCounts.Where(p => p.StateWinCount == maxWins).ToList();

            if (topParties.Count() == 1)
            {
                return new List<NationalResultModel>
                    {
                        new NationalResultModel
                        {
                            PartyName = topParties.First().PartyName,
                            PartySymbol = topParties.First().PartySymbol
                        }
                    };
            }
            else
            {
                // Return a tie-breaker result with all tied parties
                //message: draw
                return new List<NationalResultModel>
                    {
                        new NationalResultModel
                        {
                            PartyName = string.Join(" & ", topParties.Select(p => p.PartyName)),
                            PartySymbol = "Tie-Breaker"
                        }
                    };
            }
        }
    }


}
