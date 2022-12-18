module Day04.Main

open System
open Day04.Solution

let assignmentText = System.IO.File.ReadAllText "input.txt"

let numberFullyContained =
    assignmentText
    |> numberOfAssignmentsFullyContained

Console.WriteLine $"Number of assignments fully contained: {numberFullyContained}"

let numberOverlapping =
    assignmentText
    |> numberOfAssignmentsOverlapping

Console.WriteLine $"Number of assignments overlapping: {numberOverlapping}"