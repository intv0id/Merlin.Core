using Merlin.Planner.Planning;

namespace Merlin.Planner.Helpers
{
    public static class PlanningComputeStatusHelper
    {
        private static readonly HashSet<PlanningComputeStatus> FinalStatuses = new HashSet<PlanningComputeStatus>()
        { 
            PlanningComputeStatus.Succeeded,
            PlanningComputeStatus.Failed,
        };

        public static bool IsFinal(this PlanningComputeStatus status)
        {
            return FinalStatuses.Contains(status);
        }
    }
}
