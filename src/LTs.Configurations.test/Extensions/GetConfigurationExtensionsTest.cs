using JetBrains.Annotations;
using LTs.Configurations.Exceptions;
using LTs.Configurations.Extensions;
using Microsoft.Extensions.Configuration;
using static LTs.Configurations.test.Extensions.GetConfigurationExtensionsTest.RecordA;

namespace LTs.Configurations.test.Extensions;

public class GetConfigurationExtensionsTest
{
    #region GetRequiredValue
    [ Fact ]
    public void GetRequiredValue_CorrectParameters_Successes()
    {
        // Arrange
        var configuration = new ConfigurationBuilder()
                            .AddInMemoryCollection( new Dictionary<string, string?>
                            {
                                { "setting:key1", "value1" },
                                { "setting:key2", "1" },
                                { "setting:key3", "value3" }
                            } ).Build();

        // Act
        var section = configuration.GetSection( "setting" );
        var result1 = section.GetRequiredValue<string>( "key1" );
        var result2 = section.GetRequiredValue<int>( "key2" );
        var result3 = section.GetRequiredValue<string>( "key3" );

        // Assert
        result1.Should().Be( "value1" );
        result2.Should().Be( 1 );
        result3.Should().Be( "value3" );
    }

    [ Fact ]
    public void GetRequiredValue_WithClass_Successes()
    {
        // Arrange
        var configuration = new ConfigurationBuilder()
                            .AddInMemoryCollection( new Dictionary<string, string?>
                            {
                                { "setting:key1:ValueString", "some string" },
                                { "setting:key1:ValueBool", "false" },
                                { "setting:key1:ValueRecord:ValueString", "another string" },
                                { "setting:key1:ValueRecord:ValueInt", "23" },
                                { "setting:key1:ValueRecord:ValueEnum", "3" }
                            } ).Build();

        // Act
        var section = configuration.GetSection( "setting" );
        var result = section.GetRequiredValue<RecordA>( "key1" );

        // Assert
        result.Should().BeEquivalentTo( new RecordA
        {
            ValueString = "some string",
            ValueBool = false,
            ValueRecord = new RecordB
            {
                ValueString = "another string",
                ValueInt = 23,
                ValueEnum = Enum1.Value3
            }
        } );
    }

    [ Fact ]
    public void GetRequiredValue_WithClassWrongTypeValue_Throws()
    {
        // Arrange
        var configuration = new ConfigurationBuilder()
                            .AddInMemoryCollection( new Dictionary<string, string?>
                            {
                                { "setting:key1:ValueString", "some string" },
                                { "setting:key1:ValueBool", "false" },
                                { "setting:key1:ValueRecord:ValueString", "another string" },
                                { "setting:key1:ValueRecord:ValueInt", "23" },
                                { "setting:key1:ValueRecord:ValueEnum", "invalid" }
                            } ).Build();

        // Act
        var section = configuration.GetSection( "setting" );
        var act = () => section.GetRequiredValue<RecordA>( "key1" );

        // Assert
        act.Should().Throw<ConfigurationException>()
           .WithMessage(
               "Configuration parameter 'key1' is not of type 'RecordA'.\n" +
               "Exception message: Failed to convert configuration value at 'setting:key1:ValueRecord:ValueEnum' to type 'LTs.Configurations.test.Extensions.GetConfigurationExtensionsTest+Enum1'.\n" +
               "Section content: \n" +
               "\tsetting:key1:ValueString = some string\r\n" +
               "\tsetting:key1:ValueRecord:ValueString = another string\r\n" +
               "\tsetting:key1:ValueRecord:ValueInt = 23\r\n" +
               "\tsetting:key1:ValueRecord:ValueEnum = invalid\r\n" +
               "\tsetting:key1:ValueBool = false" );
    }

    [ Fact ]
    public void GetRequiredValue_WithDefaultClass_Throws()
    {
        // Arrange
        var configuration = new ConfigurationBuilder()
                            .AddInMemoryCollection( new Dictionary<string, string?>
                            {
                                { "setting:key1:", "" }
                            } ).Build();

        // Act
        var section = configuration.GetSection( "setting" );
        var act = () => section.GetRequiredValue<RecordB>( "key1" );

        // Assert
        act.Should().Throw<ConfigurationException>()
           .WithMessage( "Configuration parameter 'key1' is not of type 'RecordB'*" );
    }

