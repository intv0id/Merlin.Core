using Merlin.Planner.Engines;
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
    public class DaysParityConstraint : ICspSolvableConstraint
    {
        public Task<ISet<Assignment>> GetConflictingSlots(IList<Employee> employees, Schedule schedule)
        {
            throw new NotImplementedException();
        }

        public IList<Decider.Csp.BaseTypes.IConstraint> ToCspConstraints(IList<ConstraintProgrammingEngine.CspAssignment> cspSlots, IList<Employee> employees)
        {
            throw new NotImplementedException();
        }

        public Task<bool> VerifyAsync(IList<Employee> employees, Schedule schedule)
        {
            throw new NotImplementedException();
        }
    }
}
