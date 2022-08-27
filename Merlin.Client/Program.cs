using Merlin.Client;
using Merlin.Planner;
using Merlin.Planner.Constraint;
using Merlin.Planner.Helpers;
using Merlin.Planner.Model;

// Given
var alice = new Employee(name: "Alice");
var bob = new Employee(name: "Bob");
var edouard = new Employee(name: "Edouard");
var someCat = new Employee(name: "Some cat");
var paul = new Employee(name: "Paul");
var marie = new Employee(name: "Marie");

var bobDayOff = new DateTime(year: 2020, month: 5, day: 3);
var bobDayOffVacation = new VacationConstraint(
    employee: bob,
    vacationStart: bobDayOff);

var aliceVacationStartDate = new DateTime(year: 2020, month: 5, day: 1);
var aliceVacationEndDate = new DateTime(year: 2020, month: 5, day: 15);
var aliceVacation = new VacationConstraint(
    employee: alice,
    vacationStart: aliceVacationStartDate,
    vacationEnd: aliceVacationEndDate);

var solver = SolverHelpers.GetSolver(MerlinSolversEnum.ORTools);
var planner = new Planner(solver);

planner.SetDates(
    new DateTime(year: 2020, month: 5, day: 1),
    new DateTime(year: 2020, month: 5, day: 15));
planner.AddEmployee(alice);
planner.AddEmployee(bob);
planner.AddEmployee(edouard);
planner.AddEmployee(someCat);
planner.AddEmployee(paul);
planner.AddEmployee(marie);
planner.AddConstraint(bobDayOffVacation);
planner.AddConstraint(aliceVacation);
planner.AddConstraint(new NotConsecutiveDaysConstraint());
planner.AddConstraint(new DaysParityConstraint(CustomDaysGrouping.GetDayGrouping()));

// When
planner.Start();

while (!planner.GetPlanning().ComputeStatus.IsFinal())
{
    await Task.Delay(200);
}

// Then
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
