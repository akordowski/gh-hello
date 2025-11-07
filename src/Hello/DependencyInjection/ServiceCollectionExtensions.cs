using Microsoft.Extensions.DependencyInjection;
using Serilog;
using Serilog.Events;

namespace Hello.DependencyInjection;

internal static class ServiceCollectionExtensions
{
    public static IServiceCollection AddLogging(this IServiceCollection services, LogEventLevel logEventLevel) =>
        services.AddSingleton<ILogger>(_ =>
        {
            // Creating the logger via the factory method prevents from creating
            // an empty log file when calling the --help or --version options.

            const string outputTemplate = "[{Timestamp:yyyy-MM-dd HH:mm:ss}] [{LogEventLevel}] {Message}{NewLine}";
            var path = $"{DateTime.Now:yyyyMMddHHmmss}-{Environment.ProcessId}.extension.log";

            var loggerConfig = new LoggerConfiguration()
                .MinimumLevel.Is(logEventLevel)
                .Enrich.FromLogContext()
                .Enrich.With<LogEventLevelEnricher>()
                .WriteTo.Console(
                    outputTemplate: outputTemplate,
                    formatProvider: Global.CultureInfo)
                .WriteTo.File(
                    path,
                    outputTemplate: outputTemplate,
                    formatProvider: Global.CultureInfo);

            return loggerConfig.CreateLogger();
        });
}