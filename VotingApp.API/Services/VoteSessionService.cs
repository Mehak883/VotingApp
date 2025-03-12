using Newtonsoft.Json;
using VotingApp.API.DTOs;
using VotingApp.API.Exceptions;
using VotingApp.API.Services.Interfaces;

namespace VotingApp.API.Services
{
    public class VoteSessionService : IVoteSessionService
    {
        private readonly string _filePath;
        private readonly ILoggerService _logger;

        public VoteSessionService(ILoggerService logger)
        {
            _logger = logger;
            _filePath = Path.Combine(Directory.GetCurrentDirectory(), "VotingSession.json");
            _logger.LogInfo($"file fetched: {_filePath}");
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
            _logger.LogInfo("Fetching voting timings");
            var votingTimings = LoadVotingTimings();
            _logger.LogInfo($"Getting voting timings:  {votingTimings.StartTime}, {votingTimings.EndTime}");
            return votingTimings;
        }

        public bool UpdateVoteTiming(VotingTimingDTO newTimings)
        {
            _logger.LogInfo("Updating vote timings.");
            if (newTimings == null)
            {
                _logger.LogError("Vote timings are null.");
                throw new BadRequestException("Vote timings are null");
            }
            if (!DateTime.TryParse(newTimings.StartTime, out DateTime newStartTime))
            {
                _logger.LogError($"Invalid start time format: {newTimings.StartTime}");
                throw new BadRequestException("Invalid start time format.");
            }

            if (!DateTime.TryParse(newTimings.EndTime, out DateTime newEndTime))
            {
                _logger.LogError($"Invalid end time format: {newTimings.EndTime}");

                throw new BadRequestException("Invalid end time format.");
            }

            VotingTimingDTO prevVotingTiming = LoadVotingTimings();
            DateTime currentTime = DateTime.Now;

            if (!DateTime.TryParse(prevVotingTiming.StartTime, out DateTime prevStartTime))
            {
                _logger.LogError($"Invalid previous start time format: {prevVotingTiming.StartTime}");
                throw new BadRequestException("Invalid previous start time format.");
            }

            if (newStartTime < currentTime) {
                _logger.LogError($"New start time {newStartTime} is before current time {currentTime}");
                throw new BadRequestException("Start time must not be before the current time.");
        }

            if (newEndTime <= newStartTime)
            {
                _logger.LogError($"New end time {newEndTime} must be after start time {newStartTime}");
                throw new BadRequestException("End time must be after start time.");
            }

            if (prevStartTime < currentTime)
            {
                _logger.LogError($"Previous start time {prevStartTime} has already passed. Cannot update timings.");
                throw new BadRequestException("Voting timings cannot be updated after the previous start time.");
            }
            newTimings.StartTime = newStartTime.ToString("yyyy-MM-dd HH:mm:ss");
            newTimings.EndTime = newEndTime.ToString("yyyy-MM-dd HH:mm:ss");

            SaveVotingTimings(newTimings);
            _logger.LogInfo("Vote timings updated: {newTimings.StartTime} - {newTimings.EndTime}");
            return true;
           
        }
        public VotingTimingDTO LoadVotingTimings()
        {
            _logger.LogInfo("Loading voting timings from file.");

            string json = System.IO.File.ReadAllText(_filePath);
            return JsonConvert.DeserializeObject<VotingTimingDTO>(json);
        }

        public void SaveVotingTimings(VotingTimingDTO timings)
        {
            _logger.LogInfo($"Saving voting timings: {timings.StartTime} - {timings.EndTime}");

            string json = JsonConvert.SerializeObject(timings, Newtonsoft.Json.Formatting.Indented);
            System.IO.File.WriteAllText(_filePath, json);
            _logger.LogInfo("Voting timings saved successfully.");

        }
    }
}
