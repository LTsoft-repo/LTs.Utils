using LTs.Utils.Collections;

namespace LTs.Utils.test.Collections;

public class AsyncEnumerableTest
{
    #region Constructor
    [ Fact ]
    public void Constructor_WithEnumerable_Successes()
    {
        // Arrange
        var enumerable = new[] { 1, 4, 5 };

        // Act
        var asyncEnumerable = new AsyncEnumerable<int>( enumerable );

        // Assert
        asyncEnumerable.Should().NotBeNull();
    }

    [ Fact ]
    public void Constructor_WithExpression_Successes()
    {
        // Arrange
        var expression = new[] { 1, 4, 5 }.AsQueryable().Expression;

        // Act
        var asyncEnumerable = new AsyncEnumerable<int>( expression );

        // Assert
        asyncEnumerable.Should().NotBeNull();
    }
    #endregion

    #region GetAsyncEnumerator
    [ Fact ]
    public async Task GetAsyncEnumerator_WithEnumerable_Successes()
    {
        // Arrange
        var values = new[] { 1, 23 };
        var asyncEnumerable = new AsyncEnumerable<int>( values );

        // Act
        await using var result = asyncEnumerable.GetAsyncEnumerator();

        // Assert
        result.Should().NotBeNull();

        var gotValue = await result.MoveNextAsync();
        gotValue.Should().BeTrue();
        result.Current.Should().Be( values[ 0 ] );

        gotValue = await result.MoveNextAsync();
        gotValue.Should().BeTrue();
        result.Current.Should().Be( values[ 1 ] );

        gotValue = await result.MoveNextAsync();
        gotValue.Should().BeFalse();
    }

    [ Fact ]
    public async Task GetAsyncEnumerator_WithExpression_Successes()
    {
        // Arrange
        var values = new[] { 1, 23 };
        var expression = values.AsQueryable().Expression;
        var asyncEnumerable = new AsyncEnumerable<int>( expression );

        // Act
        await using var result = asyncEnumerable.GetAsyncEnumerator();

        // Assert
        result.Should().NotBeNull();

        var gotValue = await result.MoveNextAsync();
        gotValue.Should().BeTrue();
        result.Current.Should().Be( values[ 0 ] );

        gotValue = await result.MoveNextAsync();
        gotValue.Should().BeTrue();
        result.Current.Should().Be( values[ 1 ] );

        gotValue = await result.MoveNextAsync();
        gotValue.Should().BeFalse();
    }
    #endregion
}