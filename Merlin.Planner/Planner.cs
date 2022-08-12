using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data;
using Merlin.Planner.Constraint;
using Merlin.Planner.Engines;
using Merlin.Planner.Planning;

namespace Merlin.Planner
{
    public class Planner
    {
        // Inputs
        private IList<IConstraint> constraints;
        private IList<Employee> employees;
        private DateTime? startDate;
        private DateTime? endDate;

        // Outputs
        private PlanningComputeStatus computeStatus;
        private ComputeErrorCode? errorCode;
        private Schedule? schedule;

        private readonly List<IEngine> engines;

        public Planner()
        {
            this.constraints = new List<IConstraint>();
            this.employees = new List<Employee>();
            this.computeStatus = PlanningComputeStatus.NotStarted;

            this.engines = new List<IEngine>
            {
                new GreedyEngine(),
            };
        }

        public void AddConstraint(IConstraint constraint)
        {
            this.constraints.Add(constraint);
        }

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

        public void AddEmployee(Employee employee)
        {
            this.employees.Add(employee);
        }

        public void Start()
        {
            if (!this.VerifyArguments())
            {
                this.computeStatus = PlanningComputeStatus.Failed;
                this.errorCode = ComputeErrorCode.InvalidArguments;
                return;
            }

            this.computeStatus = PlanningComputeStatus.Started;
            _ = Compute();
        }

        public PlanningResult GetPlanning()
        {
            return new PlanningResult(
                computeStatus: this.computeStatus,
                errorCode: this.errorCode,
                schedule: this.computeStatus == PlanningComputeStatus.Succeeded
                    ? this.schedule
                    : null);
        }

        private async Task Compute()
        {
            try
            {
                if (!this.startDate.HasValue || !this.endDate.HasValue)
                {
                    this.errorCode = ComputeErrorCode.InvalidArguments;
                    throw new ArgumentNullException();
                }

                var schedule = new Schedule(startDate: this.startDate.Value, endDate: this.endDate.Value);

                foreach (var engine in this.engines)
                {
                    await engine.ComputeAsync(
                        constraints: this.constraints,
                        employees: this.employees,
                        schedule: schedule);
                }

                foreach (var constraint in this.constraints)
                {
                    if (!await constraint.VerifyAsync(
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
                if (errorCode == null)
                {
                    errorCode = ComputeErrorCode.Unexpected;
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
}