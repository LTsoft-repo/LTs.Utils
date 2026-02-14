using System.Diagnostics.CodeAnalysis;
using LTs.Utils.Comparers.Abstractions;

namespace LTs.Utils.Comparers;

/// <summary>
///     Equality comparer that uses equivalency instead of equality.
/// </summary>
/// <typeparam name="T">Type to compare.</typeparam>
public class EquivalencyEqualityComparer<T> : IEqualityComparer<T>
    where T : class
{
    private readonly IEquivalencyComparer<T> equivalencyComparer;

    /// <summary>
    ///     Creates a new instance of <see cref="EquivalencyEqualityComparer{T}" />.
    /// </summary>
    /// <param name="equivalencyComparer">Equivalency comparer to use.</param>
    // ReSharper disable once ConvertToPrimaryConstructor
    public EquivalencyEqualityComparer( IEquivalencyComparer<T> equivalencyComparer )
        => this.equivalencyComparer = equivalencyComparer;

    /// <inheritdoc />
    public bool Equals( T? x, T? y )
        => equivalencyComparer.IsEquivalent( x, y );

    /// <inheritdoc />
    public int GetHashCode( [ DisallowNull ] T obj )
        => equivalencyComparer.GetEquivalencyHash( obj );
}