using LTs.Configurations.Exceptions;
using Microsoft.Extensions.Configuration;

namespace LTs.Configurations.test.Exceptions;

public class ConfigurationExceptionTest
{
    #region ThrowIfNull
    [ Fact ]
    public void ThrowIfNull_NullValue_Throws()
    {
        // Arrange
        var configuration = new ConfigurationBuilder()
                            .AddInMemoryCollection( new Dictionary<string, string?>
                            {
                                { "key1", "value1" }
                            } ).Build();

        var name = "key2";
        var section = configuration;
        var value = section.GetValue<string>( name );

        // Act
        var act = () => ConfigurationException.ThrowIfNull( value, name, section );

        // Assert
        act.Should().Throw<ConfigurationException>()
           .WithMessage( $"Configuration parameter '{name}' not defined." );
    }

    [ Fact ]
    public void ThrowIfNull_NullValueWithSection_Throws()
    {
        // Arrange
        var configuration = new ConfigurationBuilder()
                            .AddInMemoryCollection( new Dictionary<string, string?>
                            {
                                { "section:key1", "value1" }
                            } ).Build();

        var name = "key2";
        var section = configuration.GetSection( "section" );
        var value = section.GetValue<string>( name );

        // Act
        var act = () => ConfigurationException.ThrowIfNull( value, name, section );

        // Assert
        act.Should().Throw<ConfigurationException>()
           .WithMessage( $"Configuration parameter 'section:{name}' not defined." );
    }

    [ Fact ]
    public void ThrowIfNull_NonNullValue_DoesNotThrow()
    {
        // Arrange
        var configuration = new ConfigurationBuilder()
                            .AddInMemoryCollection( new Dictionary<string, string?>
                            {
                                { "section:key1", "value1" }
                            } ).Build();

        var name = "key1";
        var section = configuration.GetSection( "section" );
        var value = section.GetValue<string>( name );

        // Act
        var act = () => ConfigurationException.ThrowIfNull( value, name, section );

        // Assert
        act.Should().NotThrow();
    }
    #endregion

    #region ThrowIfNullOrEmpty
    [ Fact ]
    public void ThrowIfNullOrEmpty_NullValue_Throws()
    {
        // Arrange
        var configuration = new ConfigurationBuilder()
                            .AddInMemoryCollection( new Dictionary<string, string?>
                            {
                                { "key1", "value1" }
                            } ).Build();

        var name = "key2";
        var section = configuration;
        var value = section.GetValue<string>( name );

        // Act
        var act = () => ConfigurationException.ThrowIfNullOrEmpty( value, name, section );

        // Assert
        act.Should().Throw<ConfigurationException>()
           .WithMessage( $"Configuration parameter '{name}' cannot be null or empty." );
    }

    [ Fact ]
    public void ThrowIfNullOrEmpty_EmptyValue_Throws()
    {
        // Arrange
        var configuration = new ConfigurationBuilder()
                            .AddInMemoryCollection( new Dictionary<string, string?>
                            {
                                { "key1", "" }
                            } ).Build();

        var name = "key1";
        var section = configuration;
        var value = section.GetValue<string>( name );

        // Act
        var act = () => ConfigurationException.ThrowIfNullOrEmpty( value, name, section );

        // Assert
        act.Should().Throw<ConfigurationException>()
           .WithMessage( $"Configuration parameter '{name}' cannot be null or empty." );
    }

    [ Fact ]
    public void ThrowIfNullOrEmpty_NullValueWithSection_Throws()
    {
        // Arrange
        var configuration = new ConfigurationBuilder()
                            .AddInMemoryCollection( new Dictionary<string, string?>
                            {
                                { "section:key1", "value1" }
                            } ).Build();

        var name = "key2";
        var section = configuration.GetSection( "section" );
        var value = section.GetValue<string>( name );

        // Act
        var act = () => ConfigurationException.ThrowIfNullOrEmpty( value, name, section );

        // Assert
        act.Should().Throw<ConfigurationException>()
           .WithMessage( $"Configuration parameter 'section:{name}' cannot be null or empty." );
    }

    [ Fact ]
    public void ThrowIfNullOrEmpty_EmptyValueCustomMessage_Throws()
    {
        // Arrange
        var configuration = new ConfigurationBuilder()
                            .AddInMemoryCollection( new Dictionary<string, string?>
                            {
                                { "key1", "" }
                            } ).Build();

        var name = "key1";
        var section = configuration;
        var value = section.GetValue<string>( name );

        // Act
        var act = () => ConfigurationException.ThrowIfNullOrEmpty( value, $"The configuration {name} cannot be empty" );

        // Assert
        act.Should().Throw<ConfigurationException>()
           .WithMessage( $"The configuration {name} cannot be empty" );
    }