    [ Fact ]
    public void GetRequiredValue_WithArray_Successes()
    {
        // Arrange
        var configuration = new ConfigurationBuilder()
                            .AddInMemoryCollection( new Dictionary<string, string?>
                            {
                                { "setting:key1:0", "Value1" },
                                { "setting:key1:1", "Value2" },
                                { "setting:key1:2", "Value3" }
                            } ).Build();

        // Act
        var section = configuration.GetSection( "setting" );
        var result = section.GetRequiredValue<IEnumerable<string>>( "key1" );

        // Assert
        result.Should().BeEquivalentTo( "Value1", "Value2", "Value3" );
    }

    [ Fact ]
    public void GetRequiredValue_WithEnum_Successes()
    {
        // Arrange
        var configuration = new ConfigurationBuilder()
                            .AddInMemoryCollection( new Dictionary<string, string?>
                            {
                                { "setting:key1", "1" }
                            } ).Build();

        // Act
        var section = configuration.GetSection( "setting" );
        var result = section.GetRequiredValue<Enum1>( "key1" );

        // Assert
        result.Should().Be( Enum1.Value1 );
    }

    [ Fact ]
    public void GetRequiredValue_WithArrayOfClasses_Successes()
    {
        // Arrange
        var configuration = new ConfigurationBuilder()
                            .AddInMemoryCollection( new Dictionary<string, string?>
                            {
                                { "setting:key1:0:ValueString", "string 1" },
                                { "setting:key1:0:ValueInt", "23" },

                                { "setting:key1:1:ValueString", "string 2" },
                                { "setting:key1:1:ValueInt", "11" },

                                { "setting:key1:2:ValueString", "string 3" },
                                { "setting:key1:2:ValueInt", "19" }
                            } ).Build();

        // Act
        var section = configuration.GetSection( "setting" );
        var result = section.GetRequiredValue<IEnumerable<RecordB>>( "key1" );

        // Assert
        result.Should().BeEquivalentTo( new RecordB[]
        {
            new()
            {
                ValueString = "string 1",
                ValueInt = 23
            },
            new()
            {
                ValueString = "string 2",
                ValueInt = 11
            },
            new()
            {
                ValueString = "string 3",
                ValueInt = 19
            }
        } );
    }

    [ Fact ]
    public void GetRequiredValue_EmptyStringValue_Successes()
    {
        // Arrange
        var configuration = new ConfigurationBuilder()
                            .AddInMemoryCollection( new Dictionary<string, string?>
                            {
                                { "setting:key1", "" }
                            } ).Build();

        // Act
        var section = configuration.GetSection( "setting" );
        var result = section.GetRequiredValue<string>( "key1" );

        // Assert
        result.Should().Be( string.Empty );
    }

    [ Fact ]
    public void GetRequiredValue_MissingIntValue_Throws()
    {
        // Arrange
        var configuration = new ConfigurationBuilder()
                            .AddInMemoryCollection( new Dictionary<string, string?>
                            {
                                { "setting:key1", "value1" }
                            } ).Build();

        // Act
        var section = configuration.GetSection( "setting" );
        var act = new Action( () => section.GetRequiredValue<int>( "key2" ) );

        // Assert
        act.Should().Throw<ConfigurationException>()
           .WithMessage( "Configuration parameter 'setting:key2' not defined." );
    }

    [ Fact ]
    public void GetRequiredValue_MissingStringValue_Throws()
    {
        // Arrange
        var configuration = new ConfigurationBuilder()
                            .AddInMemoryCollection( new Dictionary<string, string?>
                            {
                                { "setting:key1", "value1" }
                            } ).Build();

        // Act
        var section = configuration.GetSection( "setting" );
        var act = new Action( () => section.GetRequiredValue<int>( "key2" ) );

        // Assert
        act.Should().Throw<ConfigurationException>()
           .WithMessage( "Configuration parameter 'setting:key2' not defined." );
    }

    [ Fact ]
    public void GetRequiredValue_MissingClassValue_Throws()
    {
        // Arrange
        var configuration = new ConfigurationBuilder()
                            .AddInMemoryCollection( new Dictionary<string, string?>
                            {
                                { "setting:key1", "value1" }
                            } ).Build();

        // Act
        var section = configuration.GetSection( "setting" );
        var act = new Action( () => section.GetRequiredValue<RecordB>( "key2" ) );

        // Assert
        act.Should().Throw<ConfigurationException>()
           .WithMessage( "Configuration parameter 'setting:key2' not defined." );
    }

    [ Fact ]
    public void GetRequiredValue_EmptyArrayValue_Throws()
    {
        // Arrange
        var configuration = new ConfigurationBuilder()
                            .AddInMemoryCollection( new Dictionary<string, string?>
                            {
                                { "setting:key1", "" }
                            } ).Build();

        // Act
        var section = configuration.GetSection( "setting" );
        var act = () => section.GetRequiredValue<string[]>( "key1" );

        // Assert
        act.Should().Throw<ConfigurationException>()
           .WithMessage( "Configuration parameter 'key1' is not of type 'String[]'*" );
    }

