namespace Merlin.Planner.Planning
{
    public class PlanningResult
    {
        public PlanningResult(
            PlanningComputeStatus computeStatus,
            ComputeErrorCode? errorCode,
            Schedule? schedule)
        {
            ComputeStatus = computeStatus;
            ErrorCode = errorCode;
            Schedule = schedule;
        }

        public PlanningComputeStatus ComputeStatus { get; }
        public ComputeErrorCode? ErrorCode { get; }
        public Schedule? Schedule { get; }
    }
}