namespace Day09

module Main =
    open Day09.Solution
    open System
    
    let instructions = IO.File.ReadAllText "input.txt"
    
    let numberOfTailPositions = instructions |> HowManyPositionsWasTailAt
    Console.WriteLine $"Tail was at {numberOfTailPositions} of positions through the way"
    
    let numberOfNineTailPositions = instructions |> HowManyPositionsWasLongTailAt
    Console.WriteLine $"Tail #9 was at {numberOfNineTailPositions} of positions through the way"