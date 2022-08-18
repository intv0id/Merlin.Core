using Decider.Csp.Integer;
using Merlin.Planner.Model;

namespace Merlin.Solvers.Decider
{
    public class CspAssignment
    {
        public CspAssignment(
            Assignment assignment,
            int numberOfEmployee)
        {
            Assignment = assignment;
            CspVariable = new VariableInteger(
                name: assignment.Date.ToString("yyyyMMdd"),
                lowerBound: 0,
                upperBound: numberOfEmployee - 1);
        }

        public Assignment Assignment { get; }
        public VariableInteger CspVariable { get; }
    }
}
