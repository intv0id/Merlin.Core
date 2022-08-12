using Merlin.Planner.Planning;

namespace Merlin.Planner.Constraint
{
    public class NotConsecutiveDaysConstraint : IConstraint
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
    }
}
