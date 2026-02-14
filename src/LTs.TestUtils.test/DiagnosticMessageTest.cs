namespace LTs.TestUtils.test;

public class DiagnosticMessageTest
{
    #region Constructror
    [ Fact ]
    public void Ctor_WithoutParameters_GetsNullMessage()
    {
        // Arrange

        // Act
        var message = new DiagnosticMessage();

        // Assert
        message.Message.Should().BeNull();
    }

    [ Fact ]
    public void Ctor_WithMessage_SetsMessageSuccessfully()
    {
        // Arrange
        var messageText = "Test message";

        // Act
        var message = new DiagnosticMessage( messageText );

        // Assert
        message.Message.Should().Be( messageText );
    }

    [ Fact ]
    public void Ctor_WithFormattedMessage_SetsMessageSuccessfully()
    {
        // Arrange
        var messageText = "Test {0}";
        var expectedMessageText = "Test message";

        // Act
        var message = new DiagnosticMessage( messageText, "message" );

        // Assert
        message.Message.Should().Be( expectedMessageText );
    }
    #endregion

    #region ToString
    [ Fact ]
    public void ToString_WithMessage_ReturnsMessage()
    {
        // Arrange
        var messageText = "Test message";
        var message = new DiagnosticMessage( messageText );

        // Act
        var result = message.ToString();

        // Assert
        result.Should().Be( messageText );
    }

    [ Fact ]
    public void ToString_WithoutMessage_ReturnsNull()
    {
        // Arrange
        var message = new DiagnosticMessage();

        // Act
        var result = message.ToString();

        // Assert
        result.Should().BeNull();
    }

    [ Fact ]
    public void ToString_WithFormattedMessage_ReturnsFormattedMessage()
    {
        // Arrange
        var messageText = "Test {0}";
        var expectedMessageText = "Test message";

        var message = new DiagnosticMessage( messageText, "message" );

        // Act
        var result = message.ToString();

        // Assert
        result.Should().Be( expectedMessageText );
    }
    #endregion

    #region InterfaceTypes
    [ Fact ]
    public void InterfaceTypes_ShouldContainIDiagnosticMessage()
    {
        // Arrange

        // Act
        var interfaceTypes = DiagnosticMessage.InterfaceTypes;

        // Asser
        interfaceTypes.Should().Contain( typeof( IDiagnosticMessage ).FullName );
        interfaceTypes.Should().Contain( typeof( IMessageSinkMessage ).FullName );
    }
    #endregion
}