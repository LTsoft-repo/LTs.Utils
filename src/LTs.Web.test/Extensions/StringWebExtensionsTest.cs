using LTs.Web.Extensions;

namespace LTs.Web.test.Extensions;

public class StringWebExtensionsTest
{
    #region AddQueryString
    [ Theory ]
    [ InlineData( "", "key", "value", "?key=value" ) ]
    [ InlineData( "http://a.com/b", "key", "value", "http://a.com/b?key=value" ) ]
    [ InlineData( "http://a.com/b?key1=1", "key2", "value", "http://a.com/b?key1=1&key2=value" ) ]
    [ InlineData( "http://a.com/b?key1=1&anotherKey=2", "key2", "value", "http://a.com/b?key1=1&anotherKey=2&key2=value" ) ]
    [ InlineData( "http://a.com/b", "key", "", "http://a.com/b?key=" ) ]
    [ InlineData( "http://a.com/b", "key", "value with space", "http://a.com/b?key=value%20with%20space" ) ]
    [ InlineData( "http://a.com/b", "key.1", "value with space", "http://a.com/b?key.1=value%20with%20space" ) ]
    [ InlineData( "http://a.com/b", "key", " ", "http://a.com/b?key=%20" ) ]
    public void AddQueryString_AddsSuccessfully( string uri, string key, string value, string expected )
    {
        // Act
        var result = uri.AddQueryString( key, value );

        // Assert
        result.Should().Be( expected );
    }

    [ Theory ]
    [ InlineData( null, "key", "value", "uri" ) ]
    [ InlineData( "uri", null, "value", "key" ) ]
    [ InlineData( "uri", "key", null, "value" ) ]
    public void AddQueryString_NullParameter_ThrowsArgumentNullException( string? uri, string? key, string? value, string errorParameter )
    {
        // Act
        var act = () => uri!.AddQueryString( key!, value! );

        // Assert
        act.Should().Throw<ArgumentNullException>()
           .WithMessage( $"Value cannot be null. (Parameter '{errorParameter}')" );
    }

    [ Theory ]
    [ InlineData( "uri", "", "value", "key" ) ]
    public void AddQueryString_EmptyValue_ThrowsArgumentException( string uri, string key, string value, string errorParameter )
    {
        // Act
        var act = () => uri.AddQueryString( key, value );

        // Assert
        act.Should().Throw<ArgumentException>()
           .WithMessage( $"Value cannot be empty. (Parameter '{errorParameter}')" );
    }
    #endregion
}