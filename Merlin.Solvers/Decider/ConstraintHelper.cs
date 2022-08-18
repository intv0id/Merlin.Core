using Decider.Csp.Integer;
using Merlin.Planner.Constraint;
using Merlin.Planner.Model;
using IDeciderConstraint = Decider.Csp.BaseTypes.IConstraint;

namespace Merlin.Solvers.Decider;

/// <summary>
/// Decider solver constraint helper methods.
/// </summary>
public static class ConstraintHelper
{
    /// <summary>
    /// Converts a constraint to a set of Decider constraints.
    /// </summary>
    /// <param name="constraint">The constraint.</param>
    /// <param name="cspSlots">The Decider assignents.</param>
    /// <param name="employees">The employees list.</param>
    /// <returns>The list of Decider constraints.</returns>
    /// <exception cref="InvalidOperationException">The constraint is not valid.</exception>
    public static IList<IDeciderConstraint> ToCspConstraints(
        IConstraint constraint,
        IList<CspAssignment> cspSlots,
        IList<Employee> employees)
    {
        return constraint switch
        {
            DaysParityConstraint dpConstraint =>
                ConvertDaysParityConstraint(
                    dpConstraint,
                    cspSlots,
                    employees),
            NotConsecutiveDaysConstraint _ =>
                ConvertNotConsecutiveDaysConstraint(cspSlots),
            VacationConstraint vConstraint =>
                ConvertVacationConstraint(
                    vConstraint,
                    cspSlots,
                    employees),
            _ => throw new InvalidOperationException($"{constraint.GetType()} is not solvable by {nameof(DeciderSolver)}"),
        };
    }

    private static IList<IDeciderConstraint> ConvertDaysParityConstraint(
        DaysParityConstraint constraint,
        IList<CspAssignment> cspSlots,
        IList<Employee> employees)
    {
        var cspConstraints = new List<IDeciderConstraint>();

        foreach (var groupDefinition in constraint.GroupsDefinition)
        {
            var bagOfSlots = cspSlots
                .Where(c => groupDefinition(c.Assignment.Date.Date));
            int maxSlotsPerEmployee = (bagOfSlots.Count() / employees.Count) + 1;

            foreach (var employee in employees)
            {
                var employeeIdx = employees.IndexOf(employee);
                var employeeSlots = bagOfSlots
                    .Select(x => x.CspVariable == employeeIdx)
                    .Aggregate((x, y) => x + y);
                cspConstraints.Add(new ConstraintInteger(employeeSlots <= maxSlotsPerEmployee));
            }
        }

        return cspConstraints;
    }

    private static IList<IDeciderConstraint> ConvertNotConsecutiveDaysConstraint(
        IList<CspAssignment> cspSlots)
    {
        var cspConstraints = new List<IDeciderConstraint>();

        foreach (var cspSlot in cspSlots)
        {
            var nextDayAssignments = cspSlots.Where(c =>
                c.Assignment.Date.AddDays(1).Date
                == cspSlot.Assignment.Date.Date);
            foreach (var assignment in nextDayAssignments)
            {
                cspConstraints.Add(new ConstraintInteger(
                    cspSlot.CspVariable != assignment.CspVariable));
            }
        }

        return cspConstraints;
    }

    private static IList<IDeciderConstraint> ConvertVacationConstraint(
        VacationConstraint constraint,
        IList<CspAssignment> cspSlots,
        IList<Employee> employees)
    {
        var cspConstraints = new List<IDeciderConstraint>();

        foreach (var cspSlot in cspSlots)
        {
            if (cspSlot.Assignment.Date.Date >= constraint.VacationStartDate.Date
                && cspSlot.Assignment.Date.Date <= constraint.VacationEndDate.Date)
            {
                var employeeIdx = employees.IndexOf(constraint.Employee);
                cspConstraints.Add(new ConstraintInteger(
                        cspSlot.CspVariable != employeeIdx));
            }
        }

        return cspConstraints;
    }
}
