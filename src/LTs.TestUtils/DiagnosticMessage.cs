using System.Globalization;

namespace LTs.TestUtils;

/// <summary>
///     Default implementation of <see cref="IDiagnosticMessage" />.
/// </summary>
//internal class DiagnosticMessage : LongLivedMarshalByRefObject, IDiagnosticMessage, IMessageSinkMessage
public class DiagnosticMessage : IDiagnosticMessage
{
    /// <summary>
    ///     The types of interfaces implemented by this object.
    /// </summary>
    public static HashSet<string?> InterfaceTypes { get; } = [ .. typeof( DiagnosticMessage ).GetInterfaces().Select( x => x.FullName ) ];

    /// <inheritdoc />
    public string? Message { get; set; }

    /// <inheritdoc />
    public override string? ToString() => Message;

    // ReSharper disable once InconsistentNaming

    /// <summary>
    ///     Initializes a new instance of the <see cref="DiagnosticMessage" /> class.
    /// </summary>
    public DiagnosticMessage() { }

    /// <summary>
    ///     Initializes a new instance of the <see cref="DiagnosticMessage" /> class.
    /// </summary>
    /// <param name="message">The message to send</param>
    public DiagnosticMessage( string message )
        => Message = message;

    /// <summary>
    ///     Initializes a new instance of the <see cref="DiagnosticMessage" /> class.
    /// </summary>
    /// <param name="format">The format of the message to send</param>
    /// <param name="args">The arguments used to format the message</param>
    public DiagnosticMessage( string format, params object[] args )
        => Message = string.Format( CultureInfo.CurrentCulture, format, args );
}