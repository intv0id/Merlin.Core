namespace Merlin.Solvers.ORTools;

/// <summary>
/// <see cref="ORToolsSolver"/> exception.
/// </summary>
[Serializable]
public class ORToolsSolverException : Exception
{
    /// <summary>
    /// Initializes a new instance of the <see cref="ORToolsSolverException"/> class.
    /// </summary>
    /// <param name="message">The error message.</param>
    public ORToolsSolverException(string? message)
        : base(message)
    {
    }
}