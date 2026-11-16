using Microsoft.Extensions.DependencyInjection;

namespace LTs.DependencyInjections.test.Infrastructure;

internal class StringServiceKeyStorage : IStorage
{
    public StringServiceKeyStorage( [ServiceKey] string resolvedKey )
    {
        ResolvedKey = resolvedKey;
    }

    public string ResolvedKey { get; }

    public string Name => ResolvedKey;
}