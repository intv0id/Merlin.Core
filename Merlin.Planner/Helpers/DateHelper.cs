namespace Merlin.Planner.Helpers;

/// <summary>
/// Helper methods for <see cref="DateTime"/>.
/// </summary>
public static class DateHelper
{
    /// <summary>
    /// Enumerates dates between <paramref name="startDate"/> and <paramref name="endTime"/>.
    /// </summary>
    /// <param name="startDate">The start date (included).</param>
    /// <param name="endTime">The end date (included).</param>
    /// <returns>
    /// An enumerable of <see cref="DateTime"/> in range
    /// [<paramref name="startDate"/>, <paramref name="endTime"/>].
    /// </returns>
    public static IEnumerable<DateTime> DateRange(DateTime startDate, DateTime endTime)
    {
        TimeSpan difference = endTime - startDate;
        for (int i = 0; i <= difference.Days; i++)
        {
            yield return startDate.AddDays(i).Date;
        }
    }
}
