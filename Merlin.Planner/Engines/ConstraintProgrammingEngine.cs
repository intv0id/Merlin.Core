using Decider.Csp.Integer;
using Merlin.Planner.Constraint;
using Merlin.Planner.Planning;

namespace Merlin.Planner.Engines
{
    public class ConstraintProgrammingEngine : IEngine
    {
        public async Task ComputeAsync(
            IList<IConstraint> constraints, 
            IList<Employee> employees, 
            Schedule schedule)
        {
            await Task.Yield();

            if (constraints.Any(c => c is not ICspSolvableConstraint))
            {
                throw new ArgumentException("Some of the constraints are not solvable by the CSP solver.");
            }
            IList<ICspSolvableConstraint> cspSolvableConstraints = constraints
                .Select(c => (ICspSolvableConstraint)c)
                .ToList();

            // Model
            var numberOfEmployees = employees.Count();
            var cspSlots = schedule.Slots
                .Select(s => new CspAssignment(s, numberOfEmployees))
                .ToList();

            // Constraints
            var cspConstraints = cspSolvableConstraints
                .Select(c => c.ToCspConstraints(
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

        public class CspAssignment
        {
            public CspAssignment(
                Assignment assignment, 
                int numberOfEmployee)
            {
                Assignment = assignment;
                CspVariable = new VariableInteger(
                    name: assignment.Date.ToString("yyyyMMdd"),
                    lowerBound: 0,
                    upperBound: numberOfEmployee - 1);
            }

            public Assignment Assignment { get; }
            public VariableInteger CspVariable { get; }
        }


    }
}
