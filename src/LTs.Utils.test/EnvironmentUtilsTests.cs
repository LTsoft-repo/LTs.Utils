namespace LTs.Utils.test;

public class EnvironmentUtilsTests : BaseTest
{
    public EnvironmentUtilsTests( ITestOutputHelper testOutput )
        : base( testOutput ) { }

    #region GetEnvironmentName
    [ Fact ]
    public void GetEnvironmentName_FromAspNetEnvironment_ReturnsCorrectEnvironmentName()
    {
        // Arrange
        var expectedEnvironment = "Production";
        Environment.SetEnvironmentVariable( "AZURE_FUNCTIONS_ENVIRONMENT", null );
        Environment.SetEnvironmentVariable( "ASPNETCORE_ENVIRONMENT", expectedEnvironment );

        // Act
        var actualEnvironment = EnvironmentUtils.GetEnvironmentName();

        // Assert
        actualEnvironment.Should().Be( expectedEnvironment );
    }

    [ Fact ]
    public void GetEnvironmentName_FromFunctionAppEnvironment_ReturnsCorrectEnvironmentName()
    {
        // Arrange
        var expectedEnvironment = "Production";
        Environment.SetEnvironmentVariable( "AZURE_FUNCTIONS_ENVIRONMENT", expectedEnvironment );
        Environment.SetEnvironmentVariable( "ASPNETCORE_ENVIRONMENT", null );

        // Act
        var actualEnvironment = EnvironmentUtils.GetEnvironmentName();

        // Assert
        actualEnvironment.Should().Be( expectedEnvironment );
    }

    [ Fact ]
    public void GetEnvironmentName_NoEnvironmentDefined_ReturnsDevelopment()
    {
        // Arrange
        Environment.SetEnvironmentVariable( "AZURE_FUNCTIONS_ENVIRONMENT", null );
        Environment.SetEnvironmentVariable( "ASPNETCORE_ENVIRONMENT", null );

        // Act
        var actualEnvironment = EnvironmentUtils.GetEnvironmentName();

        // Assert
        actualEnvironment.Should().Be( "Development" );
    }
    #endregion

    #region ExpandEnvironmentVariables
    [ Fact ]
    public void ExpandEnvironmentVariables_WhenValueIsNull_ReturnsNull()
    {
        // Arrange
        string? value = null;

        // Act
        var result = value.ExpandEnvironmentVariables();

        // Assert
        result.Should().BeNull();
    }

    [ Fact ]
    public void ExpandEnvironmentVariables_WhenValueHasNoVariables_ReturnsSameValue()
    {
        // Arrange
        const string value = "plain text";

        // Act
        var result = value.ExpandEnvironmentVariables();

        // Assert
        result.Should().Be( value );
    }

    [ Fact ]
    public void ExpandEnvironmentVariables_WhenValueContainsVariable_ExpandsVariable()
    {
        // Arrange
        const string variableName = "LTs_Utils_ExpandEnvironmentVariables_Test";
        const string variableValue = "expanded-value";
        const string input = $"prefix-%{variableName}%-suffix";
        Environment.SetEnvironmentVariable( variableName, variableValue );

        // Act
        var result = input.ExpandEnvironmentVariables();

        // Assert
        result.Should().Be( $"prefix-{variableValue}-suffix" );

        Environment.SetEnvironmentVariable( variableName, null );
    }
    #endregion
}