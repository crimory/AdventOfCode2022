open System
open Day11

let instructions = IO.File.ReadAllText "input.txt"

let monkeyBusinessScore = instructions |> Solution.MonkeyBusinessScore 20 (fun x -> x / 3UL)
Console.WriteLine $"Monkey business score is: {monkeyBusinessScore}"

let monkeyBusinessScoreRoundTwo = instructions |> Solution.MonkeyBusinessScore 10_000 id
Console.WriteLine $"Monkey business score for round 2 is: {monkeyBusinessScoreRoundTwo}"