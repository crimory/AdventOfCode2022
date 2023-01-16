open System
open Day12

let instructions = IO.File.ReadAllText "input.txt"

let shortestSteps = instructions |> SolutionForwards.GetShortestRouteNumberOfSteps
Console.WriteLine $"Minimum number of steps is: {shortestSteps}"

let shortestStepsToAnyLowestPoint = instructions |> SolutionForwards.GetShortestRouteNumberOfStepsToAnyLowestPoint
Console.WriteLine $"Minimum number of steps to any lowest point is: {shortestStepsToAnyLowestPoint}"