    [ Fact ]
    public void GetRequiredValue_WrongBool_Throws()
    {
        // Arrange
        var configuration = new ConfigurationBuilder()
                            .AddInMemoryCollection( new Dictionary<string, string?>
                            {
                                { "setting:key1", "" }
                            } ).Build();

        // Act
        var section = configuration.GetSection( "setting" );
        var act = () => section.GetRequiredValue<bool>( "key1" );

        // Assert
        act.Should().Throw<InvalidOperationException>()
           .WithMessage( "Failed to convert configuration value at 'setting:key1' to type 'System.Boolean'." );
    }
    #endregion

    #region GetRequiredConnectionString
    [ Fact ]
    public void GetRequiredConnectionString_ValidParameters_Successes()
    {
        // Arrange
        var configuration = new ConfigurationBuilder()
                            .AddInMemoryCollection( new Dictionary<string, string?>
                            {
                                { "ConnectionStrings:Connection1", "SomeConnectionString" }
                            } )
                            .Build();

        // Act
        var result = configuration.GetRequiredConnectionString( "Connection1" );

        // Assert
        result.Should().NotBeNull();
        result.Should().Be( "SomeConnectionString" );
    }

    [ Fact ]
    public void GetRequiredConnectionString_WithoutConfiguration_Throws()
    {
        // Arrange
        var configuration = new ConfigurationBuilder()
                            .AddInMemoryCollection( new Dictionary<string, string?>
                            {
                                { "ConnectionStrings:Connection1", "SomeConnectionString" }
                            } )
                            .Build();

        // Act
        var act = () => configuration.GetRequiredConnectionString( "Connection2" );

        // Assert
        act.Should().Throw<ConfigurationException>()
           .WithMessage( "Connection String 'Connection2' not defined." );
    }

    [ Fact ]
    public void GetRequiredConnectionString_EmptyValue_Throws()
    {
        // Arrange
        var configuration = new ConfigurationBuilder()
                            .AddInMemoryCollection( new Dictionary<string, string?>
                            {
                                { "ConnectionStrings:Connection1", "" }
                            } )
                            .Build();

        // Act
        var act = () => configuration.GetRequiredConnectionString( "Connection1" );

        // Assert
        act.Should().Throw<ConfigurationException>()
           .WithMessage( "Connection String 'Connection1' not defined." );
    }

    [ Fact ]
    public void GetRequiredConnectionString_WhiteSpacedValue_Throws()
    {
        // Arrange
        var configuration = new ConfigurationBuilder()
                            .AddInMemoryCollection( new Dictionary<string, string?>
                            {
                                { "ConnectionStrings:Connection1", " " }
                            } )
                            .Build();

        // Act
        var act = () => configuration.GetRequiredConnectionString( "Connection1" );

        // Assert
        act.Should().Throw<ConfigurationException>()
           .WithMessage( "Connection String 'Connection1' not defined." );
    }
    #endregion

    #region GetSectionPath
    [ Fact ]
    public void GetSectionPath_WithSection_Successes()
    {
        // Arrange
        var configuration = new ConfigurationBuilder()
                            .AddInMemoryCollection( new Dictionary<string, string?>
                            {
                                { "setting:key1", "value1" }
                            } ).Build();

        // Act
        var section = configuration.GetSection( "setting" );
        var result = section.GetSectionPath( "key1" );

        // Assert
        result.Should().Be( "setting:key1" );
    }

    [ Fact ]
    public void GetSectionPath_WithoutSection_Successes()
    {
        // Arrange
        var configuration = new ConfigurationBuilder()
                            .AddInMemoryCollection( new Dictionary<string, string?>
                            {
                                { "key1", "value1" }
                            } ).Build();

        // Act
        var section = configuration;
        var result = section.GetSectionPath( "key1" );

        // Assert
        result.Should().Be( "key1" );
    }
    #endregion

    internal enum Enum1
    {
        [ UsedImplicitly ] Value0,
        Value1,
        [ UsedImplicitly ] Value2,
        Value3
    }

    internal record RecordA
    {
        [ UsedImplicitly ]
        public string? ValueString { get; init; }

        [ UsedImplicitly ]
        public bool ValueBool { get; init; }

        [ UsedImplicitly ]
        public RecordB? ValueRecord { get; init; }

        internal record RecordB
        {
            [ UsedImplicitly ]
            public string? ValueString { get; init; }

            [ UsedImplicitly ]
            public int ValueInt { get; init; }

            [ UsedImplicitly ]
            public Enum1 ValueEnum { get; init; }
        }
    }
}