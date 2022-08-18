using Merlin.Planner.Model;

namespace Merlin.Planner.Constraint;

/// <summary>
/// Employees should not be on-call two consecutive days.
/// </summary>
public class NotConsecutiveDaysConstraint : IConstraint
{
    /// <inheritdoc/>
    public async Task<bool> VerifyAsync(
        IList<Employee> employees,
        Schedule schedule)
    {
        await Task.Yield();
        foreach (var employee in employees)
        {
            var employeeAssignments = schedule.Slots.Where(a => a.Employee == employee);
            foreach (var assignment in employeeAssignments)
            {
                if (employeeAssignments.Any(a => a.Date == assignment.Date.AddDays(1).Date))
                {
                    return false;
                }
            }
        }

        return true;
    }

    /// <inheritdoc/>
    public async Task<ISet<Assignment>> GetConflictingSlots(
        IList<Employee> employees,
        Schedule schedule)
    {
        await Task.Yield();

        var conflitingSlots = new HashSet<Assignment>();

        foreach (var employee in employees)
        {
            var employeeAssignments = schedule.Slots.Where(a => a.Employee == employee);
            foreach (var assignment in employeeAssignments)
            {
                var cSlots = employeeAssignments.Where(
                    a => a.Date == assignment.Date.AddDays(1).Date);

                foreach (var cSlot in cSlots)
                {
                    conflitingSlots.Add(cSlot);
                }

                if (cSlots.Any())
                {
                    conflitingSlots.Add(assignment);
                }
            }
        }

        return conflitingSlots;
    }
}
