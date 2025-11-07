using Hello;
using Hello.Commands;
using Hello.Commands.Hello;
using Hello.DependencyInjection;
using Hello.Services;
using Microsoft.Extensions.DependencyInjection;
using Serilog.Events;
using Spectre.Console.Cli;
using System.Reflection;

var services = new ServiceCollection()
    .AddLogging(LogEventLevel.Verbose)
    .AddSingleton<EnvironmentVariableProvider>()
    .AddSingleton<GitHubStatusClient>()
    .AddSingleton<GlobalCommandInterceptor>()
    .AddSingleton<VersionProvider>()
    .AddHttpClient();

var registrar = new TypeRegistrar(services);
var resolver = registrar.Build();

var app = new CommandApp(registrar);
app.Configure(config =>
{
    var globalCommandInterceptor = resolver.Resolve(typeof(GlobalCommandInterceptor)) as GlobalCommandInterceptor;

    var name = Assembly.GetExecutingAssembly().GetName().Name!;
    var version = Assembly.GetExecutingAssembly().GetName().Version?.ToString(3)!;

    config.SetApplicationCulture(Global.CultureInfo);
    config.SetApplicationName(name);
    config.SetApplicationVersion(version);
    config.SetInterceptor(globalCommandInterceptor!);

    config.AddCommand<HelloCommand>("hello")
        .WithDescription("Prints a name");
});

await app.RunAsync(args);