using LTs.Utils.Extensions.Enumerables;
using LTs.Utils.test.Infrastructure;

namespace LTs.Utils.test.Extensions.Enumerables;

public class AsyncEnumerableExtensionsTest
{
    #region ToEnumerableAsync
    [ Fact ]
    public async Task ToEnumerableAsync_WithItems_ReturnsAllItemsInOrder()
    {
        // Arrange
        var source = AsyncEnumerableHelper.GetAsyncEnumerable( 1, 2, 3, 4, 5 );

        // Act
        var result = await source.ToEnumerableAsync();

        // Assert
        result.Should().BeEquivalentTo( [ 1, 2, 3, 4, 5 ],
                                        options => options.WithStrictOrdering() );
    }

    [ Fact ]
    public async Task ToEnumerableAsync_Empty_ReturnsEmptyCollection()
    {
        // Arrange
        var source = AsyncEnumerableHelper.GetAsyncEnumerable<int>();

        // Act
        var result = ( await source.ToEnumerableAsync() )
            .ToArray();

        // Assert
        result.Should().NotBeNull();
        result.Should().BeEmpty();
    }

    [ Fact ]
    public async Task ToEnumerableAsync_SingleItem_ReturnsSingleItem()
    {
        // Arrange
        var source = AsyncEnumerableHelper.GetAsyncEnumerable( "test" );

        // Act
        var result = ( await source.ToEnumerableAsync() )
            .ToArray();

        // Assert
        result.Should().ContainSingle();
        result.Single().Should().Be( "test" );
    }

    [ Fact ]
    public async Task ToEnumerableAsync_MultipleAwaitIterations_ReturnsSameResults()
    {
        // Arrange
        var source = AsyncEnumerableHelper.GetAsyncEnumerable( 10, 20, 30 );

        // Act
        var result = await source.ToEnumerableAsync();

        // Assert
        result.Should().Equal( 10, 20, 30 );
    }
    #endregion
}