    [ Fact ]
    public void ThrowIfNullOrEmpty_NonNullValue_DoesNotThrow()
    {
        // Arrange
        var configuration = new ConfigurationBuilder()
                            .AddInMemoryCollection( new Dictionary<string, string?>
                            {
                                { "section:key1", "value1" }
                            } ).Build();

        var name = "key1";
        var section = configuration.GetSection( "section" );
        var value = section.GetValue<string>( name );

        // Act
        var act = () => ConfigurationException.ThrowIfNullOrEmpty( value, name, section );

        // Assert
        act.Should().NotThrow();
    }

    [ Fact ]
    public void ThrowIfNullOrEmpty_WhiteSpaceValue_DoesNotThrow()
    {
        // Arrange
        var configuration = new ConfigurationBuilder()
                            .AddInMemoryCollection( new Dictionary<string, string?>
                            {
                                { "section:key1", " " }
                            } ).Build();

        var name = "key1";
        var section = configuration.GetSection( "section" );
        var value = section.GetValue<string>( name );

        // Act
        var act = () => ConfigurationException.ThrowIfNullOrEmpty( value, name, section );

        // Assert
        act.Should().NotThrow();
    }
    #endregion

    #region ThrowIfNullOrWhiteSpace
    [ Fact ]
    public void ThrowIfNullOrWhiteSpace_NullValue_Throws()
    {
        // Arrange
        var configuration = new ConfigurationBuilder()
                            .AddInMemoryCollection( new Dictionary<string, string?>
                            {
                                { "key1", "value1" }
                            } ).Build();

        var name = "key2";
        var section = configuration;
        var value = section.GetValue<string>( name );

        // Act
        var act = () => ConfigurationException.ThrowIfNullOrWhiteSpace( value, name, section );

        // Assert
        act.Should().Throw<ConfigurationException>()
           .WithMessage( $"Configuration parameter '{name}' cannot be null or empty." );
    }

    [ Fact ]
    public void ThrowIfNullOrWhiteSpace_EmptyValue_Throws()
    {
        // Arrange
        var configuration = new ConfigurationBuilder()
                            .AddInMemoryCollection( new Dictionary<string, string?>
                            {
                                { "key1", "" }
                            } ).Build();

        var name = "key1";
        var section = configuration;
        var value = section.GetValue<string>( name );

        // Act
        var act = () => ConfigurationException.ThrowIfNullOrWhiteSpace( value, name, section );

        // Assert
        act.Should().Throw<ConfigurationException>()
           .WithMessage( $"Configuration parameter '{name}' cannot be null or empty." );
    }

    [ Fact ]
    public void ThrowIfNullOrWhiteSpace_NullValueWithSection_Throws()
    {
        // Arrange
        var configuration = new ConfigurationBuilder()
                            .AddInMemoryCollection( new Dictionary<string, string?>
                            {
                                { "section:key1", "value1" }
                            } ).Build();

        var name = "key2";
        var section = configuration.GetSection( "section" );
        var value = section.GetValue<string>( name );

        // Act
        var act = () => ConfigurationException.ThrowIfNullOrWhiteSpace( value, name, section );

        // Assert
        act.Should().Throw<ConfigurationException>()
           .WithMessage( $"Configuration parameter 'section:{name}' cannot be null or empty." );
    }

    [ Fact ]
    public void ThrowIfNullOrWhiteSpace_EmptyValueCustomMessage_Throws()
    {
        // Arrange
        var configuration = new ConfigurationBuilder()
                            .AddInMemoryCollection( new Dictionary<string, string?>
                            {
                                { "section:key1", "value1" }
                            } ).Build();

        var name = "key2";
        var section = configuration.GetSection( "section" );
        var value = section.GetValue<string>( name );

        // Act
        var act = () => ConfigurationException.ThrowIfNullOrWhiteSpace( value, $"The configuration {name} cannot be empty" );

        // Assert
        act.Should().Throw<ConfigurationException>()
           .WithMessage( $"The configuration {name} cannot be empty" );
    }

    [ Fact ]
    public void ThrowIfNullOrWhiteSpace_WhiteSpaceValue_Throws()
    {
        // Arrange
        var configuration = new ConfigurationBuilder()
                            .AddInMemoryCollection( new Dictionary<string, string?>
                            {
                                { "section:key1", " " }
                            } ).Build();

        var name = "key1";
        var section = configuration.GetSection( "section" );
        var value = section.GetValue<string>( name );

        // Act
        var act = () => ConfigurationException.ThrowIfNullOrWhiteSpace( value, name, section );

        // Assert
        act.Should().Throw<ConfigurationException>()
           .WithMessage( $"Configuration parameter 'section:{name}' cannot be null or empty." );
    }

    [ Fact ]
    public void ThrowIfNullOrWhiteSpace_NonNullValue_DoesNotThrow()
    {
        // Arrange
        var configuration = new ConfigurationBuilder()
                            .AddInMemoryCollection( new Dictionary<string, string?>
                            {
                                { "section:key1", "value1" }
                            } ).Build();

        var name = "key1";
        var section = configuration.GetSection( "section" );
        var value = section.GetValue<string>( name );

        // Act
        var act = () => ConfigurationException.ThrowIfNullOrWhiteSpace( value, name, section );

        // Assert
        act.Should().NotThrow();
    }
    #endregion
}