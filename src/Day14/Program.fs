open System
open Day14.Solution

let instructions = IO.File.ReadAllText "input.txt"

let numberOfSandUnitsThatSettle = instructions |> GetNumberOfSandUnitsThatSettle
Console.WriteLine $"Number of settled sand units: {numberOfSandUnitsThatSettle}"