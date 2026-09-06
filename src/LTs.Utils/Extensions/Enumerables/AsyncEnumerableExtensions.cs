namespace LTs.Utils.Extensions.Enumerables;

/// <summary>
///     Extensions for <see cref="IAsyncEnumerable{T}" />.
/// </summary>
public static class AsyncEnumerableExtensions
{
    /// <summary>
    ///     Converts an <see cref="IAsyncEnumerable{T}" /> to an <see cref="IEnumerable{T}" />.
    /// </summary>
    /// <typeparam name="T">The type of the elements in the enumerable.</typeparam>
    /// <param name="asyncEnumerable">The async enumerable to convert.</param>
    /// <returns>An enumerable with the elements from the async enumerable.</returns>
    public static async Task<IEnumerable<T>> ToEnumerableAsync<T>( this IAsyncEnumerable<T> asyncEnumerable )
    {
        var list = new List<T>();

        await foreach( var item in asyncEnumerable )
        {
            list.Add( item );
        }

        return [ .. list ];
    }
}