using Merlin.Planner.Helpers;
using Merlin.Planner.Model;

namespace Merlin.Planner.Constraint;

/// <summary>
/// Employee is on vacation.
/// </summary>
public class VacationConstraint : IConstraint
{
    /// <summary>
    /// Initializes a new instance of the <see cref="VacationConstraint"/> class.
    /// </summary>
    /// <param name="employee">The employee.</param>
    /// <param name="vacationStart">The vacation start date.</param>
    /// <param name="vacationEnd">The vacation end date. Null if same as start date.</param>
    /// <exception cref="ArgumentOutOfRangeException">The end date is before the start date.</exception>
    public VacationConstraint(
        Employee employee,
        DateTime vacationStart,
        DateTime? vacationEnd = null)
    {
        var vacationStartDate = vacationStart.Date;
        var vacationEndDate = vacationEnd?.Date ?? vacationStartDate;

        if (vacationEndDate < vacationStartDate)
        {
            throw new ArgumentOutOfRangeException("endDate is before startDate.");
        }

        this.Employee = employee;
        this.VacationStartDate = vacationStartDate;
        this.VacationEndDate = vacationEndDate;
    }

    /// <summary>
    /// Gets the employee.
    /// </summary>
    public Employee Employee { get; }

    /// <summary>
    /// Gets the vacation start date.
    /// </summary>
    public DateTime VacationStartDate { get; }

    /// <summary>
    /// Gets the vacation end date.
    /// </summary>
    public DateTime VacationEndDate { get; }

    /// <inheritdoc/>
    public async Task<bool> VerifyAsync(
        IList<Employee> employees,
        Schedule schedule)
    {
        await Task.Yield();
        foreach (var slot in schedule.Slots)
        {
            if (slot.Employee == this.Employee
                && DateHelper
                    .DateRange(this.VacationStartDate, this.VacationEndDate)
                    .Contains(slot.Date.Date))
            {
                return false;
            }
        }

        return true;
    }

    /// <inheritdoc/>
    public async Task<ISet<Assignment>> GetConflictingSlots(
        IList<Employee> employees,
        Schedule schedule)
    {
        await Task.Yield();

        var conflictingSlots = new HashSet<Assignment>();

        foreach (var slot in schedule.Slots)
        {
            if (slot.Employee == this.Employee
                && DateHelper.DateRange(this.VacationStartDate, this.VacationEndDate)
                .Contains(slot.Date.Date))
            {
                conflictingSlots.Add(slot);
            }
        }

        return conflictingSlots;
    }
}
