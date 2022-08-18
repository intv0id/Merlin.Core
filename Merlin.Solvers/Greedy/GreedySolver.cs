namespace Merlin.Solvers.Greedy
{
    using Merlin.Planner.Constraint;
    using Merlin.Planner.Model;

    /// <summary>
    /// A greedy solver that assigns alternatively slots to employees.
    /// </summary>
    public class GreedySolver : ISolver
    {
        /// <inheritdoc/>
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
