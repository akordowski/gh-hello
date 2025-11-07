using Serilog.Core;
using Serilog.Events;

// ReSharper disable once CheckNamespace
namespace Serilog;

public class LogEventLevelEnricher : ILogEventEnricher
{
    public void Enrich(LogEvent logEvent, ILogEventPropertyFactory propertyFactory)
    {
        var logEventLevel = logEvent.Level switch
        {
            LogEventLevel.Debug => "DEBUG",
            LogEventLevel.Error => "ERROR",
            LogEventLevel.Fatal => "FATAL",
            LogEventLevel.Information => "INFO",
            LogEventLevel.Verbose => "VERBOSE",
            LogEventLevel.Warning => "WARN",
            _ => string.Empty
        };

        logEvent.AddPropertyIfAbsent(propertyFactory.CreateProperty("LogEventLevel", logEventLevel));
    }
}