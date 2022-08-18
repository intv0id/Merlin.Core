# Merlin 🧙‍♂️
The on-call wizard.

---

This library allows to create an on-call schedule, encoding constraints.


## Usage

### 1. Describe the problem

``` csharp
// Choose the solver
var solver = new DeciderSolver();

// Instantiate the engine
var planner = new Planner(solver);

// Set planning dates
planner.SetDates(
    new DateTime(year: 2020, month: 5, day: 1),
    new DateTime(year: 2020, month: 6, day: 30));

// Add employees
var alice = new Employee(name: "Alice");
planner.AddEmployee(alice);
var bob = new Employee(name: "Bob");
planner.AddEmployee(bob);

// AddConstraints
planner.AddConstraint(new VacationConstraint(
    employee: alice,
    vacationStart: ...,
    vacationEnd: ...));
planner.AddConstraint(new NotConsecutiveDaysConstraint());
```

### 2. Start the planner and poll the results

``` csharp
planner.Start();

while (!planner.GetPlanning().ComputeStatus.IsFinal())
{
    await Task.Delay(200);
}
```

### 3. Display the results

``` csharp
var planningResult = planner.GetPlanning();

if (planningResult.ComputeStatus == PlanningComputeStatus.Failed)
{
    Console.Error.WriteLine($"Schedule compute failed with status {planningResult.ErrorCode}");
    return;
}

if (planningResult.Schedule == null)
{
    Console.Error.WriteLine($"Schedule is null");
    return;
}

foreach (var assignment in planningResult.Schedule.Slots)
{
    var assignedTo = assignment.Employee?.Name ?? "N/A";
    Console.WriteLine($"{assignment.Date.ToString("ddd yyyy/MM/dd")} - {assignedTo}");
}
```
