using Merlin.Planner.Constraint;
using Merlin.Planner.Model;
using System.ComponentModel.DataAnnotations;

namespace Merlin.Planner;

/// <summary>
/// The Merlin planner engine.
/// </summary>
public class Planner
{
    private readonly ISolver engine;

    // Inputs
    private readonly IList<IConstraint> constraints;
    private readonly IList<Employee> employees;
    private DateTime? startDate;
    private DateTime? endDate;

    // Outputs
    private PlanningComputeStatus computeStatus;
    private ComputeErrorCode? errorCode;
    private Schedule? schedule;

    /// <summary>
    /// Initializes a new instance of the <see cref="Planner"/> class.
    /// </summary>
    /// <param name="solver">The solver.</param>
    public Planner(ISolver solver)
    {
        this.constraints = new List<IConstraint>();
        this.employees = new List<Employee>();
        this.computeStatus = PlanningComputeStatus.NotStarted;

        this.engine = solver;
    }

    /// <summary>
    /// Add a constraint to the problem.
    /// </summary>
    /// <param name="constraint">The constraint.</param>
    public void AddConstraint(IConstraint constraint)
    {
        this.constraints.Add(constraint);
    }

    /// <summary>
    /// Sets the problem dates.
    /// </summary>
    /// <param name="startDate">The start date.</param>
    /// <param name="endDate">The end date.</param>
    /// <exception cref="ArgumentOutOfRangeException">The start date is after the end date.</exception>
    public void SetDates(DateTime startDate, DateTime endDate)
    {
        var endDateDay = endDate.Date;
        var startDateDay = startDate.Date;

        if (endDateDay < startDateDay)
        {
            throw new ArgumentOutOfRangeException("endDate is before startDate.");
        }

        this.startDate = startDateDay;
        this.endDate = endDateDay;
    }

    /// <summary>
    /// Add an employee to the problem.
    /// </summary>
    /// <param name="employee">The employee.</param>
    public void AddEmployee(Employee employee)
    {
        this.employees.Add(employee);
    }

    /// <summary>
    /// Starts the computation.
    /// </summary>
    /// <param name="verifyConstraints">
    /// Assert that the constraints are met at the end of the computation if set to true.
    /// false otherwise.
    /// </param>
    public void Start(bool verifyConstraints = true)
    {
        if (!this.VerifyArguments())
        {
            this.computeStatus = PlanningComputeStatus.Failed;
            this.errorCode = ComputeErrorCode.InvalidArguments;
            return;
        }

        this.computeStatus = PlanningComputeStatus.Started;
        _ = this.Compute(verifyConstraints: verifyConstraints);
    }

    /// <summary>
    /// Gets the planning result.
    /// </summary>
    /// <returns>The <see cref="PlanningResult"/>.</returns>
    public PlanningResult GetPlanning()
    {
        var schedule =
            this.computeStatus == PlanningComputeStatus.Succeeded
                ? this.schedule
                : null;
        return new PlanningResult(
            computeStatus: this.computeStatus,
            errorCode: this.errorCode,
            schedule: schedule);
    }

    private async Task Compute(bool verifyConstraints)
    {
        try
        {
            if (!this.startDate.HasValue)
            {
                this.errorCode = ComputeErrorCode.InvalidArguments;
                throw new ArgumentNullException($"{nameof(this.startDate)}");
            }

            if (!this.endDate.HasValue)
            {
                this.errorCode = ComputeErrorCode.InvalidArguments;
                throw new ArgumentNullException($"{nameof(this.endDate)}");
            }

            var schedule = new Schedule(startDate: this.startDate.Value, endDate: this.endDate.Value);

            await this.engine.ComputeAsync(
                constraints: this.constraints,
                employees: this.employees,
                schedule: schedule);

            foreach (var constraint in this.constraints)
            {
                if (verifyConstraints && !await constraint.VerifyAsync(
                        employees: this.employees,
                        schedule: schedule))
                {
                    this.errorCode = ComputeErrorCode.ConstraintNotMet;
                    throw new ValidationException();
                }
            }

            this.schedule = schedule;
            this.computeStatus = PlanningComputeStatus.Succeeded;
        }
        catch
        {
            this.computeStatus = PlanningComputeStatus.Failed;
            if (this.errorCode == null)
            {
                this.errorCode = ComputeErrorCode.Unexpected;
            }
        }
    }

    private bool VerifyArguments()
    {
        if (this.startDate == null)
        {
            return false;
        }

        if (this.endDate == null)
        {
            return false;
        }

        if (this.employees.Count == 0)
        {
            return false;
        }

        return true;
    }
}