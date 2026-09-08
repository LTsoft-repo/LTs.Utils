using LTs.Configurations.Attributes;

namespace LTs.Configurations.test.Infrastructure;

internal record RequiredCollectionConfiguration
{
    [ Required ]
    public IReadOnlyList<ItemConfiguration> Items { get; init; } = [ ];
}
