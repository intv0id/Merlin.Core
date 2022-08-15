using Decider.Csp.Integer;
using Merlin.Planner.Planning;
using static Merlin.Planner.Engines.ConstraintProgrammingEngine;
using ICspConstraint = Decider.Csp.BaseTypes.IConstraint;

namespace Merlin.Planner.Constraint
{
    public class DaysParityConstraint : ICspSolvableConstraint
    {
        IList<Func<DateTime, bool>> groupsDefinition;

        public DaysParityConstraint(
            IList<Func<DateTime, bool>> groupsDefinition)
        {
            this.groupsDefinition = groupsDefinition;
        }

        public Task<ISet<Assignment>> GetConflictingSlots(
            IList<Employee> employees, 
            Schedule schedule)
        {
            throw new NotImplementedException();
        }

        public async Task<bool> VerifyAsync(
            IList<Employee> employees, 
            Schedule schedule)
        {
            await Task.Yield();

            foreach (var groupDefinition in groupsDefinition)
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

        public IList<ICspConstraint> ToCspConstraints(
            IList<CspAssignment> cspSlots, 
            IList<Employee> employees)
        {
            var cspConstraints = new List<ICspConstraint>();

            foreach (var groupDefinition in groupsDefinition)
            {
                var bagOfSlots = cspSlots
                    .Where(c => groupDefinition(c.Assignment.Date.Date));
                int minSlotsPerEmployee = bagOfSlots.Count() / employees.Count();
                int maxSlotsPerEmployee = minSlotsPerEmployee + 1;

                foreach (var employee in employees)
                {
                    var employeeIdx = employees.IndexOf(employee);
                    var employeeSlots = bagOfSlots.
                        Select(x => x.CspVariable == employeeIdx).
                        Aggregate((x, y) => x + y);
                    cspConstraints.Add(new ConstraintInteger(employeeSlots <= maxSlotsPerEmployee));
                    cspConstraints.Add(new ConstraintInteger(employeeSlots >= minSlotsPerEmployee));
                }
            }

            return cspConstraints;
        }
    }
}
