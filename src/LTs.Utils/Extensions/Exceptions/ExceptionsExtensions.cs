namespace LTs.Utils.Extensions.Exceptions;

/// <summary>
///     Extensions related to exceptions.
/// </summary>
public static class ExceptionsExtensions
{
    /// <summary>
    ///     Throws the specified exception if the condition is <see langword="false" />.
    /// </summary>
    /// <param name="condition">Boolean condition to check.</param>
    /// <param name="exception">Exception to throw.</param>
    public static void ThrowIfFalse( this bool condition, Exception exception )
    {
        if( !condition )
        {
            throw exception;
        }
    }

    /// <summary>
    ///     Throws the specified exception if the condition is <see langword="true" />.
    /// </summary>
    /// <param name="condition">Boolean condition to check.</param>
    /// <param name="exception">Exception to throw.</param>
    public static void ThrowIfTrue( this bool condition, Exception exception )
    {
        if( condition )
        {
            throw exception;
        }
    }
}