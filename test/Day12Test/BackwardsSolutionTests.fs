module Day12Test.BackwardsSolutionTests

open Day12.Solution
open Day12.SolutionBackwards
open SolutionTests
open Xunit

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