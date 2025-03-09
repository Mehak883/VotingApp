using Microsoft.EntityFrameworkCore;
using VotingApp.API.Data;
using VotingApp.API.DTOs.State;
using VotingApp.API.Models;
using VotingApp.API.Services.Interfaces;

namespace VotingApp.API.Services
{
    public class StateService : IStateService
    {
        private readonly VotingAppDbContext _context;

        public StateService(VotingAppDbContext context)
        {
            _context = context;
        }
        public async Task<List<StateResponse>> GetAllStatesAsync()
        {
            return await _context.States
               .Select(s => new StateResponse { Id = s.Id, Name = s.Name })
               .ToListAsync() ;
        }

        public async Task<StateResponse> GetStateByIdAsync(Guid Id)
        {
            State ?state = await _context.States.FindAsync(Id);
            if (state == null)
            {
                return null;
            }
            return new StateResponse { Id = state.Id, Name = state.Name };
        }
        public async Task<StateResponse> AddStateAsync(StateRequest stateRequest)
        {
            bool stateExists = await _context.States.AnyAsync(s => s.Name.ToLower() == stateRequest.Name.ToLower());

            if (stateExists)
            {
                    return null; 
             
            }
            var state = new State { Name = stateRequest.Name };
            _context.States.Add(state);
            await _context.SaveChangesAsync();
            return new StateResponse { Id = state.Id, Name = state.Name };
        }

        public async Task<bool?> UpdateStateAsync(Guid Id, StateRequest stateRequest )
        {
            bool stateExists = await _context.States.AnyAsync(s => s.Name.ToLower() == stateRequest.Name.ToLower());

            if (stateExists)
            {
                return null; // Indicating state already exists

            }

            var state = await _context.States.FindAsync(Id);
            
            if (state == null) {
                return false; 
            }
             
            state.Name = stateRequest.Name;
            await _context.SaveChangesAsync();
            return true;
        }



    }
}
