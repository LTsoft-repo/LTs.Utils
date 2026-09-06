namespace LTs.Web.Mime;

/// <summary>
///     Media type name constants.
/// </summary>
public static class MediaTypeNames
{
    /// <summary>
    ///     No media type.
    /// </summary>
    public const string None = "";

    /// <summary>
    ///     Not supported media type.
    /// </summary>
    [ UsedImplicitly ] public const string NotSupported = "";

    /// <summary>
    ///     Text related media types.
    /// </summary>
    public static class Text
    {
        /// <summary>
        ///     Plain text media type.
        /// </summary>
        public const string Plain = "text/plain";

        /// <summary>
        ///     HTML media type.
        /// </summary>
        public const string Html = "text/html";

        /// <summary>
        ///     XML media type.
        /// </summary>
        public const string Xml = "text/xml";

        /// <summary>
        ///     Rich text media type.
        /// </summary>
        public const string RichText = "text/richtext";

        /// <summary>
        ///     CSV media type.
        /// </summary>
        public const string Csv = "text/csv";
    }

    /// <summary>
    ///     Application related media types.
    /// </summary>
    public static class Application
    {
        /// <summary>
        ///     SOAP media type.
        /// </summary>
        public const string Soap = "application/soap+xml";

        /// <summary>
        ///     Octet-stream media type.
        /// </summary>
        public const string Octet = "application/octet-stream";

        /// <summary>
        ///     RTF media type.
        /// </summary>
        public const string Rtf = "application/rtf";

        /// <summary>
        ///     PDF media type.
        /// </summary>
        public const string Pdf = "application/pdf";

        /// <summary>
        ///     ZIP media type.
        /// </summary>
        public const string Zip = "application/zip";

        /// <summary>
        ///     JSON media type.
        /// </summary>
        public const string Json = "application/json";

        /// <summary>
        ///     XML media type.
        /// </summary>
        public const string Xml = "application/xml";

        /// <summary>
        ///     XLS media type.
        /// </summary>
        public const string Xls = "application/vnd.ms-excel";

        /// <summary>
        ///     XLSX media type.
        /// </summary>
        public const string Xlsx = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
    }

    /// <summary>
    ///     Image related media types.
    /// </summary>
    public static class Image
    {
        /// <summary>
        ///     GIF media type.
        /// </summary>
        public const string Gif = "image/gif";

        /// <summary>
        ///     TIFF media type.
        /// </summary>
        public const string Tiff = "image/tiff";

        /// <summary>
        ///     JPEG media type.
        /// </summary>
        public const string Jpeg = "image/jpeg";
    }

    /// <summary>
    ///     Multipart-form related media types.
    /// </summary>
    public static class Multiparts
    {
        /// <summary>
        ///     Mixed media type.
        /// </summary>
        public const string Mixed = "multipart/mixed";

        /// <summary>
        ///     Alternative media type.
        /// </summary>
        public const string Alternative = "multipart/alternative";

        /// <summary>
        ///     Related media type.
        /// </summary>
        public const string Related = "multipart/related";

        /// <summary>
        ///     Form-data media type.
        /// </summary>
        public const string FormData = "multipart/form-data";
    }
}