namespace LTs.Web.Mime;

/// <summary>
///     Media Type enumeration.
/// </summary>
public enum MediaType
{
    /// <summary>
    ///     No media type.
    /// </summary>
    None = 0,

    /// <summary>
    ///     Not supported media type.
    /// </summary>
    NotSupported = -1,

    #region Text (100_00)
    /// <summary>
    ///     Plain text media type.
    /// </summary>
    TextPlain = 100_00,

    /// <summary>
    ///     HTML media type.
    /// </summary>
    TextHtml,

    /// <summary>
    ///     XML media type.
    /// </summary>
    TextXml,

    /// <summary>
    ///     Rich text media type.
    /// </summary>
    TextRichText,

    /// <summary>
    ///     CSV media type.
    /// </summary>
    TextCsv,
    #endregion

    #region Application (110_00)
    /// <summary>
    ///     SOAP media type.
    /// </summary>
    ApplicationSoap = 110_00,

    /// <summary>
    ///     Octet media type.
    /// </summary>
    ApplicationOctet,

    /// <summary>
    ///     RTF media type.
    /// </summary>
    ApplicationRtf,

    /// <summary>
    ///     PDF media type.
    /// </summary>
    ApplicationPdf,

    /// <summary>
    ///     ZIP media type.
    /// </summary>
    ApplicationZip,

    /// <summary>
    ///     JSON media type.
    /// </summary>
    ApplicationJson,

    /// <summary>
    ///     XML media type.
    /// </summary>
    ApplicationXml,

    /// <summary>
    ///     XLS media type.
    /// </summary>
    ApplicationXls,

    /// <summary>
    ///     XLSX media type.
    /// </summary>
    ApplicationXlsx,
    #endregion

    #region Image (120_00)
    /// <summary>
    ///     GIF image media type.
    /// </summary>
    ImageGif = 120_00,

    /// <summary>
    ///     TIFF image media type.
    /// </summary>
    ImageTiff,

    /// <summary>
    ///     JPEG image media type.
    /// </summary>
    ImageJpeg,
    #endregion

    #region Multiparts-form (130_00)
    /// <summary>
    ///     Multiparts mixed media type.
    /// </summary>
    MultipartsMixed = 130_00,

    /// <summary>
    ///     Multiparts alternative media type.
    /// </summary>
    MultipartsAlternative,

    /// <summary>
    ///     Multiparts related media type.
    /// </summary>
    MultipartsRelated,

    /// <summary>
    ///     Multiparts form data media type.
    /// </summary>
    MultipartsFormData
    #endregion
}