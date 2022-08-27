namespace Merlin.Client;

/// <summary>
/// Enum describing all the possible solvers for Merlin.
/// </summary>
public enum MerlinSolversEnum
{
    /// <summary>
    /// A greedy solver that matches no constraint.
    /// </summary>
    Greedy,

    /// <summary>
    /// A solver based on the Decider library.
    /// </summary>
    Decider,

    /// <summary>
    /// A solver based on the ORTools framework.
    /// </summary>
    ORTools,
}
