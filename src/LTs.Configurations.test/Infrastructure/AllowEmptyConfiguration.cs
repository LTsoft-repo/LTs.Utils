using LTs.Configurations.Attributes;

namespace LTs.Configurations.test.Infrastructure;

internal record AllowEmptyConfiguration
{
    [ Required( AllowEmpty = true ) ]
    public string RequiredName { get; init; } = string.Empty;

    [ Required( AllowEmpty = true ) ]
    public IReadOnlyList<ItemConfiguration> Items { get; init; } = [ ];

    public IReadOnlyList<ItemConfiguration> ItemsNotRequired { get; init; } = [ ];

    public IReadOnlyList<ItemConfiguration>? ItemsNull { get; init; }
}