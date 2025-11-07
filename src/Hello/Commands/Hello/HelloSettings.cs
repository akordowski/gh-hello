using Spectre.Console.Cli;
using System.ComponentModel;

namespace Hello.Commands.Hello;

public class HelloSettings : CommandSettings
{
    [CommandOption("-n|--name")]
    [Description("The name")]
    public string Name { get; set; } = default!;
}