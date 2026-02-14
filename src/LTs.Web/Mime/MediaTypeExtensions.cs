namespace LTs.Web.Mime;

/// <summary>
///     The media type enumeration extensions.
/// </summary>
public static class MediaTypeExtensions
{
    /// <summary>
    ///     Converts the <see cref="MediaType" /> to a string representation for request Headers.
    /// </summary>
    /// <param name="mediaType">The <see cref="MediaType" /> to convert.</param>
    /// <returns>The string representation of the <see cref="MediaType" />.</returns>
    /// <exception cref="ArgumentOutOfRangeException"></exception>
    public static string ToMediaTypeString( this MediaType mediaType )
        => mediaType switch
        {
            MediaType.None => MediaTypeNames.None,

            MediaType.TextPlain => MediaTypeNames.Text.Plain,
            MediaType.TextHtml => MediaTypeNames.Text.Html,
            MediaType.TextXml => MediaTypeNames.Text.Xml,
            MediaType.TextRichText => MediaTypeNames.Text.RichText,
            MediaType.TextCsv => MediaTypeNames.Text.Csv,

            MediaType.ApplicationSoap => MediaTypeNames.Application.Soap,
            MediaType.ApplicationOctet => MediaTypeNames.Application.Octet,
            MediaType.ApplicationRtf => MediaTypeNames.Application.Rtf,
            MediaType.ApplicationPdf => MediaTypeNames.Application.Pdf,
            MediaType.ApplicationZip => MediaTypeNames.Application.Zip,
            MediaType.ApplicationJson => MediaTypeNames.Application.Json,
            MediaType.ApplicationXml => MediaTypeNames.Application.Xml,

            MediaType.ApplicationXls => MediaTypeNames.Application.Xls,
            MediaType.ApplicationXlsx => MediaTypeNames.Application.Xlsx,

            MediaType.ImageGif => MediaTypeNames.Image.Gif,
            MediaType.ImageTiff => MediaTypeNames.Image.Tiff,
            MediaType.ImageJpeg => MediaTypeNames.Image.Jpeg,

            MediaType.MultipartsMixed => MediaTypeNames.Multiparts.Mixed,
            MediaType.MultipartsAlternative => MediaTypeNames.Multiparts.Alternative,
            MediaType.MultipartsRelated => MediaTypeNames.Multiparts.Related,
            MediaType.MultipartsFormData => MediaTypeNames.Multiparts.FormData,

            _ => throw new ArgumentOutOfRangeException( nameof( mediaType ), mediaType, null )
        };

    /// <summary>
    ///     Converts the Headers string representation of a media type to a <see cref="MediaType" />.
    /// </summary>
    /// <param name="mediaType">The media type string to convert.</param>
    /// <returns>The <see cref="MediaType" /> representation of the string.</returns>
    [ UsedImplicitly ]
    public static MediaType ToMediaType( this string mediaType )
    {
        if( string.IsNullOrEmpty( mediaType ) )
        {
            return MediaType.None;
        }

        // Removes if there is anything after the media type
        var indexSemicolon = mediaType.IndexOf( ';' );

        if( indexSemicolon > 0 )
        {
            mediaType = mediaType[ ..indexSemicolon ].Trim();
        }

        return mediaType switch
        {
            MediaTypeNames.Text.Plain => MediaType.TextPlain,
            MediaTypeNames.Text.Html => MediaType.TextHtml,
            MediaTypeNames.Text.Xml => MediaType.TextXml,
            MediaTypeNames.Text.RichText => MediaType.TextRichText,
            MediaTypeNames.Text.Csv => MediaType.TextCsv,

            MediaTypeNames.Application.Soap => MediaType.ApplicationSoap,
            MediaTypeNames.Application.Octet => MediaType.ApplicationOctet,
            MediaTypeNames.Application.Rtf => MediaType.ApplicationRtf,
            MediaTypeNames.Application.Pdf => MediaType.ApplicationPdf,
            MediaTypeNames.Application.Zip => MediaType.ApplicationZip,
            MediaTypeNames.Application.Json => MediaType.ApplicationJson,
            MediaTypeNames.Application.Xml => MediaType.ApplicationXml,

            MediaTypeNames.Application.Xls => MediaType.ApplicationXls,
            MediaTypeNames.Application.Xlsx => MediaType.ApplicationXlsx,

            MediaTypeNames.Image.Gif => MediaType.ImageGif,
            MediaTypeNames.Image.Tiff => MediaType.ImageTiff,
            MediaTypeNames.Image.Jpeg => MediaType.ImageJpeg,

            MediaTypeNames.Multiparts.Mixed => MediaType.MultipartsMixed,
            MediaTypeNames.Multiparts.Alternative => MediaType.MultipartsAlternative,
            MediaTypeNames.Multiparts.Related => MediaType.MultipartsRelated,
            MediaTypeNames.Multiparts.FormData => MediaType.MultipartsFormData,

            _ => MediaType.NotSupported
        };
    }
}