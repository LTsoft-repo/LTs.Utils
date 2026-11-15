namespace LTs.Utils;

/// <summary>
///     Utilities for Environment.
/// </summary>
public static class EnvironmentUtils
{
    /// <summary>
    ///     The name of the Azure Functions Environment Name's variable.
    /// </summary>
    [ UsedImplicitly ]
    public const string AzureFunctionsEnvironmentVariable = "AZURE_FUNCTIONS_ENVIRONMENT";

    /// <summary>
    ///     The name of the ASP.NET Core Environment Name's variable.
    /// </summary>
    [ UsedImplicitly ]
    public const string AspNetCoreEnvironmentVariable = "ASPNETCORE_ENVIRONMENT";

    /// <summary>
    ///     Get the name of the environment the application is running in, whether from
    ///     <see cref="AzureFunctionsEnvironmentVariable" /> or
    ///     <see cref="AspNetCoreEnvironmentVariable" />.
    /// </summary>
    /// <returns>The name of the environment the application is running in.</returns>
    public static string GetEnvironmentName()
        => Environment.GetEnvironmentVariable( AzureFunctionsEnvironmentVariable ) ??
           Environment.GetEnvironmentVariable( AspNetCoreEnvironmentVariable ) ??
           "Development";

    /// <summary>
    ///     Expands environment variables in the given string value.
    /// </summary>
    /// <param name="value">The string value containing environment variables to expand.</param>
    /// <returns>The string value with environment variables expanded, or null if the input value is null.</returns>
    public static string? ExpandEnvironmentVariables( this string? value )
        => value != null ? Environment.ExpandEnvironmentVariables( value ) : null;
}