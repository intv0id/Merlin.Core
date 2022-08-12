using Merlin.Planner.Constraint;
using Merlin.Planner.Planning;

namespace Merlin.Planner.Engines
{
    public interface IEngine
    {
        public Task ComputeAsync(
            IList<IConstraint> constraints,
            IList<Employee> employees,
            Schedule schedule);
    }
}