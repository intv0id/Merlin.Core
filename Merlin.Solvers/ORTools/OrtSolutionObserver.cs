using Google.OrTools.Sat;

namespace Merlin.Solvers.ORTools;

/// <summary>
/// The ORTools solution observer.
/// </summary>
internal class OrtSolutionObserver : CpSolverSolutionCallback
{
    /// <summary>
    /// Initializes a new instance of the <see cref="OrtSolutionObserver"/> class.
    /// </summary>
    /// <param name="ortShifts">The ORTools shifts representations.</param>
    public OrtSolutionObserver(
        OrtShifts ortShifts)
    {
        this.ortShifts = ortShifts;
    }

    private readonly OrtShifts ortShifts;

    /// <inheritdoc/>
    public override void OnSolutionCallback()
    {
        this.ortShifts.ForeachAssignmentIdx(a =>
        {
            this.ortShifts.ForeachEmployeeIdx(e =>
            {
                if (this.BooleanValue(this.ortShifts.GetOrtValue(e, a)))
                {
                    var employee = this.ortShifts.GetEmployee(e);
                    var assignment = this.ortShifts.GetAssignment(a);

                    assignment.Employee = employee;
                }
            });
        });

        this.StopSearch();
    }
}
