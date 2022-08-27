using Decider.Csp.Integer;
using Merlin.Planner.Constraint;
using Merlin.Planner.Model;

namespace Merlin.Solvers.Decider
{
    public class DeciderSolver : ISolver
    {
        public async Task ComputeAsync(
            IList<IConstraint> constraints,
            IList<Employee> employees,
            Schedule schedule)
        {
            await Task.Yield();

            // Model
            var numberOfEmployees = employees.Count();
            var cspSlots = schedule.Slots
                .Select(s => new CspAssignment(s, numberOfEmployees))
                .ToList();

            // Constraints
            var cspConstraints = constraints
                .Select(c => DeciderConstraintHelpers.ToCspConstraints(
                    constraint: c,
                    cspSlots: cspSlots,
                    employees: employees))
                .SelectMany(x => x)
                .ToList();

            // Solve
            var state = new StateInteger(
                variables: cspSlots.Select(x => x.CspVariable),
                constraints: cspConstraints);
            state.Search();

            foreach (var cspSlot in cspSlots)
            {
                cspSlot.Assignment.Employee
                    = employees[cspSlot.CspVariable.Value];
            }
        }
    }
}
