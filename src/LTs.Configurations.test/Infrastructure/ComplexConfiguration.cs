using LTs.Configurations.Attributes;

namespace LTs.Configurations.test.Infrastructure;

internal record ComplexConfiguration
{
    [ Required ]
    public required NestedConfiguration Nested { get; init; }

    public string[] Tags { get; init; } = [ ];

    [ Required ]
    public required IReadOnlyList<ItemConfiguration> Items { get; init; }

    public int OptionalCount { get; init; }
}