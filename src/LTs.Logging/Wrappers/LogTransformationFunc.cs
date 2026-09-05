using Serilog.Events;

namespace LTs.Logging.Wrappers;

/// <summary>
///     Function containing the transformation of a log event.
/// </summary>
/// <param name="logEvent">Log event to transform.</param>
/// <returns>The transformed log event.</returns>
public delegate LogEvent LogTransformationFunc( LogEvent logEvent );
