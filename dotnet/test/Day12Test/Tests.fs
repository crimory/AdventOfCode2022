module Day12Test.SolutionTests

open Xunit
open Day12.Solution

let Input = @"Sabqponm
abcryxxl
accszExk
acctuvwj
abdefghi"

[<Fact>]
let ``Reading the input`` () =
    let expectedStart = { Height = Start; Coordinates = { X = 0; Y = 0 } }
    let expectedEnd = { Height = End; Coordinates = { X = 5; Y = 2 } }
    let expectedExample1 = { Height = Height 21; Coordinates = { X = 4; Y = 3 } }
    let expectedExample2 = { Height = Height 9; Coordinates = { X = 7; Y = 4 } }
    let expected = [ expectedStart; expectedEnd; expectedExample1; expectedExample2 ]
    let result = ReadInput Input
    expected
    |> List.map (fun x -> Assert.Contains (x, result))