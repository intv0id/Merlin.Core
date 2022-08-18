using Merlin.Planner.Model;

namespace Merlin.Planner.Helpers;

/// <summary>
/// Helper methods for <see cref="PlanningComputeStatus"/>.
/// </summary>
public static class PlanningComputeStatusHelper
{
    private static readonly HashSet<PlanningComputeStatus> FinalStatuses = new ()
    {
        PlanningComputeStatus.Succeeded,
        PlanningComputeStatus.Failed,
    };

    /// <summary>
    /// Returns whether the <see cref="PlanningComputeStatus"/> is final.
    /// </summary>
    /// <param name="status">The planning compute status.</param>
    /// <returns>A boolean indicating whether the <see cref="PlanningComputeStatus"/> is final.</returns>
    public static bool IsFinal(this PlanningComputeStatus status)
    {
        return FinalStatuses.Contains(status);
    }
}
