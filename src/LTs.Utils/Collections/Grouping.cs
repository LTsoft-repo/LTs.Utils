using System.Collections;

namespace LTs.Utils.Collections;

/// <summary>
///     Represents a collection of objects that have a common key.
/// </summary>
/// <typeparam name="TKey">The type of the key.</typeparam>
/// <typeparam name="TElement">The type of the values.</typeparam>
public class Grouping<TKey, TElement> : IGrouping<TKey, TElement>
{
    /// <summary>
    ///     Creates an empty <see cref="Grouping{TKey,TElement}" /> instance.
    /// </summary>
    /// <returns></returns>
    public static Grouping<TKey, TElement> Empty() => new();

    /// <inheritdoc />
    public TKey Key { get; }

    private readonly IEnumerable<TElement> elements;

    /// <summary>
    ///     Internal constructor for the default instance.
    /// </summary>
    [ UsedImplicitly ]
    protected Grouping()
    {
        Key = default!;
        elements = Array.Empty<TElement>();
    }

    /// <summary>
    ///     Initializes a new instance of the <see cref="Grouping{TKey,TElement}" /> class.
    /// </summary>
    /// <param name="key"></param>
    /// <param name="elements"></param>
    // ReSharper disable once ConvertToPrimaryConstructor
    public Grouping( TKey key, IEnumerable<TElement> elements )
    {
        Key = key;
        this.elements = elements;
    }

    /// <inheritdoc />
    public IEnumerator<TElement> GetEnumerator()
        => elements.GetEnumerator();

    IEnumerator IEnumerable.GetEnumerator()
        => GetEnumerator();
}