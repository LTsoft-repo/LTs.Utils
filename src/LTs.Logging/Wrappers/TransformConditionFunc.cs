using Serilog.Events;

namespace LTs.Logging.Wrappers;

/// <summary>
///     Function that will return whether the transformation should be applied to the log event.
/// </summary>
/// <param name="logEvent">Log event to check.</param>
/// <returns>Whether the transformation should be applied to the log event.</returns>
public delegate bool TransformConditionFunc( LogEvent logEvent );
