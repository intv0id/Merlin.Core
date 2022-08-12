using Merlin.Planner;
using Merlin.Planner.Constraint;
using Merlin.Planner.Helpers;
using Merlin.Planner.Planning;

// Given
var alice = new Employee(name: "Alice");
var bob = new Employee(name: "Bob");
var zdouard = new Employee(name: "Zdouard");

var bobDayOff = new DateTime(year: 2020, month: 5, day: 3);
var bobDayOffVacation = new VacationConstraint(
    employee: bob,
    vacationStart: bobDayOff);

var aliceVacationStartDate = new DateTime(year: 2020, month: 5, day: 25);
var aliceVacationEndDate = new DateTime(year: 2020, month: 5, day: 30);
var aliceVacation = new VacationConstraint(
    employee: alice,
    vacationStart: aliceVacationStartDate,
    vacationEnd: aliceVacationEndDate);

var planner = new Planner();

planner.SetDates(
    new DateTime(year: 2020, month: 5, day: 1),
    new DateTime(year: 2020, month: 5, day: 30));
planner.AddEmployee(alice);
planner.AddEmployee(bob);
planner.AddEmployee(zdouard);
planner.AddConstraint(bobDayOffVacation);
planner.AddConstraint(aliceVacation);

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
    Console.WriteLine($"{assignment.Date.ToString("yyyy/MM/dd")} - {assignment.Employee?.Name}");
}
