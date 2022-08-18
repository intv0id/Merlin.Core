namespace Merlin.Planner.Model
{
    public class Assignment
    {
        public DateTime Date { get; }

        public Employee? Employee { get; set; }

        public Assignment(DateTime date)
        {
            Date = date;
        }
    }
}