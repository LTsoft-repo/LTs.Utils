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
}