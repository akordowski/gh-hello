using Spectre.Console;
using Spectre.Console.Cli;

namespace Hello.Commands.Hello;

public class HelloCommand : AsyncCommand<HelloSettings>
{
    private readonly IAnsiConsole _console;

    public HelloCommand(IAnsiConsole console)
    {
        _console = console;
    }

    public override Task<int> ExecuteAsync(
        CommandContext context,
        HelloSettings settings,
        CancellationToken cancellationToken)
    {
        _console.WriteLine($"Hello {settings.Name}!");
        return Task.FromResult(0);
    }

    public override ValidationResult Validate(CommandContext context, HelloSettings settings)
    {
        if (string.IsNullOrWhiteSpace(settings.Name))
        {
            return ValidationResult.Error("Name is required");
        }

        return base.Validate(context, settings);
    }
}