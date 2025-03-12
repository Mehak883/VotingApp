using VotingApp.API.DTOs.Candidate;
using VotingApp.API.DTOs.Party;

namespace VotingApp.API.Services.Interfaces
{
    public interface ICandidateService
    {
        Task<List<CandidateResponse>> GetAllCandidateData();
        Task<List<CandidateResponse>> GetCandidateDataByStateId(Guid StateId);
        Task<List<CandidateResponse>> GetCandidateDataByPartyId(Guid PartyId);

        Task<CandidateResponse?> AddCandidateData(CandidateRequest candidateRequest);
        Task<CandidateResponse?> GetCandidateData(Guid id);
        Task<bool?> UpdateCandidateData(Guid Id, CandidateRequest candidateRequest);
        Task<bool> DeleteCandidateData(Guid id);
    }
}
