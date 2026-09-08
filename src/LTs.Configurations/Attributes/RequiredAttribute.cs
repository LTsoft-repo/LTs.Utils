namespace LTs.Configurations.Attributes;

/// <summary>
///     Marks a configuration property as required when loaded by
///     <see cref="Configurations.TypedConfigurationLoader" />.
/// </summary>
[ AttributeUsage( AttributeTargets.Property ) ]
public sealed class RequiredAttribute : Attribute
{
    /// <summary>
    ///     Gets whether an empty string or empty collection is allowed when the setting is present.
    /// </summary>
    public bool AllowEmpty { get; init; }
}
