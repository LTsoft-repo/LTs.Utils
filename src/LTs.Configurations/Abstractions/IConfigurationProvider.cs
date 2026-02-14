namespace LTs.Configurations.Abstractions;

/// <summary>
///     Configuration provider to register and get configuration classes.
/// </summary>
public interface IConfigurationProvider
{
    /// <summary>
    ///     Gets a configuration of the specified type.
    /// </summary>
    /// <typeparam name="T">The type of the configuration to get.</typeparam>
    /// <returns>The configuration of the specified type.</returns>
    T Get<T>() where T : notnull;
}