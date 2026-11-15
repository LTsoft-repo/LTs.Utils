using LTs.Configurations.Attributes;

namespace LTs.Configurations.test.Infrastructure;

internal record ItemConfiguration
{
    [ Required ]
    public required string Name { get; init; }
}