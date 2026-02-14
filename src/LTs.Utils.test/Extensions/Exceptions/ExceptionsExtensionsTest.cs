using LTs.Utils.Extensions.Exceptions;

namespace LTs.Utils.test.Extensions.Exceptions;

public class ExceptionsExtensionsTest
{
    #region ThrowIfFalse
    [ Fact ]
    public void ThrowIfFalse_ConditionIsFalse_Throws()
    {
        // Arrange
        var condition = false;

        // Act
        var act = () => condition.ThrowIfFalse( new Exception() );

        // Assert
        act.Should().Throw<Exception>();
    }

    [ Fact ]
    public void ThrowIfFalse_ConditionIsTrue_NoException()
    {
        // Arrange
        var condition = true;

        // Act
        var act = () => condition.ThrowIfFalse( new Exception() );

        // Assert
        act.Should().NotThrow();
    }
    #endregion

    #region ThrowIfTrue
    [ Fact ]
    public void ThrowIfTrue_ConditionIsTrue_Throws()
    {
        // Arrange
        var condition = true;

        // Act
        var act = () => condition.ThrowIfTrue( new Exception() );

        // Assert
        act.Should().Throw<Exception>();
    }

    [ Fact ]
    public void ThrowIfTrue_ConditionIsFalse_NoException()
    {
        // Arrange
        var condition = false;

        // Act
        var act = () => condition.ThrowIfTrue( new Exception() );

        // Assert
        act.Should().NotThrow();
    }
    #endregion
}