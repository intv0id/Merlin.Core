using Merlin.Planner.Helpers;

namespace Merlin.Planner.Planning
{
    public class Schedule
    {
        public List<Assignment> Slots { get; }

        public Schedule(DateTime startDate, DateTime endDate)
        {
            this.Slots = new List<Assignment>();
            foreach (var day in DateHelper.DateRange(startDate, endDate))
            {
                this.Slots.Add(new Assignment(date: day));
            }
        }
    }
}