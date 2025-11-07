using Serilog;
using System.Text.Json;

namespace Hello.Services;

public class GitHubStatusClient
{
    private readonly HttpClient _httpClient;
    private readonly ILogger _logger;

    public GitHubStatusClient(HttpClient httpClient, ILogger logger)
    {
        ArgumentNullException.ThrowIfNull(httpClient);
        ArgumentNullException.ThrowIfNull(logger);

        _httpClient = httpClient;
        _logger = logger;
    }

    public async Task<int> GetUnresolvedIncidentsCountAsync()
    {
        const string url = "https://www.githubstatus.com/api/v2/incidents/unresolved.json";

        _logger.Verbose($"HTTP GET: {url}");

        var response = await _httpClient.GetAsync(url);
        var content = await response.Content.ReadAsStringAsync();

        _logger.Verbose($"RESPONSE ({response.StatusCode}): {content}");

        response.EnsureSuccessStatusCode();

        using var doc = JsonDocument.Parse(content);
        var incidents = doc.RootElement.GetProperty("incidents");

        return incidents.GetArrayLength();
    }
}