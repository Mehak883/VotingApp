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
            if (await GetSymbolExists(partyModel))
            {
                throw new ConflictException("Symbol already taken by another party");
            }
            var party = new Models.Party
            {
                Id= Guid.NewGuid(),
                Name = partyModel.Name.ToUpper(),
                Symbol = partyModel.Symbol.ToUpper(),
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
            return await dbContext.Parties.AnyAsync(p => p.Name.ToUpper() == partyModel.Name.ToUpper());
        }
        public async Task<bool> GetSymbolExists(PartyModel partyModel)
        {
            return await dbContext.Parties.AnyAsync(p => p.Symbol.ToUpper() == partyModel.Symbol.ToUpper());
        }
        public async Task<bool> GetPartyExistsUpdate(PartyModel partyModel,Guid Id)
        {
            return await dbContext.Parties.AnyAsync(p => p.Name.ToUpper() == partyModel.Name.ToUpper() && p.Id!=Id);
        }
        public async Task<bool> GetSymbolExistsUpdate(PartyModel partyModel,Guid Id)
        {
            return await dbContext.Parties.AnyAsync(p => p.Symbol.ToUpper() == partyModel.Symbol.ToUpper() && p.Id != Id);
        }
        public async Task<bool?> UpdatePartyAsync(Guid Id, PartyModel partyModel) {


            if (await GetPartyExistsUpdate(partyModel,Id))
            {
                throw new ConflictException("Party already exist");
            }
            if (await GetSymbolExistsUpdate(partyModel, Id)){
                throw new ConflictException("Symbol already taken");
            }
            var party = await dbContext.Parties.FindAsync(Id);
            if (party == null) 
                {
                    throw new NotFoundException("Party not found");
                }
            
            party.Name = partyModel.Name.ToUpper();
            party.Symbol = partyModel.Symbol.ToUpper();
            await dbContext.SaveChangesAsync();
            return true;
        }
    }
}
