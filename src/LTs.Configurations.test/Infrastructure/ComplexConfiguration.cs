using System.ComponentModel.DataAnnotations;

namespace LTs.Configurations.test.Infrastructure;

internal record ComplexConfiguration
{
    public NestedConfiguration Nested { get; init; } = new();

    public string[] Tags { get; init; } = [ ];

    [ Required ]
    public IReadOnlyList<ItemConfiguration> Items { get; init; } = [ ];
}