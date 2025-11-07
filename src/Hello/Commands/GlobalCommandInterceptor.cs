using Hello.Services;
using Serilog;
using Spectre.Console.Cli;

namespace Hello.Commands;

public class GlobalCommandInterceptor : ICommandInterceptor
{
    private readonly EnvironmentVariableProvider _envVariableProvider;
    private readonly GitHubStatusClient _gitHubStatusClient;
    private readonly VersionProvider _versionProvider;
    private readonly ILogger _logger;

    public GlobalCommandInterceptor(
        EnvironmentVariableProvider envVariableProvider,
        GitHubStatusClient gitHubStatusClient,
        VersionProvider versionProvider,
        ILogger logger)
    {
        ArgumentNullException.ThrowIfNull(envVariableProvider);
        ArgumentNullException.ThrowIfNull(gitHubStatusClient);
        ArgumentNullException.ThrowIfNull(versionProvider);
        ArgumentNullException.ThrowIfNull(logger);

        _envVariableProvider = envVariableProvider;
        _gitHubStatusClient = gitHubStatusClient;
        _versionProvider = versionProvider;
        _logger = logger;
    }

    public void Intercept(CommandContext context, CommandSettings settings)
    {
        GitHubStatusCheck();
        LatestVersionCheck();
    }

    private void GitHubStatusCheck()
    {
        try
        {
            if (_envVariableProvider.SkipStatusCheck()?.ToUpperInvariant() is "TRUE" or "1")
            {
                _logger.Information("Skipped GitHub status check due to HELLO_SKIP_STATUS_CHECK environment variable");
                return;
            }

            if (_gitHubStatusClient.GetUnresolvedIncidentsCountAsync().Result > 0)
            {
                _logger.Warning("GitHub is currently experiencing availability issues. See https://www.githubstatus.com for details.");
            }
        }
        catch (Exception ex)
        {
            _logger.Warning("Could not check GitHub availability from githubstatus.com. See https://www.githubstatus.com for details.");
            _logger.Verbose(ex.ToString());
        }
    }

    private void LatestVersionCheck()
    {
        try
        {
            if (_envVariableProvider.SkipVersionCheck()?.ToUpperInvariant() is "TRUE" or "1")
            {
                _logger.Information("Skipped latest version check due to HELLO_SKIP_VERSION_CHECK environment variable");
                return;
            }

            if (_versionProvider.IsLatest())
            {
                _logger.Information($"You are running an up-to-date version of the hello CLI [v{_versionProvider.GetCurrentVersion()}]");
            }
            else
            {
                _logger.Warning($"You are running an old version of the hello CLI [v{_versionProvider.GetCurrentVersion()}]. The latest version is v{_versionProvider.GetLatestVersion()}.");
                _logger.Warning("Please update by running: gh extension upgrade hello");
            }
        }
        catch (Exception ex)
        {
            _logger.Warning("Could not retrieve latest hello CLI version from github.com, please ensure you are using the latest version by running: gh extension upgrade hello");
            _logger.Verbose(ex.ToString());
        }
    }
}