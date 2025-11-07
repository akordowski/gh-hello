using Serilog;
using System.Reflection;

namespace Hello.Services;

public class VersionProvider
{
    private readonly HttpClient _httpClient;
    private readonly ILogger _logger;

    private string? _latestVersion;

    public VersionProvider(HttpClient httpClient, ILogger logger)
    {
        ArgumentNullException.ThrowIfNull(httpClient);
        ArgumentNullException.ThrowIfNull(logger);

        _httpClient = httpClient;
        _logger = logger;
    }

    public string GetCurrentVersion() =>
        Assembly.GetExecutingAssembly().GetName().Version!.ToString(3);

    public string GetLatestVersion()
    {
        if (string.IsNullOrWhiteSpace(_latestVersion))
        {
            const string url = "https://raw.githubusercontent.com/akordowski/gh-hello/main/LATEST-VERSION.txt";

            _logger.Verbose($"HTTP GET: {url}");

            var response = _httpClient.GetAsync(url).Result;
            var content = response.Content.ReadAsStringAsync().Result.Trim();

            _logger.Verbose($"RESPONSE ({response.StatusCode}): {content}");

            response.EnsureSuccessStatusCode();

            _latestVersion = content.TrimStart('v', 'V').Trim();
        }

        return _latestVersion;
    }

    public bool IsLatest()
    {
        var currentVersion = Version.Parse(GetCurrentVersion());
        var latestVersion = Version.Parse(GetLatestVersion());

        return currentVersion >= latestVersion;
    }
}