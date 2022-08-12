using Merlin.Planner.Constraint;
using Merlin.Planner.Planning;

namespace Merlin.Planner.Engines
{
    public class GreedyEngine : IEngine
    {
        public async Task ComputeAsync(
            IList<IConstraint> constraints, 
            IList<Employee> employees, 
            Schedule schedule)
        {
            await Task.Yield();
            for (int i = 0; i < schedule.Slots.Count; i++)
            {
                schedule.Slots[i].Employee = employees[i % employees.Count];
            }
        }
    }
}
