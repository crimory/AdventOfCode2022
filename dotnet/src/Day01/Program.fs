module Day01.Main

open System
open Day01.Solution

let inputSeparatedByElves =
    System.IO.File.ReadAllText "input.txt"
    |> separateByElves

let sum =
    inputSeparatedByElves
    |> getMostCalories
Console.WriteLine $"Highest calories sum is: {sum}"

let sumOfThree =
    inputSeparatedByElves
    |> getMostCaloriesFirstElves 3
Console.WriteLine $"Highest calories sum for 3 first elves is: {sumOfThree}"