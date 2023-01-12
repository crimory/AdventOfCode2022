open System
open Day12

let instructions = IO.File.ReadAllText "input.txt"

let shortestSteps = instructions |> Solution.GetShortestRouteNumberOfSteps
Console.WriteLine $"Minimum number of steps is: {shortestSteps}"