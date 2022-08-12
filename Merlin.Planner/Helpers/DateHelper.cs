namespace Merlin.Planner.Helpers
{
    public static class DateHelper
    {
        public static IEnumerable<DateTime> DateRange(DateTime dateTime, DateTime endTime)
        {
            TimeSpan difference = (endTime - dateTime);
            for (int i = 0; i <= difference.Days; i++)
            {
                yield return dateTime.AddDays(i).Date;
            }
        }
    }
}
