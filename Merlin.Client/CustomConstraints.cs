using System.Linq;

namespace Merlin.Client
{
    public class CustomConstraints
    {
        private static readonly IList<DateTime> BankHolidays = new List<DateTime>
        {

        };

        private static IList<DateTime> BankHolidaysMinus2 => BankHolidays
            .Select(d => d.AddDays(-2).Date)
            .ToList();

        public static IList<Func<DateTime, bool>> GetDayGrouping()
        {
            var bagsOfDays = (BagOfDays[])Enum.GetValues(typeof(BagOfDays));
            return bagsOfDays
                .Select(b => new Func<DateTime, bool>(
                    d => IsInGrouping(b, d)))
                .ToList();
        }

        private static bool IsInGrouping(BagOfDays bagOfDays, DateTime day)
        {
            return (bagOfDays) switch
            {
                BagOfDays.SundaysOrBank => 
                    day.DayOfWeek == DayOfWeek.Sunday
                    || BankHolidays.Contains(day.Date),
                BagOfDays.Saturdays => day.DayOfWeek == DayOfWeek.Saturday,
                BagOfDays.Fridays => day.DayOfWeek == DayOfWeek.Friday,
                BagOfDays.ThursdaysOr2PriorBank => 
                    day.DayOfWeek == DayOfWeek.Thursday
                    || BankHolidaysMinus2.Contains(day.Date),
                BagOfDays.WeekDays => 
                    day.DayOfWeek == DayOfWeek.Monday
                    || day.DayOfWeek == DayOfWeek.Tuesday
                    || day.DayOfWeek == DayOfWeek.Wednesday,
                _ => throw new ArgumentException(
                    $"{bagOfDays.ToString("g")} is not part of bag of days implementation."),
            };
        }

        private enum BagOfDays
        {
            /// <summary>
            /// Regular Mondays, Tuesdays, Wednesdays.
            /// </summary>
            WeekDays,

            /// <summary>
            /// Thurdays, or 2 days before a bank holiday.
            /// </summary>
            ThursdaysOr2PriorBank,

            /// <summary>
            /// Fridays.
            /// </summary>
            Fridays,

            /// <summary>
            /// Saturdays.
            /// </summary>
            Saturdays,

            /// <summary>
            /// Sundays or bank holidays.
            /// </summary>
            SundaysOrBank,
        }
    }
}
