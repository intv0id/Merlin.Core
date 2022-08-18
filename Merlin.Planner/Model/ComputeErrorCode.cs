namespace Merlin.Planner.Model;

/// <summary>
/// Enum reprsenting the error code hat may occur during compute time.
/// </summary>
public enum ComputeErrorCode
{
    /// <summary>
    /// Input arguments are invalid.
    /// </summary>
    InvalidArguments,

    /// <summary>
    /// Unexpected error.
    /// </summary>
    Unexpected,

    /// <summary>
    /// Constraints cannot be met by the solver.
    /// </summary>
    ConstraintNotMet,
}
