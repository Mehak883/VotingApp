using Microsoft.EntityFrameworkCore;
using VotingApp.API.Data;
using VotingApp.API.DTOs.Candidate;
using VotingApp.API.DTOs.Party;
using VotingApp.API.Exceptions;
using VotingApp.API.Models;
using VotingApp.API.Services.Interfaces;

namespace VotingApp.API.Services
{
    public class CandidateService :ICandidateService
    {
        private readonly VotingAppDbContext dbContext;
        private readonly ILoggerService _logger;


        public CandidateService(VotingAppDbContext dbContext, ILoggerService logger)
        {
            this.dbContext = dbContext;
            _logger = logger;
        }

        public async Task<CandidateResponse?> AddCandidateData(CandidateRequest candidateRequest)
        {
            _logger.LogInfo("Adding new candidate.");
            var stateExists = await dbContext.States.AnyAsync(s => s.Id == candidateRequest.StateId);
            var partyExists = await dbContext.Parties.AnyAsync(p => p.Id == candidateRequest.PartyId);

            if (!stateExists || !partyExists)
            {
                _logger.LogWarning("Invalid StateId or PartyId.");
                throw new NotFoundException("Invalid StateId or PartyId.");
            }

            var existingCandidate = await dbContext.Candidates
       .AnyAsync(c => c.StateId == candidateRequest.StateId && c.PartyId == candidateRequest.PartyId);

            if (existingCandidate)
            {
                _logger.LogWarning("Candidate from this party already exists in this state.");
                throw new ConflictException("Candidate from this party already exists in this state.");
            }


            var candidate = new Candidate
            {
                Id = Guid.NewGuid(),
                FullName = candidateRequest.FullName,
                StateId = candidateRequest.StateId,
                PartyId = candidateRequest.PartyId
            };
            _logger.LogInfo($"Candidate {candidate.FullName} added successfully.");

            dbContext.Candidates.Add(candidate);
            await dbContext.SaveChangesAsync();

            var savedCandidate = await dbContext.Candidates
            .Include(c => c.State)
            .Include(c => c.Party)
            .FirstOrDefaultAsync(c => c.Id == candidate.Id);

            if (savedCandidate == null) { 

                throw new ConflictException("A candidate from this party already exists in this state."); }
            _logger.LogInfo($"Candidate {candidate.FullName} added successfully.");
            return new CandidateResponse
            {
                Id = candidate.Id,
                FullName = candidate.FullName,
                StateId = candidate.StateId,
                PartyId = candidate.PartyId,
                StateName = candidate.State.Name,
                PartyName = candidate.Party.Name,
                Symbol = candidate.Party.Symbol
            };

        }

        public async Task<bool> DeleteCandidateData(Guid id)
        {
            _logger.LogInfo($"Deleting candidate with ID: {id}");
            var candidate = await dbContext.Candidates.FindAsync(id);
            if(candidate == null)
            {
                _logger.LogWarning("Candidate not found.");
                throw new NotFoundException("Candidate not found.");
            }
            dbContext.Candidates.Remove(candidate);

            await dbContext.SaveChangesAsync();
            _logger.LogInfo($"Candidate with ID: {id} deleted successfully.");
            return true;
        }

        public async Task<List<CandidateResponse>> GetAllCandidateData()
        {
            _logger.LogInfo("Fetching all candidates.");

            return await dbContext.Candidates
                .Include(c => c.State)
                .Include(c => c.Party)
                .Select(c => new CandidateResponse
                {
                    Id = c.Id,
                    FullName = c.FullName,
                    StateId = c.StateId,
                    StateName = c.State.Name,
                    PartyId = c.PartyId,
                    PartyName = c.Party.Name,
                    Symbol=c.Party.Symbol
                })
                .ToListAsync();
        }
        public async Task<List<CandidateResponse>> GetCandidateDataByStateId(Guid StateId)
        {
            _logger.LogInfo($"Fetching candidate with State ID: {StateId}");
            var state = await dbContext.States.FindAsync(StateId);
            if (state == null)
            {_logger.LogWarning("No such state exists.");
                throw new NotFoundException("No such state exists.");
            }
            
            return await dbContext.Candidates
                .Where(c => c.StateId == StateId)
                .Include(c => c.State)
                .Include(c => c.Party)
                .Select(c => new CandidateResponse
                {
                    Id = c.Id,
                    FullName = c.FullName,
                    StateId = c.StateId,
                    StateName = c.State.Name,
                    PartyId = c.PartyId,
                    PartyName = c.Party.Name,
                    Symbol = c.Party.Symbol
                })
                .ToListAsync();
        }

