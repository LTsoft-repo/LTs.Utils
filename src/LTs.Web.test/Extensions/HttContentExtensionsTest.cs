using System.Collections.Immutable;
using LTs.Web.Extensions;

namespace LTs.Web.test.Extensions;

public class HttContentExtensionsTest
{
    [ Fact ]
    public async Task ReadFormAsync_WithFormUrlEncodedContent_ReturnsFields()
    {
        // Arrange
        var formContent = new FormUrlEncodedContent( new[]
        {
            new KeyValuePair<string, string>( "name", "John" ),
            new KeyValuePair<string, string>( "email", "john@example.com" ),
            new KeyValuePair<string, string>( "address", "1234 My Street Rd." )
        } );

        // Act
        var formFields = await formContent.ReadFormAsync();

        // Assert
        formFields.Should().NotBeNull();

        formFields.Should().BeEquivalentTo( new Dictionary<string, string>
        {
            { "name", "John" },
            { "email", "john@example.com" },
            { "address", "1234 My Street Rd." }
        }.ToImmutableDictionary() );
    }

    [ Fact ]
    public async Task ReadFormAsync_WithStringContent_ReturnsFields()
    {
        // Arrange
        var formData = "name=John&email=john@example.com&address=1234+My+Street+Rd.";
        var content = new StringContent( formData );

        // Act
        var formFields = await content.ReadFormAsync();

        // Assert
        formFields.Should().NotBeNull();

        formFields.Should().BeEquivalentTo( new Dictionary<string, string>
        {
            { "name", "John" },
            { "email", "john@example.com" },
            { "address", "1234 My Street Rd." }
        }.ToImmutableDictionary() );
    }

    [ Fact ]
    public async Task ReadFormAsync_EmptyContent_ReturnsEmpty()
    {
        // Arrange
        var content = new StringContent( "" );

        // Act
        var formFields = await content.ReadFormAsync();

        // Assert
        formFields.Should().BeEmpty();
    }

    [ Fact ]
    public async Task ReadFormAsync_WithNullContent_Throws()
    {
        // Arrange
        HttpContent? content = null;

        // Act
        var act = () => content.ReadFormAsync();

        // Assert
        await act.Should().ThrowAsync<ArgumentNullException>()
                 .WithMessage( "Value cannot be null. (Parameter 'content')" );
    }
}