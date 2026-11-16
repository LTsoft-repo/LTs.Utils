using Microsoft.Extensions.DependencyInjection;

namespace LTs.DependencyInjections.test.Infrastructure;

internal class IntServiceKeyStorage : IStorage
{
    public IntServiceKeyStorage( [ServiceKey] int resolvedKey )
    {
        ResolvedKey = resolvedKey;
    }

    public int ResolvedKey { get; }

    public string Name => ResolvedKey.ToString();
}