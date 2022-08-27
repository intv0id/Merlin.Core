using Google.OrTools.Sat;
using Merlin.Planner.Model;

namespace Merlin.Solvers.ORTools;

public class OrtShifts
{
    private int assignmentsCount;
    private int employeesCount;

    /// <summary>
    /// Initializes a new instance of the <see cref="OrtShifts"/> class.
    /// </summary>
    /// <param name="employees">The <see cref="Employee"/> list.</param>
    /// <param name="assignments">The <see cref="Assignment"/> list.</param>
    /// <param name="cpModel">The ORTools model.</param>
    public OrtShifts(
        IList<Employee> employees,
        IList<Assignment> assignments,
        CpModel cpModel)
    {
        this.Employees = employees;
        this.Assignments = assignments;

        this.employeesCount = employees.Count;
        this.assignmentsCount = assignments.Count;

        this.Shifts = new BoolVar[this.employeesCount, this.assignmentsCount];
        for (int e = 0; e < this.employeesCount; e++)
        {
            for (int a = 0; a < this.assignmentsCount; a++)
            {
                this.Shifts[e, a] = cpModel.NewBoolVar(string.Format("shift_e{0}a{1}", e, a));
            }
        }
    }

    /// <summary>
    /// Gets the <see cref="Employee"/> list.
    /// </summary>
    public IList<Employee> Employees { get; }

    /// <summary>
    /// Gets the <see cref="Assignment"/> list.
    /// </summary>
    public IList<Assignment> Assignments { get; }

    /// <summary>
    /// Gets the ORTools encoded schedule.
    /// </summary>
    public BoolVar[,] Shifts { get; }

    /// <summary>
    /// Gets the employee count.
    /// </summary>
    internal int GetEmployeeCount => this.employeesCount;

    /// <summary>
    /// Get the ort value for the given employee and assignment indexes.
    /// </summary>
    /// <param name="employeeIdx">The employee idx.</param>
    /// <param name="assignmentIdx">The assignment index.</param>
    /// <returns>The ort value.</returns>
    internal BoolVar GetOrtValue(int employeeIdx, int assignmentIdx)
    {
        return this.Shifts[employeeIdx, assignmentIdx];
    }

    /// <summary>
    /// Gets the employee corresponding to the provided index.
    /// </summary>
    /// <param name="employeeIdx">The employee index.</param>
    /// <returns>The employee.</returns>
    internal Employee GetEmployee(int employeeIdx)
    {
        return this.Employees[employeeIdx];
    }

    /// <summary>
    /// Gets the assignment corresponding to the provided index.
    /// </summary>
    /// <param name="assignmentIdx">The assignment index.</param>
    /// <returns>The assignment.</returns>
    internal Assignment GetAssignment(int assignmentIdx)
    {
        return this.Assignments[assignmentIdx];
    }

    /// <summary>
    /// Returns the assignment indexes for the days after the provided assignment.
    /// </summary>
    /// <param name="assignmentIdx">The assignment index.</param>
    /// <returns>The next day assignments indexes.</returns>
    internal HashSet<int> GetNextDayAssignmentsIdx(int assignmentIdx)
    {
        var assignment = this.Assignments[assignmentIdx];
        var nextDayAssignments = this.Assignments.Where(c =>
            c.Date.Date
            == assignment.Date.AddDays(1).Date);
        return nextDayAssignments
            .Select(ndAssignment => this.Assignments.IndexOf(ndAssignment))
            .ToHashSet();
    }

    /// <summary>
    /// Gets the employee index.
    /// </summary>
    /// <param name="employee">The employee.</param>
    /// <returns>The employee index.</returns>
    /// <exception cref="ArgumentException">The employee could not be found in the list.</exception>
    internal int GetEmployeeIdx(Employee employee)
    {
        var idx = this.Employees.IndexOf(employee);

        if (idx == -1)
        {
            throw new ArgumentException("The specified employee is not in the employee list");
        }

        return idx;
    }

    /// <summary>
    /// Gets the assignment index.
    /// </summary>
    /// <param name="assignment">The assignment.</param>
    /// <returns>The assignment index.</returns>
    /// <exception cref="ArgumentException">The assignment could not be found in the list.</exception>
    internal int GetAssignmentIdx(Assignment assignment)
    {
        var idx = this.Assignments.IndexOf(assignment);

        if (idx == -1)
        {
            throw new ArgumentException("The specified assignment is not in the employee list");
        }

        return idx;
    }

    /// <summary>
    /// Get the employee assignments ort representation.
    /// </summary>
    /// <param name="employeeIdx">The employee index.</param>
    /// <returns>The employee assignment in ort representation.</returns>
    internal BoolVar[] GetEmployeeOrtSlots(int employeeIdx)
    {
        BoolVar[] tmp = new BoolVar[this.assignmentsCount];

        this.ForeachAssignmentIdx(assignmentIdx => 
            tmp[assignmentIdx] = this.GetOrtValue(employeeIdx: employeeIdx, assignmentIdx: assignmentIdx));

        return tmp;
    }

    /// <summary>
    /// Do an action for each assignment index.
    /// </summary>
    /// <param name="action">The action.</param>
    internal void ForeachAssignmentIdx(Action<int> action)
    {
        for (int a = 0; a < this.assignmentsCount; a++)
        {
            action(a);
        }
    }

    /// <summary>
    /// Do an action for each employee index.
    /// </summary>
    /// <param name="action">The action.</param>
    internal void ForeachEmployeeIdx(Action<int> action)
    {
        for (int e = 0; e < this.employeesCount; e++)
        {
            action(e);
        }
    }
}
