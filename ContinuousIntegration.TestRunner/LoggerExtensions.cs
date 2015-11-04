﻿namespace ContinuousIntegration.TestRunner
{
    using System;
    using Microsoft.Framework.Logging;

    public static class LoggerExtensions
    {
        public static void Info(this ILogger logger, string message)
        {
            logger.LogInformation(FormatLogMessage(message));
        }

        public static void Error(this ILogger logger, string message)
        {
            logger.LogError(FormatLogMessage(message));
        }

        private static string FormatLogMessage(string message)
        {
            return $"[{DateTime.UtcNow}] :    {message}";
        }
    }
}