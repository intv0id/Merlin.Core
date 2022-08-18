namespace Merlin.Planner.Model;

/// <summary>
/// Class representing the result of a planning operation.
/// </summary>
public class PlanningResult
{
    ///
    /// <summary>
    /// Initializes a new instance of the <see cref="PlanningResult"/> class.
    /// </summary>
    /// <param name="computeStatus">The compute status.</param>
    /// <param name="errorCode">The error code. Null if there is none.</param>
    /// <param name="schedule">The schedule. Null if not available.</param>
    public PlanningResult(
        PlanningComputeStatus computeStatus,
        ComputeErrorCode? errorCode,
        Schedule? schedule)
    {
        this.ComputeStatus = computeStatus;
        this.ErrorCode = errorCode;
        this.Schedule = schedule;
    }

    /// <summary>
    /// Gets the compute status.
    /// </summary>
    public PlanningComputeStatus ComputeStatus { get; }

    /// <summary>
    /// Gets the compute error code. Null if there is none.
    /// </summary>
    public ComputeErrorCode? ErrorCode { get; }

    /// <summary>
    /// Gets the schedule if ready.
    /// </summary>
    public Schedule? Schedule { get; }
}
