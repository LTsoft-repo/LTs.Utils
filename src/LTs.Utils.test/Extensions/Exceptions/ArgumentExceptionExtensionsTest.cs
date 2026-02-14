using LTs.Utils.Extensions.Exceptions;

namespace LTs.Utils.test.Extensions.Exceptions;

public class ArgumentExceptionExtensionsTest
{
    #region GetInformation
    [ Fact ]
    public void GetInformation_ArgumentExceptionNull_ReturnsInformation()
    {
        // Arrange
        // ReSharper disable once NotResolvedInText
        var exception = new ArgumentException( "Value cannot be null.", "testParameter" );

        // Act
        var result = exception.GetInformation();

        // Assert
        result.Should().NotBeNull();

        result.Should().BeEquivalentTo( new ArgumentExceptionInformation
        {
            Parameter = "testParameter",
            ErrorType = ArgumentExceptionErrorType.Null
        } );
    }

    [ Fact ]
    public void GetInformation_ArgumentExceptionEmpty_ReturnsInformation()
    {
        // Arrange
        // ReSharper disable once NotResolvedInText
        var exception = new ArgumentException( "Value cannot be an empty string or all whitespace.", "testParameter" );

        // Act
        var result = exception.GetInformation();

        // Assert
        result.Should().NotBeNull();

        result.Should().BeEquivalentTo( new ArgumentExceptionInformation
        {
            Parameter = "testParameter",
            ErrorType = ArgumentExceptionErrorType.Empty
        } );
    }

    [ Fact ]
    public void GetInformation_ArgumentExceptionCustom_ReturnsInformation()
    {
        // Arrange
        // ReSharper disable once NotResolvedInText
        var exception = new ArgumentException( "Test message", "testParameter" );

        // Act
        var result = exception.GetInformation();

        // Assert
        result.Should().NotBeNull();

        result.Should().BeEquivalentTo( new ArgumentExceptionInformation
        {
            Parameter = "testParameter",
            ErrorType = ArgumentExceptionErrorType.Other
        } );
    }

    [ Fact ]
    public void GetInformation_ArgumentExceptionCustomWithoutParameter_ReturnsInformation()
    {
        // Arrange
        // ReSharper disable once NotResolvedInText
        var exception = new ArgumentException( "Test message. (Parameter 'testParameter')" );

        // Act
        var result = exception.GetInformation();

        // Assert
        result.Should().NotBeNull();

        result.Should().BeEquivalentTo( new ArgumentExceptionInformation
        {
            Parameter = "testParameter",
            ErrorType = ArgumentExceptionErrorType.Other
        } );
    }

    [ Fact ]
    public void GetInformation_ArgumentNullExceptionNull_ReturnsInformation()
    {
        // Arrange
        // ReSharper disable once NotResolvedInText
        var exception = new ArgumentNullException( "testParameter" );

        // Act
        var result = exception.GetInformation();

        // Assert
        result.Should().NotBeNull();

        result.Should().BeEquivalentTo( new ArgumentExceptionInformation
        {
            Parameter = "testParameter",
            ErrorType = ArgumentExceptionErrorType.Null
        } );
    }

    [ Fact ]
    public void GetInformation_ArgumentNullExceptionCustom_ReturnsInformation()
    {
        // Arrange
        // ReSharper disable once NotResolvedInText
        var exception = new ArgumentNullException( "testParameter", "Some random exception message." );

        // Act
        var result = exception.GetInformation();

        // Assert
        result.Should().NotBeNull();

        result.Should().BeEquivalentTo( new ArgumentExceptionInformation
        {
            Parameter = "testParameter",
            ErrorType = ArgumentExceptionErrorType.Null
        } );
    }
    #endregion
}