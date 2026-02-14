using LTs.Utils.Collections;

namespace LTs.Utils.test.Collections;

public class GroupingTest
{
    #region Constructor
    [ Fact ]
    public void Constructor_WithCorrectParameters_Successes()
    {
        // Arrange
        var key = "key";
        var elements = new[] { 1, 2, 154 };

        // Act
        var result = new Grouping<string, int>( key, elements );

        // Assert
        result.Should().NotBeNull();
        result.Key.Should().Be( key );
        result.Should().BeEquivalentTo( elements );
    }
    #endregion

    #region GetEnumerator
    [ Fact ]
    public void GetEnumerator_WithCorrectParameters_Successes()
    {
        // Arrange
        var key = "key";
        var elements = new[] { 1, 2, 154 };

        var grouping = new Grouping<string, int>( key, elements );

        // Act
        using var result = grouping.GetEnumerator();

        // Assert
        result.Should().NotBeNull();
        result.Should().BeAssignableTo<IEnumerator<int>>();
    }
    #endregion
}