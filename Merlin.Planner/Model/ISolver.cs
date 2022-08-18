using Merlin.Planner.Constraint;

namespace Merlin.Planner.Model;

/// <summary>
/// Interface for an assignment solver.
/// </summary>
public interface ISolver
{
    /// <summary>
    /// Computes the result in place in the <paramref name="schedule"/> object.
    /// </summary>
    /// <param name="constraints">The constraints.</param>
    /// <param name="employees">The employees list.</param>
    /// <param name="schedule">The schedule.</param>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    public Task ComputeAsync(
        IList<IConstraint> constraints,
        IList<Employee> employees,
        Schedule schedule);
}
