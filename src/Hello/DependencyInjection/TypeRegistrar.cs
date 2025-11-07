using Microsoft.Extensions.DependencyInjection;
using Spectre.Console.Cli;

namespace Hello.DependencyInjection;

public class TypeRegistrar : ITypeRegistrar
{
    private readonly IServiceCollection _serviceCollection;

    public TypeRegistrar(IServiceCollection serviceCollection)
    {
        ArgumentNullException.ThrowIfNull(serviceCollection);
        _serviceCollection = serviceCollection;
    }

    public ITypeResolver Build() =>
        new TypeResolver(_serviceCollection.BuildServiceProvider());

    public void Register(Type service, Type implementation)
    {
        ArgumentNullException.ThrowIfNull(service);
        ArgumentNullException.ThrowIfNull(implementation);

        _serviceCollection.AddSingleton(service, implementation);
    }

    public void RegisterInstance(Type service, object implementation)
    {
        ArgumentNullException.ThrowIfNull(service);
        ArgumentNullException.ThrowIfNull(implementation);

        _serviceCollection.AddSingleton(service, implementation);
    }

    public void RegisterLazy(Type service, Func<object> func)
    {
        ArgumentNullException.ThrowIfNull(service);
        ArgumentNullException.ThrowIfNull(func);

        _serviceCollection.AddSingleton(service, _ => func());
    }
}