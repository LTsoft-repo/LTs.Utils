using Microsoft.Extensions.DependencyInjection;

namespace LTs.DependencyInjections.test.Infrastructure;

internal class GuidServiceKeyStorageNoConverter : IStorage
{
    public GuidServiceKeyStorageNoConverter( [ServiceKey] Guid resolvedKey )
    {
        ResolvedKey = resolvedKey;
    }

    public Guid ResolvedKey { get; }

    public string Name => ResolvedKey.ToString();
}