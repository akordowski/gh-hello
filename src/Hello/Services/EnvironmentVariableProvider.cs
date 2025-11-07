namespace Hello.Services;

public class EnvironmentVariableProvider
{
    private const string HELLO_SKIP_STATUS_CHECK = "HELLO_SKIP_STATUS_CHECK";
    private const string HELLO_SKIP_VERSION_CHECK = "HELLO_SKIP_VERSION_CHECK";

    public string? SkipStatusCheck() =>
        Environment.GetEnvironmentVariable(HELLO_SKIP_STATUS_CHECK);

    public string? SkipVersionCheck() =>
        Environment.GetEnvironmentVariable(HELLO_SKIP_VERSION_CHECK);
}