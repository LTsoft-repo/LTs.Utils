using LTs.Configurations.Attributes;

namespace LTs.Configurations.test.Infrastructure;

internal record SampleConfiguration
{
    [ Required ]
    public required string RequiredName { get; init; }
    
    public  EnumValues EnumValue { get; init; } = EnumValues.None;

    public string OptionalDescription { get; init; } = "default-description";

    public int OptionalCount { get; init; } = 5;

    public Uri OptionalUri { get; init; } = new( "http://localhost/" );

    public TimeSpan OptionalTimeout { get; init; } = TimeSpan.FromMinutes( 1 );
}

internal enum EnumValues
{
    None = 0,
    Value1,
    Value2,
    Value3
}