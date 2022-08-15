using Decider.Csp.Integer;
using Merlin.Planner.Engines;
using Merlin.Planner.Planning;
using static Merlin.Planner.Engines.ConstraintProgrammingEngine;
using ICspConstraint = Decider.Csp.BaseTypes.IConstraint;

namespace Merlin.Planner.Constraint
{
    public class NotConsecutiveDaysConstraint : ICspSolvableConstraint
    {
        public async Task<bool> VerifyAsync(
            IList<Employee> employees, 
            Schedule schedule)
        {
            await Task.Yield();
            foreach (var employee in employees)
            {
                var employeeAssignments = schedule.Slots.Where(a => a.Employee == employee);
                foreach(var assignment in employeeAssignments)
                {
                    if (employeeAssignments.Any(a => a.Date == assignment.Date.AddDays(1).Date))
                    {
                        return false;
                    }
                }
            }

            return true;
        }

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

                    if (cSlots.Count() > 0)
                    {
                        conflitingSlots.Add(assignment);
                    }
                }
            }

            return conflitingSlots;
        }

        public IList<ICspConstraint> ToCspConstraints(
            IList<CspAssignment> cspSlots, 
            IList<Employee> employees)
        {
            var cspConstraints = new List<ICspConstraint>();

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
    }
}
