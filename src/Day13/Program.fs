open System
open Day13.Solution

let instructions = IO.File.ReadAllText "input.txt"

let sumOfCorrectPacketsIndices = instructions |> GetSumOfPacketIndicesInCorrectOrder
Console.WriteLine $"Sum of indices of correct order indices: {sumOfCorrectPacketsIndices}"