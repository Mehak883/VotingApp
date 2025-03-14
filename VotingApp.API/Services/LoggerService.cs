﻿using System;
using Microsoft.Extensions.Logging;
using VotingApp.API.Services.Interfaces;

namespace VotingApp.API.Services
{
    public class LoggerService : ILoggerService
    {
        private readonly ILogger<LoggerService> _logger;

        public LoggerService(ILogger<LoggerService> logger)
        {
            _logger = logger;
        }

        public void LogInfo(string message)
        {
            _logger.LogInformation(message);
        }

        public void LogWarning(string message)
        {
            _logger.LogWarning(message);
        }

        public void LogError(string message, Exception? ex)
        {
            if (ex != null)
                _logger.LogError(ex, message);
            else
                _logger.LogError(message);
        }

        public void LogDebug(string message)
        {
            _logger.LogDebug(message);
        }
    }
}
