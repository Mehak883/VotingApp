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

        public async Task<PartyResponse?> AddPartyData(PartyRequest partyRequest)
        {

            if (await GetPartyExists(partyRequest))
            {
                throw new ConflictException("Party already exist");
            }
            if (await GetSymbolExists(partyRequest))
            {
                throw new ConflictException("Symbol already taken by another party");
            }
            var party = new Models.Party
            {
                Id= Guid.NewGuid(),
                Name = partyRequest.Name.ToUpper(),
                Symbol = partyRequest.Symbol.ToUpper(),
            };


            dbContext.Parties.Add(party);
            await dbContext.SaveChangesAsync();
            return new PartyResponse
            {
                Id = party.Id,
                Name = party.Name,
                Symbol = party.Symbol
            };
        }
        public async Task<List<PartyResponse>> GetAllPartiesAsync()
        {
            return await dbContext.Parties
               .Select(p => new PartyResponse { Id = p.Id, Name = p.Name,Symbol=p.Symbol })
               .ToListAsync();
        }



        public async Task<PartyResponse?> GetPartyData(Guid Id)
        {
           
            var party = await dbContext.Parties.FindAsync(Id);
            if (party == null) throw new NotFoundException("Party not found");

            return new PartyResponse
            {
                Id = party.Id,
                Name = party.Name,
                Symbol = party.Symbol
            };
        }

        public async Task<bool> GetPartyExists(PartyRequest partyRequest)
        {
            return await dbContext.Parties.AnyAsync(p => p.Name.ToUpper() == partyRequest.Name.ToUpper());
        }
        public async Task<bool> GetSymbolExists(PartyRequest partyRequest)
        {
            return await dbContext.Parties.AnyAsync(p => p.Symbol.ToUpper() == partyRequest.Symbol.ToUpper());
        }
        public async Task<bool> GetPartyExistsUpdate(PartyRequest partyRequest, Guid Id)
        {
            return await dbContext.Parties.AnyAsync(p => p.Name.ToUpper() == partyRequest.Name.ToUpper() && p.Id!=Id);
        }
        public async Task<bool> GetSymbolExistsUpdate(PartyRequest partyRequest,Guid Id)
        {
            return await dbContext.Parties.AnyAsync(p => p.Symbol.ToUpper() == partyRequest.Symbol.ToUpper() && p.Id != Id);
        }
        public async Task<bool?> UpdatePartyAsync(Guid Id, PartyRequest partyRequest) {


            if (await GetPartyExistsUpdate(partyRequest,Id))
            {
                throw new ConflictException("Party already exist");
            }
            if (await GetSymbolExistsUpdate(partyRequest, Id)){
                throw new ConflictException("Symbol already taken");
            }
            var party = await dbContext.Parties.FindAsync(Id);
            if (party == null) 
                {
                    throw new NotFoundException("Party not found");
                }
            
            party.Name = partyRequest.Name.ToUpper();
            party.Symbol = partyRequest.Symbol.ToUpper();
            await dbContext.SaveChangesAsync();
            return true;
        }
    }
}
