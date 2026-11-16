using Microsoft.Extensions.DependencyInjection;

namespace LTs.DependencyInjections.test.Infrastructure;

internal class AnyKeyStorage : IStorage
{
    public AnyKeyStorage( [ServiceKey] object resolvedKey )
    {
        ResolvedKey = resolvedKey;
    }

    public object ResolvedKey { get; }

    public string Name => "any-key";
}