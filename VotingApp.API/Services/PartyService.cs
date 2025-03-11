using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;
using VotingApp.API.Data;
using VotingApp.API.DTOs.Party;
using VotingApp.API.Exceptions;
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

            if (await GetPartyExists(partyModel))
            {
                throw new ConflictException("Party already exist");
            }

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
        public async Task<List<PartyResponseDto>> GetAllPartiesAsync()
        {
            return await dbContext.Parties
               .Select(p => new PartyResponseDto { Id = p.Id, Name = p.Name,Symbol=p.Symbol })
               .ToListAsync();
        }



        public async Task<PartyResponseDto?> GetPartyData(Guid Id)
        {
           
            var party = await dbContext.Parties.FindAsync(Id);
            if (party == null) throw new NotFoundException("Party not found");

            return new PartyResponseDto
            {
                Id = party.Id,
                Name = party.Name,
                Symbol = party.Symbol
            };
        }

        public async Task<bool> GetPartyExists(PartyModel partyModel)
        {
            return await dbContext.Parties.AnyAsync(p => p.Name.ToLower() == partyModel.Name.ToLower() || p.Symbol == partyModel.Symbol.ToLower());
        }

        public async Task<bool?> UpdatePartyAsync(Guid Id, PartyModel partyModel) {
            if (await GetPartyExists(partyModel))
            {
                throw new ConflictException("Party already exist"); 
            }

            var party = await dbContext.Parties.FindAsync(Id);
            if (party == null) 
                {
                    throw new NotFoundException("Party not found");
                }
            
            party.Name = partyModel.Name;
            party.Symbol = partyModel.Symbol;
            await dbContext.SaveChangesAsync();
            return true;
        }
    }
}
