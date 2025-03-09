using VotingApp.API.Data;
using VotingApp.API.DTOs;
using VotingApp.API.Services.Interfaces;

namespace VotingApp.API.Services
{
    public class PartyService : IPartyService
    {

        private readonly VotingAppDbContext dbContext;
        public PartyService(VotingAppDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public async Task<PartyResponseDto?> AddPartyData(PartyModel partyModel)
        {
            var party = new Models.Party
            {
                Id= Guid.NewGuid(),
                Name = partyModel.Name,
                Symbol = partyModel.Symbol,
            };


            dbContext.Parties.Add(party);
            await dbContext.SaveChangesAsync();
            return new PartyResponseDto
            {
                Id = party.Id,
                Name = party.Name,
                Symbol = party.Symbol
            };
        }

        public async Task<PartyResponseDto?> GetPartyData(Guid id)
        {
            var party = await dbContext.Parties.FindAsync(id);
            if (party == null) return null;

            return new PartyResponseDto
            {
                Id = party.Id,
                Name = party.Name,
                Symbol = party.Symbol
            };
        }

    }
}
