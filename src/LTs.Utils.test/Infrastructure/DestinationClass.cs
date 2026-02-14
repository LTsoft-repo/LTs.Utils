namespace LTs.Utils.test.Infrastructure;

internal record DestinationClass
{
    public string PropertyA { get; set; } = string.Empty;
    public int PropertyB { get; set; }
    public DateTime PropertyC { get; set; }

    public string PropertyD { get; set; } = "Default"; // Not in SourceClass
}