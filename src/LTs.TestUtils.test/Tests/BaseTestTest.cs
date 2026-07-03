using LTs.TestUtils.test.Infrastructure;

namespace LTs.TestUtils.test.Tests;

public class BaseTestTest
{
    private readonly ITestOutputHelper testOutput;

    public BaseTestTest( ITestOutputHelper testOutput )
        => this.testOutput = testOutput;

    #region TestOutput
    [ Fact ]
    public void TestOutput_WithTestOutput_ReturnsTestOutput()
    {
        // Arrange
        var test = new TestClass( testOutput );

        // Act
        var output = test.GetTestOutput();

        // Assert
        output.Should().BeSameAs( testOutput );
    }
    #endregion

    #region Dispose
    [ Fact ]
    public void Dispose_WithDisposables_DisposesSuccessfully()
    {
        // Arrange
        var disposable1 = new DisposableClass();
        var disposable2 = new DisposableClass();
        var disposable3 = new DisposableClass();

        var test = new TestClass( testOutput );
        test.AddDisposable( disposable1 );
        test.AddDisposable( disposable2 );
        test.AddDisposable( disposable3 );

        // Act
        test.Dispose();

        // Assert
        disposable1.IsDisposed.Should().BeTrue();
        disposable2.IsDisposed.Should().BeTrue();
        disposable3.IsDisposed.Should().BeTrue();

        var disposables = test.GetDisposables();
        disposables.Should().BeEmpty();
    }

    [ Fact ]
    public void Dispose_NoDisposables_Successes()
    {
        // Arrange
        var test = new TestClass( testOutput );

        // Act
        test.Dispose();

        // Assert
        var disposables = test.GetDisposables();
        disposables.Should().BeEmpty();
    }
    #endregion
}