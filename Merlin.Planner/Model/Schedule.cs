using Merlin.Planner.Helpers;

namespace Merlin.Planner.Model;

/// <summary>
/// Class representing a schedule of time slot assignments.
/// </summary>
public class Schedule
{
    /// <summary>
    /// Initializes a new instance of the <see cref="Schedule"/> class.
    /// </summary>
    /// <param name="startDate">The start date.</param>
    /// <param name="endDate">The end date.</param>
    public Schedule(DateTime startDate, DateTime endDate)
    {
        this.Slots = new List<Assignment>();
        foreach (var day in DateHelper.DateRange(startDate, endDate))
        {
            this.Slots.Add(new Assignment(date: day));
        }
    }

    /// <summary>
    /// Gets the list of slot assignments.
    /// </summary>
    public List<Assignment> Slots { get; }
}
