using LTs.Configurations.Attributes;

namespace LTs.Configurations.test.Infrastructure;

internal record SampleConfiguration
{
    [ Required ]
    public required string RequiredName { get; init; }

    public EnumValues EnumValue { get; init; } = EnumValues.None;

    public string OptionalDescription { get; init; } = "default-description";

    public int OptionalCount { get; init; } = 5;

    public Uri OptionalUri { get; init; } = new( "http://localhost/" );

    public TimeSpan OptionalTimeout { get; init; } = TimeSpan.FromMinutes( 1 );

    public string[] OptionalStringArray { get; init; } = Array.Empty<string>();
}