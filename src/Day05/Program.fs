namespace Day05

module Main =
    open System
    open Day05.Solution

    let shipmentInput = System.IO.File.ReadAllText "input.txt"
    
    let finalTopCrates =
        shipmentInput
        |> processCraneMover9000Plan

    Console.WriteLine $"Final top crates are: {finalTopCrates}"
    
    let finalTopCratesCrateMover9001 =
        shipmentInput
        |> processCraneMover9001Plan

    Console.WriteLine $"Final top crates for CrateMover 9001 are: {finalTopCratesCrateMover9001}"