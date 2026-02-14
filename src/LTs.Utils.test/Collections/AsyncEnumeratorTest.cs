using LTs.Utils.Collections;

namespace LTs.Utils.test.Collections;

public class AsyncEnumeratorTest
{
    #region Constructor
    [ Fact ]
    public void Constructor_WithEnumeratorT_Successes()
    {
        // Arrange
        var enumerator = new List<int> { 1, 4, 5 }.GetEnumerator();

        // Act
        var asyncEnumerator = new AsyncEnumerator<int>( enumerator );

        // Assert
        asyncEnumerator.Should().NotBeNull();
    }
    #endregion

    #region DisposeAsync
    [ Fact ]
    public async Task DisposeAsync_WithList_Successes()
    {
        // Arrange
        var enumerator = new List<int> { 1, 4, 5 }.GetEnumerator();
        var asyncEnumerator = new AsyncEnumerator<int>( enumerator );

        // Act
        await asyncEnumerator.DisposeAsync();

        // Assert
        asyncEnumerator.MoveNextAsync().Should().BeEquivalentTo( new ValueTask<bool>( false ) );
    }
    #endregion

    #region MoveNextAsync
    [ Fact ]
    public async Task MoveNextAsync_Successes()
    {
        // Arrange
        var enumerator = new List<int> { 1, 4, 5 }.GetEnumerator();
        var asyncEnumerator = new AsyncEnumerator<int>( enumerator );

        // Act
        var gotValue = await asyncEnumerator.MoveNextAsync();

        // Assert
        gotValue.Should().BeTrue();
        asyncEnumerator.Current.Should().Be( 1 );

        gotValue = await asyncEnumerator.MoveNextAsync();
        gotValue.Should().BeTrue();
        asyncEnumerator.Current.Should().Be( 4 );

        gotValue = await asyncEnumerator.MoveNextAsync();
        gotValue.Should().BeTrue();
        asyncEnumerator.Current.Should().Be( 5 );

        gotValue = await asyncEnumerator.MoveNextAsync();
        gotValue.Should().BeFalse();
    }
    #endregion
}