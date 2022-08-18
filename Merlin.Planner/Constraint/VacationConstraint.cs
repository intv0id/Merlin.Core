using Merlin.Planner.Helpers;
using Merlin.Planner.Model;

namespace Merlin.Planner.Constraint
{
    public class VacationConstraint : IConstraint
    {
        public Employee Employee { get; }
        public DateTime VacationStartDate { get; }
        public DateTime VacationEndDate { get; }

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

            Employee = employee;
            VacationStartDate = vacationStartDate;
            VacationEndDate = vacationEndDate;
        }

        public async Task<bool> VerifyAsync(
            IList<Employee> employees, 
            Schedule schedule)
        {
            await Task.Yield();
            foreach (var slot in schedule.Slots)
            {
                if ( slot.Employee == Employee
                    && DateHelper.DateRange(this.VacationStartDate, this.VacationEndDate)
                    .Contains(slot.Date.Date))
                {
                    return false;
                }
            }

            return true;
        }

        public async Task<ISet<Assignment>> GetConflictingSlots(
            IList<Employee> employees, 
            Schedule schedule)
        {
            await Task.Yield();

            var conflictingSlots = new HashSet<Assignment>();

            foreach (var slot in schedule.Slots)
            {
                if (slot.Employee == Employee
                    && DateHelper.DateRange(this.VacationStartDate, this.VacationEndDate)
                    .Contains(slot.Date.Date))
                {
                    conflictingSlots.Add(slot);
                }
            }

            return conflictingSlots;
        }
    }
}
