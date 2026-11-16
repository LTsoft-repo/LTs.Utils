using Autofac;
using LTs.Utils.Time;
using LTs.Utils.Time.Abstractions;
using LTs.Utils.Time.DependencyInjection;

#pragma warning disable IDE0290

namespace LTs.Utils.test.Time.DependencyInjection;

public class RegistrationExtensionsTest : BaseTest
{
    public RegistrationExtensionsTest( ITestOutputHelper testOutput )
        : base( testOutput ) { }

    #region AddDateTimeOffsetProvider
    [ Fact ]
    public void AddDateTimeOffsetProvider_RegistersDateTimeOffsetProviderAsSingleInstance()
    {
        // Arrange
        var builder = new ContainerBuilder();

        // Act
        builder.AddDateTimeOffsetProvider();
        var container = builder.Build();
        var first = container.Resolve<IDateTimeOffsetProvider>();
        var second = container.Resolve<IDateTimeOffsetProvider>();

        // Assert
        first.Should().BeOfType<DateTimeOffsetProvider>();

        second.Should().BeSameAs( first );
    }
    #endregion
}