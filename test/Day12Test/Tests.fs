module Tests

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

[<Theory>]
[<InlineData (0, 0, 2)>]
[<InlineData (3, 0, 2)>]
[<InlineData (1, 1, 4)>]
[<InlineData (2, 3, 4)>]
[<InlineData (3, 3, 2)>]
let ``Acceptable previous steps`` currentX currentY expectedAmountFound =
    let map = ReadInput Input
    let currentPoint = map |> List.find (fun p -> p.Coordinates.X = currentX && p.Coordinates.Y = currentY)
    let result = GetAcceptablePreviousPoints map currentPoint
    Assert.Equal (expectedAmountFound, result |> List.length)

[<Fact>]
let ``Shortest route steps`` () =
    let result = GetShortestRouteNumberOfSteps Input
    Assert.Equal (31, result)