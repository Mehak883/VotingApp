using Newtonsoft.Json;
using VotingApp.API.DTOs;
using VotingApp.API.Exceptions;
using VotingApp.API.Services.Interfaces;

namespace VotingApp.API.Services
{
    public class VoteSessionService : IVoteSessionService
    {
        private readonly string _filePath;

        public VoteSessionService()
        {
            _filePath = Path.Combine(Directory.GetCurrentDirectory(), "VotingSession.json");
            //if (!System.IO.File.Exists(_filePath))
            //{
                //var defaultTimings = new VotingTimingDTO
                //{
                //    StartTime = "2025-03-12T09:00:00Z",
                //    EndTime = "2025-03-12T18:00:00Z"
                //};

                //SaveVotingTimings(defaultTimings);
            //}
        }
       public VotingTimingDTO GetVoteTimings()
        {
            var votingTimings = LoadVotingTimings();
            return votingTimings;
        }

        public bool UpdateVoteTiming(VotingTimingDTO newTimings)
        {
            if (newTimings == null)
                throw new BadRequestException("Vote timings are null");

            if (!DateTime.TryParse(newTimings.StartTime, out DateTime newStartTime))
                throw new BadRequestException("Invalid start time format.");

            if (!DateTime.TryParse(newTimings.EndTime, out DateTime newEndTime))
                throw new BadRequestException("Invalid end time format.");

            VotingTimingDTO prevVotingTiming = LoadVotingTimings();
            DateTime currentTime = DateTime.Now;

            if (!DateTime.TryParse(prevVotingTiming.StartTime, out DateTime prevStartTime))
                throw new BadRequestException("Invalid previous start time format.");

            if (newStartTime < currentTime)
                throw new BadRequestException("Start time must not be before the current time.");

            if (newEndTime <= newStartTime)
                throw new BadRequestException("End time must be after start time.");

            if (prevStartTime < currentTime)
            {
                throw new BadRequestException("Voting timings cannot be updated after the previous start time.");
            }
            newTimings.StartTime = newStartTime.ToString("yyyy-MM-dd HH:mm:ss");
            newTimings.EndTime = newEndTime.ToString("yyyy-MM-dd HH:mm:ss");

            SaveVotingTimings(newTimings);
            return true;
           
        }
        public VotingTimingDTO LoadVotingTimings()
        {
            string json = System.IO.File.ReadAllText(_filePath);
            return JsonConvert.DeserializeObject<VotingTimingDTO>(json);
        }

        public void SaveVotingTimings(VotingTimingDTO timings)
        {
            string json = JsonConvert.SerializeObject(timings, Newtonsoft.Json.Formatting.Indented);
            System.IO.File.WriteAllText(_filePath, json);
        }
    }
}
