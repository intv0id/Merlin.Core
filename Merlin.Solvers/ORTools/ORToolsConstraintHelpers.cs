using Google.OrTools.Sat;
using Merlin.Planner.Constraint;

namespace Merlin.Solvers.ORTools;

/// <summary>
/// Helpers for heandling constraints in the <see cref="ORToolsSolver"/>.
/// </summary>
internal static class ORToolsConstraintHelpers
{
    /// <summary>
    /// Encode the constraints into the ORTools model.
    /// </summary>
    /// <param name="model">The ORT model.</param>
    /// <param name="ortShifts">The ort assignments variable.</param>
    /// <param name="constraints">The constraints to encode.</param>
    /// <exception cref="InvalidOperationException">The given constraint is not supported by the <see cref="ORToolsSolver"/>.</exception>
    internal static void EncodeConstraints(
        CpModel model,
        OrtShifts ortShifts,
        IList<IConstraint> constraints)
    {
        // Ensure only one employee is assigned for each slot.
        ortShifts.ForeachAssignmentIdx(a =>
        {
            List<ILiteral> slot_employees = new List<ILiteral>();

            ortShifts.ForeachEmployeeIdx(e =>
            {
                slot_employees.Add(ortShifts.GetOrtValue(
                    employeeIdx: e,
                    assignmentIdx: a));
            });

            model.AddExactlyOne(slot_employees);
        });

        foreach (var constraint in constraints)
        {
            switch (constraint)
            {
                case DaysParityConstraint dpConstraint:
                    ConvertDaysParityConstraint(
                        model,
                        ortShifts,
                        dpConstraint);
                    break;
                case NotConsecutiveDaysConstraint ncdConstraint:
                    ConvertNotConsecutiveDaysConstraint(
                        model,
                        ortShifts);
                    break;
                case VacationConstraint vConstraint:
                    ConvertVacationConstraint(
                        model,
                        ortShifts,
                        vConstraint);
                    break;
                default:
                    throw new InvalidOperationException($"{constraint.GetType()} is not solvable by {nameof(ORToolsSolver)}.");
            }
        }
    }

    private static void ConvertDaysParityConstraint(
        CpModel model,
        OrtShifts ortShifts,
        DaysParityConstraint dpConstraint)
    {
        foreach (var groupDefinition in dpConstraint.GroupsDefinition)
        {
            var bagOfSlots = ortShifts.Assignments
                .Where(a => groupDefinition(a.Date.Date));
            var employeeCount = ortShifts.GetEmployeeCount;
            int minSlotsPerEmployee = bagOfSlots.Count() / employeeCount;
            int maxSlotsPerEmployee = minSlotsPerEmployee + 1;

            ortShifts.ForeachEmployeeIdx(e =>
            {
                var employeeOrtSlots = ortShifts.GetEmployeeOrtSlots(employeeIdx: e);
                model.AddLinearConstraint(
                    expr: LinearExpr.Sum(employeeOrtSlots),
                    lb: minSlotsPerEmployee,
                    ub: maxSlotsPerEmployee);
            });
        }
    }

    private static void ConvertNotConsecutiveDaysConstraint(
        CpModel model,
        OrtShifts ortShifts)
    {
        ortShifts.ForeachAssignmentIdx(a =>
        {
            HashSet<int> nextDayASet = ortShifts.GetNextDayAssignmentsIdx(assignmentIdx: a);

            foreach (var nextDayA in nextDayASet)
            {
                ortShifts.ForeachEmployeeIdx(e =>
                {
                    model.AddBoolOr(new ILiteral[] {
                        ortShifts.GetOrtValue(employeeIdx: e, assignmentIdx: a).Not(),
                        ortShifts.GetOrtValue(employeeIdx: e, assignmentIdx: nextDayA).Not(), });
                });
            }
        });
    }

    private static void ConvertVacationConstraint(
        CpModel model,
        OrtShifts ortShifts,
        VacationConstraint vConstraint)
    {
        foreach (var slot in ortShifts.Assignments)
        {
            var assignmentIdx = ortShifts.GetAssignmentIdx(assignment: slot);

            if (slot.Date.Date >= vConstraint.VacationStartDate.Date
                && slot.Date.Date <= vConstraint.VacationEndDate.Date)
            {
                var employeeIdx = ortShifts.GetEmployeeIdx(vConstraint.Employee);

                var constraintedOrtValue = ortShifts.GetOrtValue(employeeIdx: employeeIdx, assignmentIdx: assignmentIdx);
                model.Add(constraintedOrtValue == 0);
            }
        }
    }
}