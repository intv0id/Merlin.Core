using Merlin.Planner.Planning;

namespace Merlin.Planner.Constraint
{
    public class BankHolidayParityConstraint : IConstraint
    {
        public Task<bool> VerifyAsync(IList<Employee> employees, Schedule schedule)
        {
            throw new NotImplementedException();
        }
    }
}
