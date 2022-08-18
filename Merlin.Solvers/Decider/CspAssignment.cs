using Decider.Csp.Integer;
using Merlin.Planner.Model;

namespace Merlin.Solvers.Decider;

/// <summary>
/// Decider slot assignment.
/// </summary>
public class CspAssignment
{
    /// <summary>
    /// Initializes a new instance of the <see cref="CspAssignment"/> class.
    /// </summary>
    /// <param name="assignment">The Merlin slot assignment.</param>
    /// <param name="numberOfEmployees">The number of employees.</param>
    public CspAssignment(
        Assignment assignment,
        int numberOfEmployees)
    {
        this.Assignment = assignment;
        this.CspVariable = new VariableInteger(
            name: assignment.Date.ToString("yyyyMMdd"),
            lowerBound: 0,
            upperBound: numberOfEmployees - 1);
    }

    /// <summary>
    /// Gets the Merlin assignment.
    /// </summary>
    public Assignment Assignment { get; }

    /// <summary>
    /// Gets the Csp variable giving the index of the employee assigned to this slot.
    /// </summary>
    public VariableInteger CspVariable { get; }
}
