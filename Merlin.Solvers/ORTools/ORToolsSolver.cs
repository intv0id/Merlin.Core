using Google.OrTools.Sat;
using Merlin.Planner.Constraint;
using Merlin.Planner.Model;

namespace Merlin.Solvers.ORTools;

/// <summary>
/// Solver using Google ORTools backend.
/// </summary>
public class ORToolsSolver : ISolver
{
    /// <inheritdoc/>
    public async Task ComputeAsync(
        IList<IConstraint> constraints,
        IList<Employee> employees,
        Schedule schedule)
    {
        await Task.Yield();

        var solver = new CpSolver()
        {
            StringParameters = "linearization_level:2 enumerate_all_solutions:false",
        };
        var model = new CpModel();
        var ortShifts = new OrtShifts(
            employees: employees,
            assignments: schedule.Slots,
            cpModel: model);
        var solutionObserver = new OrtSolutionObserver(
            ortShifts: ortShifts);

        ORToolsConstraintHelpers.EncodeConstraints(
            model: model,
            ortShifts: ortShifts,
            constraints: constraints);

        CpSolverStatus status = solver.Solve(model: model, cb: solutionObserver);

        if (status != CpSolverStatus.Feasible &&
            status != CpSolverStatus.Optimal)
        {
            throw new ORToolsSolverException($"Cannot find a solution. Failed with status code {status:g}");
        }
    }
}