        public async Task<List<CandidateResponse>> GetCandidateDataByPartyId(Guid PartyId)
        {
            var party = await dbContext.Parties.FindAsync(PartyId);
            if (party == null)
            {_logger.LogWarning("No such party exists.");
                throw new NotFoundException("No such party exists.");
            }

            return await dbContext.Candidates
                .Where(c => c.PartyId == PartyId)
                .Include(c => c.State)
                .Include(c => c.Party)
                .Select(c => new CandidateResponse
                {
                    Id = c.Id,
                    FullName = c.FullName,
                    StateId = c.StateId,
                    StateName = c.State.Name,
                    PartyId = c.PartyId,
                    PartyName = c.Party.Name,
                    Symbol = c.Party.Symbol
                })
                .ToListAsync();
        }
        public async Task<CandidateResponse?> GetCandidateData(Guid Id)
        {
            _logger.LogInfo($"Updating candidate with ID: {Id}");
            var candidate = await dbContext.Candidates.Include(c => c.Party)  
        .Include(c => c.State) 
        .FirstOrDefaultAsync(c => c.Id == Id);
            if (candidate == null) {
                _logger.LogWarning("Candidate not found.");
                throw new NotFoundException("Candidate not found"); }

            return new CandidateResponse
            {
                Id = candidate.Id,
                FullName = candidate.FullName,
                PartyId = candidate.PartyId,
                PartyName = candidate.Party.Name,
                StateId = candidate.StateId,
                StateName = candidate.State.Name,
                Symbol = candidate.Party.Symbol
            };
        }

        public async Task<bool?> UpdateCandidateData(Guid id, CandidateRequest candidateRequest)
        {
            var candidate = await dbContext.Candidates.FirstOrDefaultAsync(c => c.Id == id);

            if (candidate == null)
            {_logger.LogWarning("Candidate not found.");
                throw new NotFoundException("Candidate not found.") ; 
            }

       
            bool isStateChanging = candidateRequest.StateId != Guid.Empty && candidateRequest.StateId != candidate.StateId;
            bool isPartyChanging = candidateRequest.PartyId != Guid.Empty && candidateRequest.PartyId != candidate.PartyId;


            if (isStateChanging)
            {
                var state = await dbContext.States.FindAsync(candidateRequest.StateId);
                if (state == null)
                {
                    _logger.LogWarning("No such state exists.");
                    throw new NotFoundException("No such state exists.");
                }
                candidate.StateId = candidateRequest.StateId;
            }

            if (isPartyChanging)
            {
                var party = await dbContext.Parties.FindAsync(candidateRequest.PartyId);
                if (party == null)
                {_logger.LogWarning("No such party exists.");
                    throw new NotFoundException("No such party exists.");
                }
                candidate.PartyId = candidateRequest.PartyId;
            }


            Guid updatedStateId = isStateChanging ? candidateRequest.StateId : candidate.StateId;
            Guid updatedPartyId = isPartyChanging ? candidateRequest.PartyId : candidate.PartyId;

        
            bool exists = await dbContext.Candidates.AnyAsync(c =>
                c.StateId == updatedStateId &&
                c.PartyId == updatedPartyId &&
                c.Id != id); 

            if (exists)
            {
                _logger.LogWarning("Candidate with the same details already exists.");
                throw new ConflictException("Candidate with the same details already exists."); 
            }

           
            candidate.FullName = candidateRequest.FullName ?? candidate.FullName;


            await dbContext.SaveChangesAsync();
            _logger.LogInfo($"Candidate with ID: {id} updated successfully.");
            return true; 
        }


    }


}

