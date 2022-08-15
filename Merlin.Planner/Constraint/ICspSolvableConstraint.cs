using Merlin.Planner.Planning;
using static Merlin.Planner.Engines.ConstraintProgrammingEngine;
using ICspConstraint = Decider.Csp.BaseTypes.IConstraint;

namespace Merlin.Planner.Constraint
{
    public interface ICspSolvableConstraint : IConstraint
    {
        public IList<ICspConstraint> ToCspConstraints(
            IList<CspAssignment> cspSlots,
            IList<Employee> employees);
    }
}
