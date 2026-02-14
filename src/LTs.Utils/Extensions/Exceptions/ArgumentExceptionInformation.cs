namespace LTs.Utils.Extensions.Exceptions;

/// <summary>
///     Represents information about an argument exception.
/// </summary>
public record ArgumentExceptionInformation
{
    /// <summary>
    ///     The name of the parameter that caused the exception.
    /// </summary>
    public required string Parameter { get; init; }

    /// <summary>
    ///     The type of error that occurred.
    /// </summary>
    public ArgumentExceptionErrorType ErrorType { get; init; } = ArgumentExceptionErrorType.Other;
}