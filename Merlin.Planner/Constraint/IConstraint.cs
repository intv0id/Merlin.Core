using Merlin.Planner.Model;

namespace Merlin.Planner.Constraint;

/// <summary>
/// Interface for a Merlin constraint.
/// </summary>
public interface IConstraint
{
    /// <summary>
    /// Verify that the constraint has been enforced.
    /// </summary>
    /// <param name="employees">The employees list.</param>
    /// <param name="schedule">The schedule to verify.</param>
    /// <returns>Whether the constraint has been enforced.</returns>
    Task<bool> VerifyAsync(
        IList<Employee> employees,
        Schedule schedule);

    /// <summary>
    /// Returns the set of assignments that are not matching the contraint.
    /// </summary>
    /// <param name="employees">The employees list.</param>
    /// <param name="schedule">The schedule.</param>
    /// <returns>The set of assignments that don't meet the contraint.</returns>
    Task<ISet<Assignment>> GetConflictingSlots(
        IList<Employee> employees,
        Schedule schedule);
}
