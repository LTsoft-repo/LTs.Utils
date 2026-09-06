namespace LTs.Utils.Extensions.Exceptions;

/// <summary>
///     Represents the type of error that occurred in an argument exception.
/// </summary>
public enum ArgumentExceptionErrorType
{
    /// <summary>
    ///     The argument is invalid.
    /// </summary>
    Invalid,

    /// <summary>
    ///     The argument is empty.
    /// </summary>
    Empty,

    /// <summary>
    ///     The argument is null.
    /// </summary>
    Null,

    /// <summary>
    ///     Other type of error.
    /// </summary>
    Other
}