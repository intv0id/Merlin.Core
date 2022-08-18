namespace Merlin.Planner.Model;

/// <summary>
/// Class representing an employee assignment to a time slot.
/// </summary>
public class Assignment
{
    /// <summary>
    /// Initializes a new instance of the <see cref="Assignment"/> class.
    /// </summary>
    /// <param name="date">The date of the time slot (1 day).</param>
    public Assignment(DateTime date)
    {
        this.Date = date;
    }

    /// <summary>
    /// Gets the assignment date.
    /// </summary>
    public DateTime Date { get; }

    /// <summary>
    /// Gets or sets the assignment employee.
    /// </summary>
    public Employee? Employee { get; set; }
}
