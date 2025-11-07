using Spectre.Console.Cli;

namespace Hello.DependencyInjection;

internal sealed class TypeResolver : ITypeResolver, IDisposable
{
    private readonly IServiceProvider _serviceProvider;

    public TypeResolver(IServiceProvider serviceProvider)
    {
        ArgumentNullException.ThrowIfNull(serviceProvider);
        _serviceProvider = serviceProvider;
    }

    public object? Resolve(Type? type) =>
        type is null ? null : _serviceProvider.GetService(type);

    public void Dispose()
    {
        if (_serviceProvider is IDisposable disposable)
        {
            disposable.Dispose();
        }
    }
}