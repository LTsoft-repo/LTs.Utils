namespace LTs.Utils.test;

public class EnvironmentUtilsTests
{
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
}