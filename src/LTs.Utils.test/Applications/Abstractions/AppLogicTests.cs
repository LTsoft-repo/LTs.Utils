using Autofac;
using LTs.Utils.Applications.Abstractions;
using Moq;

namespace LTs.Utils.test.Applications.Abstractions;

public class AppLogicTests
{
    #region IAppLogic<T>
    [ Fact ]
    public void Class_DerivesFromIAppLogic()
    {
        // Arrange

        // Act
        var appLogic = new Mock<IAppLogic<object>>();

        // Assert
        appLogic.Object.Should().BeAssignableTo<IAppLogic>();
    }
    #endregion

    #region RunAsync
    [ Fact ]
    public async Task RunAsync_ShouldExecuteWithoutExceptions()
    {
        // Arrange
        var appLogicMock = new Mock<IAppLogic>();

        appLogicMock.Setup( al => al.RunAsync( It.IsAny<ILifetimeScope>() ) )
                    .Returns( Task.CompletedTask );

        var lifetimeScopeMock = new Mock<ILifetimeScope>();

        // Act
        var act = async () => await appLogicMock.Object.RunAsync( lifetimeScopeMock.Object );

        // Assert
        await act.Should().NotThrowAsync();
        appLogicMock.Verify( al => al.RunAsync( It.IsAny<ILifetimeScope>() ), Times.Once );
    }

    [ Fact ]
    public async Task RunAsync_WithException_Throws()
    {
        // Arrange
        var appLogicMock = new Mock<IAppLogic>();

        appLogicMock.Setup( al => al.RunAsync( It.IsAny<ILifetimeScope>() ) )
                    .ThrowsAsync( new InvalidOperationException( "Test Exception" ) );

        var lifetimeScopeMock = new Mock<ILifetimeScope>();

        // Act
        var act = async () => await appLogicMock.Object.RunAsync( lifetimeScopeMock.Object );

        // Assert
        await act.Should()
                 .ThrowAsync<InvalidOperationException>()
                 .WithMessage( "Test Exception" );
    }
    #endregion

    #region Dispose
    [ Fact ]
    public void Dispose_WithInstance_Successes()
    {
        // Arrange
        var appLogicMock = new Mock<IAppLogic>();
        appLogicMock.Setup( al => al.Dispose() );

        // Act
        var act = () => appLogicMock.Object.Dispose();

        // Assert
        act.Should().NotThrow();
        appLogicMock.Verify( al => al.Dispose(), Times.Once );
    }
    #endregion
}