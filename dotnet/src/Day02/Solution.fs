module Day02.Solution

open System

type RockPaperScissorsSign = Rock | Paper | Scissors
type RockPaperScissorsOutcome = Loose | Draw | Win

let calculateScore opponentSign playerSign =
    let scoreFromRoundOutcome =
        match opponentSign, playerSign with
        | opponent, player when opponent = player -> 3
        | Rock, Scissors
        | Scissors, Paper
        | Paper, Rock -> 0
        | _ -> 6
    let scoreFromPlayerSign =
        match playerSign with
        | Rock -> 1
        | Paper -> 2
        | Scissors -> 3
    scoreFromRoundOutcome + scoreFromPlayerSign

let calculateLineScoreTwoSigns (text: string) =
    let matchOpponentSign = dict["A",Rock; "B",Paper; "C",Scissors]
    let matchPlayerSign = dict["X",Rock; "Y",Paper; "Z",Scissors]
    let signCodes = text.Trim().Split(" ")
    
    let opponentSign = matchOpponentSign[signCodes[0]]
    let playerSign = matchPlayerSign[signCodes[1]]
    calculateScore opponentSign playerSign

let calculateStrategyScoreTwoSigns (text: string) =
    text.Trim().Split(Environment.NewLine)
    |> Array.map calculateLineScoreTwoSigns
    |> Array.sum

let calculateLineScoreSignAndOutcome (text: string) =
    let matchOpponentSign = dict["A",Rock; "B",Paper; "C",Scissors]
    let matchPlayerSign = dict["X",Loose; "Y",Draw; "Z",Win]
    let signCodes = text.Trim().Split(" ")
    
    let opponentSign = matchOpponentSign[signCodes[0]]
    let roundOutcome = matchPlayerSign[signCodes[1]]
    let playerSign =
        match roundOutcome, opponentSign with
        | Draw, opponent -> opponent
        | Loose, opponent ->
            match opponent with
            | Rock -> Scissors
            | Paper -> Rock
            | Scissors -> Paper
        | Win, opponent ->
            match opponent with
            | Rock -> Paper
            | Paper -> Scissors
            | Scissors -> Rock
    calculateScore opponentSign playerSign

let calculateStrategyScoreSignAndOutcome (text: string) =
    text.Trim().Split(Environment.NewLine)
    |> Array.map calculateLineScoreSignAndOutcome
    |> Array.sum