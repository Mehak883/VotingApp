using VotingApp.API.DTOs;
using VotingApp.API.Exceptions;
using VotingApp.API.Services.Interfaces;
namespace VotingApp.API.Services
{
    public class NationalResult : INationalResult
    {
        private readonly IStateResults _stateResult;
        private readonly IVoteSessionService _voteSessionService;

        public NationalResult(IStateResults stateResult, IVoteSessionService voteSessionService)
        {
            _stateResult = stateResult;
            _voteSessionService = voteSessionService;
        }

        public async Task<IEnumerable<NationalResultModel>> GetStateResultsAsync()
        {
            VotingTimingDTO votingTiming = _voteSessionService.LoadVotingTimings();

            DateTime currentLocal = DateTime.Now;
            if (!DateTime.TryParse(votingTiming.StartTime, out DateTime startTime))
                throw new BadRequestException("Invalid start time format.");
            if (!DateTime.TryParse(votingTiming.EndTime, out DateTime endTime))
                throw new BadRequestException("Invalid end time format.");

            if (currentLocal < endTime)
            {
                return null;
            }


            var stateResults = await _stateResult.GetStateResultsAsync();
                if (stateResults == null || !stateResults.Any())
            {
                return new List<NationalResultModel>
        {
            new NationalResultModel { PartyName = "No Results Available", PartySymbol = "-", StateWinCount = 0 }
        };
            }


           
            var partyWinCounts = new Dictionary<string, (string PartySymbol, int StateWinCount)>();

            foreach (var state in stateResults)
            {
                if (!string.IsNullOrEmpty(state.TieMessage))
                {
                 
                    foreach (var candidate in state.TiedCandidates)
                    {
                        if (partyWinCounts.ContainsKey(candidate.PartyName))
                        {
                            partyWinCounts[candidate.PartyName] = (candidate.PartySymbol, partyWinCounts[candidate.PartyName].StateWinCount + 1);
                        }
                        else
                        {
                            partyWinCounts[candidate.PartyName] = (candidate.PartySymbol, 1);
                        }
                    }
                }
                else
                {
                    if (partyWinCounts.ContainsKey(state.PartyName))
                    {
                        partyWinCounts[state.PartyName] = (state.PartySymbol, partyWinCounts[state.PartyName].StateWinCount + 1);
                    }
                    else
                    {
                        partyWinCounts[state.PartyName] = (state.PartySymbol, 1);
                    }
                }
            }

            if (!partyWinCounts.Any())
            {
                return new List<NationalResultModel>
        {
            new NationalResultModel { PartyName = "No Clear Winner", PartySymbol = "-", StateWinCount = 0 }
        };
            }

            int maxWins = partyWinCounts.Max(p => p.Value.StateWinCount);
            var topParties = partyWinCounts.Where(p => p.Value.StateWinCount == maxWins).ToList();

            if (topParties.Count == 1)
            {
                return new List<NationalResultModel>
        {
            new NationalResultModel
            {
                PartyName = topParties.First().Key,
                PartySymbol = topParties.First().Value.PartySymbol,
                StateWinCount = topParties.First().Value.StateWinCount
            }
        };
            }
            else
            {

                return topParties.Select(p => new NationalResultModel
                {
                    PartyName = p.Key,
                    PartySymbol = p.Value.PartySymbol,
                    StateWinCount = p.Value.StateWinCount
                }).ToList();
            }
        }





    }
}
