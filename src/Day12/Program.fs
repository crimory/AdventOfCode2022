open System
open Day12

let instructions = IO.File.ReadAllText "input.txt"

let shortestSteps = instructions |> SolutionForwards.GetShortestRouteNumberOfSteps
Console.WriteLine $"Minimum number of steps is: {shortestSteps}"