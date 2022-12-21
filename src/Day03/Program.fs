module Day03.Main

open System
open Day03.Solution

let rucksackLines = System.IO.File.ReadAllText "input.txt"
let prioritySum =
    rucksackLines
    |> getPrioritySum

Console.WriteLine $"Priority sum for the rucksack is {prioritySum}"

let prioritySumFor3ElfBadges =
    rucksackLines
    |> getPrioritySumPerElfBadge

Console.WriteLine $"Priority sum for the 3-Elf groups is {prioritySumFor3ElfBadges}"