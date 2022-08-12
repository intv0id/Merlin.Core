using Merlin.Planner.Planning;

namespace Merlin.Planner.Constraint
{
    public interface IConstraint
    {
        Task<bool> VerifyAsync(IList<Employee> employees, Schedule schedule);
    }
}