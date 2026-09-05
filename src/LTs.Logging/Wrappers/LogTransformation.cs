namespace LTs.Logging.Wrappers;

/// <summary>
///     Base class for log transformations.
/// </summary>
public abstract class LogTransformation : ILogTransformation
{
    /// <inheritdoc />
    public LogTransformationFunc Transform { get; init; } = logEvent => logEvent;

    /// <inheritdoc />
    public TransformConditionFunc ShouldTransform { get; init; } = _ => false;
}
