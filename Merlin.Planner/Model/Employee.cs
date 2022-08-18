namespace Merlin.Planner.Model;

/// <summary>
/// Class representing an employee.
/// </summary>
public class Employee
{
    /// <summary>
    /// Initializes a new instance of the <see cref="Employee"/> class.
    /// </summary>
    /// <param name="name">The employee name.</param>
    public Employee(string name)
    {
        this.Name = name;
    }

    /// <summary>
    /// Gets the employee name.
    /// </summary>
    public string Name { get; }
}
