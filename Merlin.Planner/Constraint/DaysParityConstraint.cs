using Merlin.Planner.Model;

namespace Merlin.Planner.Constraint
{
    public class DaysParityConstraint : IConstraint
    {
        public IList<Func<DateTime, bool>> GroupsDefinition { get; }

        public DaysParityConstraint(
            IList<Func<DateTime, bool>> groupsDefinition)
        {
            this.GroupsDefinition = groupsDefinition;
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

            foreach (var groupDefinition in GroupsDefinition)
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
}
