namespace LTs.Logging.Wrappers;

/// <summary>
///     Interface for a log transformation.
/// </summary>
public interface ILogTransformation
{
    /// <summary>
    ///     Function that will return whether the transformation should be applied to the log event.
    /// </summary>
    TransformConditionFunc ShouldTransform { get; init; }

    /// <summary>
    ///     Function containing the transformation of a log event.
    /// </summary>
    LogTransformationFunc Transform { get; init; }
}
