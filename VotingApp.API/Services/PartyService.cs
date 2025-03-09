using VotingApp.API.Data;
using VotingApp.API.DTOs;

namespace VotingApp.API.Services
{
    public class PartyService : IPartyService
    {

        private readonly VotingAppDbContext dbContext;
        public PartyService(VotingAppDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public async Task<bool> AddPartyData(PartyModel partyModel)
        {
            var party = new Models.Party
            {
                Id= Guid.NewGuid(),
                Name = partyModel.Name,
                Symbol = partyModel.Symbol,
            };


            dbContext.Parties.Add(party);
            await dbContext.SaveChangesAsync();
            return true;
        }

    }
}
