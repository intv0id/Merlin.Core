using Merlin.Planner.Model;

namespace Merlin.Planner.Constraint;

/// <summary>
/// Defines some class of days for which all the employees should have the same number of assignments.
/// </summary>
public class DaysParityConstraint : IConstraint
{
    /// <summary>
    /// Initializes a new instance of the <see cref="DaysParityConstraint"/> class.
    /// </summary>
    /// <param name="groupsDefinition">The group definition function that returns whether the specified element is part of the group.</param>
    public DaysParityConstraint(
        IList<Func<DateTime, bool>> groupsDefinition)
    {
        this.GroupsDefinition = groupsDefinition;
    }

    /// <summary>
    /// Gets the groups definition function.
    /// </summary>
    public IList<Func<DateTime, bool>> GroupsDefinition { get; }

    /// <inheritdoc/>
    public Task<ISet<Assignment>> GetConflictingSlots(
        IList<Employee> employees,
        Schedule schedule)
    {
        throw new NotImplementedException();
    }

    /// <inheritdoc/>
    public async Task<bool> VerifyAsync(
        IList<Employee> employees,
        Schedule schedule)
    {
        await Task.Yield();

        foreach (var groupDefinition in this.GroupsDefinition)
        {
            var employeeAssignmentsCount = new Dictionary<Employee, int>();
            var assignmentsInGroupByEmployee = schedule.Slots
                .Where(s => groupDefinition(s.Date))
                .GroupBy(s => s.Employee);
            foreach (var employeeAssignments in assignmentsInGroupByEmployee)
            {
                if (employeeAssignments.Key == null)
                {
                    throw new ArgumentNullException("EmployeeAssignments key cannot be null.");
                }

                employeeAssignmentsCount.Add(
                    employeeAssignments.Key,
                    employeeAssignments.Count());
            }

            if (employeeAssignmentsCount.Values.Max()
                - employeeAssignmentsCount.Values.Min() > 1)
            {
                return false;
            }
        }

        return true;
    }
}
