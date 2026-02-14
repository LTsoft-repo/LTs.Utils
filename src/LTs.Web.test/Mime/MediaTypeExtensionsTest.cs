using LTs.Web.Mime;

namespace LTs.Web.test.Mime;

public class MediaTypeExtensionsTest
{
    #region ToMediaTypeString
    [ Theory ]
    // ReSharper disable StringLiteralTypo
    [ InlineData( MediaType.ApplicationJson, "application/json" ) ]
    [ InlineData( MediaType.ApplicationXml, "application/xml" ) ]
    [ InlineData( MediaType.ApplicationZip, "application/zip" ) ]
    [ InlineData( MediaType.ApplicationPdf, "application/pdf" ) ]
    [ InlineData( MediaType.ApplicationRtf, "application/rtf" ) ]
    [ InlineData( MediaType.ApplicationOctet, "application/octet-stream" ) ]
    [ InlineData( MediaType.ApplicationSoap, "application/soap+xml" ) ]
    [ InlineData( MediaType.ApplicationXls, "application/vnd.ms-excel" ) ]
    [ InlineData( MediaType.ApplicationXlsx, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet" ) ]
    [ InlineData( MediaType.ImageGif, "image/gif" ) ]
    [ InlineData( MediaType.ImageJpeg, "image/jpeg" ) ]
    [ InlineData( MediaType.ImageTiff, "image/tiff" ) ]
    [ InlineData( MediaType.TextCsv, "text/csv" ) ]
    [ InlineData( MediaType.TextHtml, "text/html" ) ]
    [ InlineData( MediaType.TextPlain, "text/plain" ) ]
    [ InlineData( MediaType.TextRichText, "text/richtext" ) ]
    [ InlineData( MediaType.TextXml, "text/xml" ) ]
    [ InlineData( MediaType.MultipartsAlternative, "multipart/alternative" ) ]
    [ InlineData( MediaType.MultipartsFormData, "multipart/form-data" ) ]
    [ InlineData( MediaType.MultipartsMixed, "multipart/mixed" ) ]
    [ InlineData( MediaType.MultipartsRelated, "multipart/related" ) ]
    // ReSharper restore StringLiteralTypo
    public void ToMediaTypeString_WithMediaType_ReturnsMediaTypeString( MediaType mediaType, string expected )
    {
        // Arrange
        // Act
        var result = mediaType.ToMediaTypeString();

        // Assert
        result.Should().Be( expected );
    }

    [ Fact ]
    public void ToMediaTypeString_WithUnknownMediaType_ThrowsArgumentOutOfRangeException()
    {
        // Arrange
        var mediaType = (MediaType)99;

        // Act
        Action act = () => mediaType.ToMediaTypeString();

        // Assert
        act.Should().Throw<ArgumentOutOfRangeException>();
    }
    #endregion

    #region ToMediaType
    [ Theory ]
    // ReSharper disable StringLiteralTypo
    [ InlineData( "application/json", MediaType.ApplicationJson ) ]
    [ InlineData( "application/xml", MediaType.ApplicationXml ) ]
    [ InlineData( "application/zip", MediaType.ApplicationZip ) ]
    [ InlineData( "application/pdf", MediaType.ApplicationPdf ) ]
    [ InlineData( "application/rtf", MediaType.ApplicationRtf ) ]
    [ InlineData( "application/octet-stream", MediaType.ApplicationOctet ) ]
    [ InlineData( "application/soap+xml", MediaType.ApplicationSoap ) ]
    [ InlineData( "application/vnd.ms-excel", MediaType.ApplicationXls ) ]
    [ InlineData( "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", MediaType.ApplicationXlsx ) ]
    [ InlineData( "image/gif", MediaType.ImageGif ) ]
    [ InlineData( "image/jpeg", MediaType.ImageJpeg ) ]
    [ InlineData( "image/tiff", MediaType.ImageTiff ) ]
    [ InlineData( "text/csv", MediaType.TextCsv ) ]
    [ InlineData( "text/html", MediaType.TextHtml ) ]
    [ InlineData( "text/plain", MediaType.TextPlain ) ]
    [ InlineData( "text/richtext", MediaType.TextRichText ) ]
    [ InlineData( "text/xml", MediaType.TextXml ) ]
    [ InlineData( "multipart/alternative", MediaType.MultipartsAlternative ) ]
    [ InlineData( "multipart/form-data", MediaType.MultipartsFormData ) ]
    [ InlineData( "multipart/mixed", MediaType.MultipartsMixed ) ]
    [ InlineData( "multipart/related", MediaType.MultipartsRelated ) ]
    // ReSharper restore StringLiteralTypo
    public void ToMediaType_WithMediaTypeString_ReturnsMediaType( string mediaType, MediaType expected )
    {
        // Arrange
        // Act
        var result = mediaType.ToMediaType();

        // Assert
        result.Should().Be( expected );
    }

    [ Fact ]
    public void ToMediaType_WithEmptyMediaTypeString_ReturnsNone()
    {
        // Arrange
        var mediaType = string.Empty;

        // Act
        var result = mediaType.ToMediaType();

        // Assert
        result.Should().Be( MediaType.None );
    }

    [ Fact ]
    public void ToMediaType_WithNullString_ReturnsNone()
    {
        // Arrange
        string? mediaType = null;

        // Act
        var result = mediaType!.ToMediaType();

        // Assert
        result.Should().Be( MediaType.None );
    }

    [ Fact ]
    public void ToMediaType_WithUnknownMediaTypeString_ReturnsNotSupported()
    {
        // Arrange
        var mediaType = "unknown media type";

        // Act
        var result = mediaType.ToMediaType();

        // Assert
        result.Should().Be( MediaType.NotSupported );
    }
    #endregion
}