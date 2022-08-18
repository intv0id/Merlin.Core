namespace Merlin.Planner.Model;

/// <summary>
/// Enum that represents the compute status.
/// </summary>
public enum PlanningComputeStatus
{
    /// <summary>
    /// Compute has not started.
    /// </summary>
    NotStarted,

    /// <summary>
    /// Compute has started but is not finished yet.
    /// </summary>
    Started,

    /// <summary>
    /// Compute resulted in an error.
    /// </summary>
    Failed,

    /// <summary>
    /// Compute has succeeded.
    /// </summary>
    Succeeded,
}
