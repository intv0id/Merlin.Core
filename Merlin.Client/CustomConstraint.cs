using Merlin.Planner.Planning;

namespace Merlin.Planner.Constraint

{
    /// <summary>
    /// Mondays = tuesdays = wednesdays
    /// Thurdays = Bank holiday-2
    /// Fridays
    /// Saturdays
    /// Sundays = Bank holidays
    /// </summary>
    public class CustomConstraint : IConstraint
    {
        public Task<bool> VerifyAsync(IList<Employee> employees, Schedule schedule)
        {
            throw new NotImplementedException();
        }
    }
}
