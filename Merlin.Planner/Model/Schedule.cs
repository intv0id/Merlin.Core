using Merlin.Planner.Helpers;

namespace Merlin.Planner.Model
{
    public class Schedule
    {
        public List<Assignment> Slots { get; }

        public Schedule(DateTime startDate, DateTime endDate)
        {
            Slots = new List<Assignment>();
            foreach (var day in DateHelper.DateRange(startDate, endDate))
            {
                Slots.Add(new Assignment(date: day));
            }
        }
    }
}