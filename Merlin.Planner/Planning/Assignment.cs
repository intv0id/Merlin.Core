namespace Merlin.Planner.Planning
{
    public class Assignment
    {
        public DateTime Date { get; }

        public Employee? Employee { get; set; }

        public Assignment(DateTime date)
        {
            this.Date = date;
        }
    }
}