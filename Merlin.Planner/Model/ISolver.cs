using Merlin.Planner.Constraint;

namespace Merlin.Planner.Model
{
    public interface ISolver
    {
        public Task ComputeAsync(
            IList<IConstraint> constraints,
            IList<Employee> employees,
            Schedule schedule);
    }
}