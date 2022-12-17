module Day02.Main

open System
open Day02.Solution

let strategyLines = System.IO.File.ReadAllText "input.txt"
let strategyScoreTwoSigns =
    strategyLines
    |> calculateStrategyScoreTwoSigns
Console.WriteLine $"Calculated strategy score based on 2 signs is: {strategyScoreTwoSigns}"

let strategyScoreSignAndOutcome =
    strategyLines
    |> calculateStrategyScoreSignAndOutcome
Console.WriteLine $"Calculated strategy score based on a sign and an outcome is: {strategyScoreSignAndOutcome}"