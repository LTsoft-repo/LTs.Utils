namespace LTs.Utils.Comparers.Abstractions;

/// <summary>
///     Equivalency comparer that also serves as EqualityComparer.
/// </summary>
/// <typeparam name="T">Entity type to compare.</typeparam>
public interface IEquivalencyComparer<in T> : IEqualityComparer<T>
    where T : class
{
    /// <summary>
    ///     Determines if two objects are equivalent.
    /// </summary>
    /// <param name="x">Object to compare.</param>
    /// <param name="y">Object to compare.</param>
    /// <returns><see langword="true" /> if objects are equivalent, otherwise <see langword="false" />.</returns>
    bool IsEquivalent( T? x, T? y );

    /// <summary>
    ///     Gets the equivalency hash code for the object.
    /// </summary>
    /// <param name="obj">Object to get the hash code for.</param>
    /// <returns>The hash code for the object.</returns>
    int GetEquivalencyHash( T obj );
}