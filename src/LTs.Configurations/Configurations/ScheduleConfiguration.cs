

// ReSharper disable once CheckNamespace
namespace LTs.Configurations;

/// <summary>
///     Configuration to schedule the execution of a task for a specific type.
/// </summary>
/// <typeparam name="T">Type the Schedule Configuration is for.</typeparam>
[ UsedImplicitly ]
public record ScheduleConfiguration<T> : ScheduleConfiguration
    where T : class;

/// <summary>
///     Configuration to schedule the execution of a task.
/// </summary>
public record ScheduleConfiguration
{
    /// <summary>
    ///     Time in milliseconds to schedule the execution.
    /// </summary>
    public required int TimeInMilliseconds { get; init; }
}