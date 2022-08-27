using Merlin.Client;
using Merlin.Planner.Model;
using Merlin.Solvers.Decider;
using Merlin.Solvers.Greedy;
using Merlin.Solvers.ORTools;

/// <summary>
/// Helper methods for merlin solvers.
/// </summary>
public static class SolverHelpers
{
    /// <summary>
    /// Gets an instance of the designated merlin solver.
    /// </summary>
    /// <param name="solver">The solver.</param>
    /// <returns>An instance of the solver.</returns>
    /// <exception cref="ArgumentException">The requested solver is not in the supported solvers list.</exception>
    public static ISolver GetSolver(MerlinSolversEnum solver)
    {
        return solver switch
        {
            MerlinSolversEnum.ORTools => new ORToolsSolver(),
            MerlinSolversEnum.Decider => new DeciderSolver(),
            MerlinSolversEnum.Greedy => new GreedySolver(),
            _ => throw new ArgumentException("The requested solver is not matched."),
        };
    }
}