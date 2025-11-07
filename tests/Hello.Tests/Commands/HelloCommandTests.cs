using Hello.Commands.Hello;
using Spectre.Console.Testing;

namespace Hello.Tests.Commands;

public class HelloCommandTests
{
    [Fact]
    public async Task Should_Return_Expected_Result()
    {
        // Arrange
        var app = new CommandAppTester();
        app.SetDefaultCommand<HelloCommand>();

        // Act
        var result = await app.RunAsync(new[] { "--name", "John" });

        // Assert
        Assert.Equal(0, result.ExitCode);
        Assert.Equal("Hello John!", result.Output);
    }
}
