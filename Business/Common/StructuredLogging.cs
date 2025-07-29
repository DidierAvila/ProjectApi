using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace Business.Common
{
    public static class StructuredLogging
    {
        public static void LogOperationStart<T>(this ILogger logger, string operation, object parameters = null)
        {
            logger.LogInformation("Operation started: {Operation}", operation);
            logger.LogInformation("Operation details: {@OperationDetails}", new
            {
                Operation = operation,
                HandlerType = typeof(T).Name,
                Parameters = parameters,
                Timestamp = DateTime.UtcNow
            });
        }

        public static void LogOperationSuccess<T>(this ILogger logger, string operation, object result = null, long durationMs = 0)
        {
            logger.LogInformation("Operation completed successfully: {Operation}", operation);
            logger.LogInformation("Operation result: {@OperationResult}", new
            {
                Operation = operation,
                HandlerType = typeof(T).Name,
                Status = "Success",
                DurationMs = durationMs,
                Result = result,
                Timestamp = DateTime.UtcNow
            });
        }

        public static void LogOperationError<T>(this ILogger logger, string operation, Exception exception, object parameters = null)
        {
            logger.LogError(exception, "Operation failed: {Operation}", operation);
            logger.LogError("Operation error details: {@ErrorDetails}", new
            {
                Operation = operation,
                HandlerType = typeof(T).Name,
                Status = "Error",
                ExceptionType = exception.GetType().Name,
                ExceptionMessage = exception.Message,
                Parameters = parameters,
                Timestamp = DateTime.UtcNow
            });
        }

        public static void LogValidationError<T>(this ILogger logger, string operation, IEnumerable<string> validationErrors, object parameters = null)
        {
            logger.LogWarning("Validation failed: {Operation}", operation);
            logger.LogWarning("Validation error details: {@ValidationDetails}", new
            {
                Operation = operation,
                HandlerType = typeof(T).Name,
                Status = "ValidationError",
                ValidationErrors = validationErrors,
                Parameters = parameters,
                Timestamp = DateTime.UtcNow
            });
        }

        public static void LogBusinessRuleViolation<T>(this ILogger logger, string operation, string rule, object parameters = null)
        {
            logger.LogWarning("Business rule violated: {Operation} - {Rule}", operation, rule);
            logger.LogWarning("Business rule violation details: {@ViolationDetails}", new
            {
                Operation = operation,
                HandlerType = typeof(T).Name,
                Status = "BusinessRuleViolation",
                Rule = rule,
                Parameters = parameters,
                Timestamp = DateTime.UtcNow
            });
        }

        public static void LogDatabaseOperation<T>(this ILogger logger, string operation, string query, long durationMs, int rowsAffected = 0)
        {
            logger.LogInformation("Database operation: {Operation}", operation);
            logger.LogInformation("Database operation details: {@DatabaseDetails}", new
            {
                Operation = operation,
                HandlerType = typeof(T).Name,
                Query = query,
                DurationMs = durationMs,
                RowsAffected = rowsAffected,
                Timestamp = DateTime.UtcNow
            });
        }

        public static void LogPerformanceWarning<T>(this ILogger logger, string operation, long durationMs, long thresholdMs = 1000)
        {
            if (durationMs > thresholdMs)
            {
                logger.LogWarning("Performance warning: {Operation} took {DurationMs}ms", operation, durationMs);
                logger.LogWarning("Performance warning details: {@PerformanceDetails}", new
                {
                    Operation = operation,
                    HandlerType = typeof(T).Name,
                    DurationMs = durationMs,
                    ThresholdMs = thresholdMs,
                    Timestamp = DateTime.UtcNow
                });
            }
        }

        public static IDisposable LogOperationScope<T>(this ILogger logger, string operation, object parameters = null)
        {
            return logger.BeginScope(new Dictionary<string, object>
            {
                ["Operation"] = operation,
                ["HandlerType"] = typeof(T).Name,
                ["Parameters"] = parameters,
                ["StartTime"] = DateTime.UtcNow
            });
        }

        public static Stopwatch CreateStopwatch()
        {
            return Stopwatch.StartNew();
        }
    }
} 