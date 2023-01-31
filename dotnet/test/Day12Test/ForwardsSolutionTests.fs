module Day12Test.ForwardsSolutionTests

open Day12
open SolutionForwards
open Solution
open SolutionTests
open Xunit

[<Fact>]
let ``Shortest route steps example 1`` () =
    let result = GetShortestRouteNumberOfSteps Input
    Assert.Equal (31, result)

[<Fact>]
let ``Shortest route towards any a (lowest point)`` () =
    let result = GetShortestRouteNumberOfStepsToAnyLowestPoint Input
    Assert.Equal (29, result)

[<Theory>]
[<InlineData (0, 0, 2)>]
[<InlineData (3, 0, 3)>]
[<InlineData (1, 1, 4)>]
[<InlineData (2, 3, 3)>]
[<InlineData (3, 3, 4)>]
let GetNextAcceptablePointsTest currentX currentY expectedNumberOfPoints =
    let map = ReadInput Input
    let currentPoint = map |> List.find (fun x -> x.Coordinates.X = currentX && x.Coordinates.Y = currentY)
    let result = GetNextAcceptablePoints (IsNextPointHeightAcceptable Upwards) map currentPoint
    Assert.Equal (expectedNumberOfPoints, result.Length)

[<Theory>]
[<InlineData (0, 0, 2)>]
[<InlineData (3, 0, 3)>]
[<InlineData (1, 1, 4)>]
[<InlineData (2, 3, 4)>]
[<InlineData (3, 3, 4)>]
[<InlineData (3, 4, 3)>]
[<InlineData (7, 4, 2)>]
[<InlineData (7, 3, 3)>]
let GetPossibleNextPointsTests currentX currentY expectedNumberOfPoints =
    let map = ReadInput Input
    let currentPoint = map |> List.find (fun x -> x.Coordinates.X = currentX && x.Coordinates.Y = currentY)
    let result = GetPossibleNextPoints map currentPoint
    Assert.Equal (expectedNumberOfPoints, result.Length)

[<Theory>]
[<InlineData (0, 0, 1, 0, true)>]
[<InlineData (0, 0, 0, 1, true)>]
[<InlineData (0, 0, 1, 1, true)>]
[<InlineData (0, 0, 2, 1, false)>]
[<InlineData (3, 3, 2, 3, true)>]
let IsNextPointHeightAcceptableTests currentX currentY nextX nextY expected =
    let map = ReadInput Input
    let currentPoint = map |> List.find (fun x -> x.Coordinates.X = currentX && x.Coordinates.Y = currentY)
    let nextPoint = map |> List.find (fun x -> x.Coordinates.X = nextX && x.Coordinates.Y = nextY)
    let result = IsNextPointHeightAcceptable Upwards currentPoint nextPoint
    Assert.Equal (expected, result)

let InputForBranchTests = @"SabcdefghijklmnopqrstuvwxyzE
aaaaaaaaaaaaaaaaaaaaaaaaaaaz"
let branchTestsStart = { Height = Start; Coordinates = { X = 0; Y = 0 } }
let branchTestsACorrect = { Height = Height 1; Coordinates = { X = 1; Y = 0 } }
let branchTestsBCorrect = { Height = Height 2; Coordinates = { X = 2; Y = 0 } }
let branchTestsAIncorrect01 = { Height = Height 1; Coordinates = { X = 0; Y = 1 } }
let branchTestsAIncorrect11 = { Height = Height 1; Coordinates = { X = 1; Y = 1 } }

[<Fact>]
let GetFullBranchesTests () =
    let map = ReadInput InputForBranchTests
    let result = GetFullBranchesFromStartToEnd map
    Assert.Equal (1, result.Length)
    Assert.Equal (28, result.Head.Length)

[<Fact>]
let GetNextStepBranchesExample01 () =
    let expected = [ [branchTestsStart; branchTestsACorrect]; [branchTestsStart; branchTestsAIncorrect01] ]
    
    let map = ReadInput InputForBranchTests
    let getNextAcceptablePoints = GetNextAcceptablePoints (IsNextPointHeightAcceptable Upwards) map
    let result = GetNextStepBranches getNextAcceptablePoints [ map |> List.find (fun x -> x.Height = Start) ]
    Assert.Equal (expected.Length, result.Length)
    Assert.Equivalent (expected[0], result[0])
    Assert.Equivalent (expected[1], result[1])

[<Fact>]
let GetNextStepBranchesExample02 () =
    let previousPoints = [branchTestsStart; branchTestsACorrect]
    let expected = [
        previousPoints @ [branchTestsBCorrect]
        previousPoints @ [branchTestsAIncorrect11]
    ]
    
    let map = ReadInput InputForBranchTests
    let getNextAcceptablePoints = GetNextAcceptablePoints (IsNextPointHeightAcceptable Upwards) map
    let result = GetNextStepBranches getNextAcceptablePoints previousPoints
    Assert.Equal (expected.Length, result.Length)
    Assert.Equivalent (expected[0], result[0])
    Assert.Equivalent (expected[1], result[1])

[<Fact>]
let GetNextStepBranchesExample03 () =
    let previousPoints = [
        branchTestsStart
        { Height = Height 1; Coordinates = { X = 1; Y = 0 } }
        { Height = Height 2; Coordinates = { X = 2; Y = 0 } }
        { Height = Height 3; Coordinates = { X = 3; Y = 0 } }
        { Height = Height 4; Coordinates = { X = 4; Y = 0 } }
        { Height = Height 5; Coordinates = { X = 5; Y = 0 } }
        { Height = Height 6; Coordinates = { X = 6; Y = 0 } }
        { Height = Height 7; Coordinates = { X = 7; Y = 0 } }
        { Height = Height 8; Coordinates = { X = 8; Y = 0 } }
        { Height = Height 9; Coordinates = { X = 9; Y = 0 } }
        { Height = Height 10; Coordinates = { X = 10; Y = 0 } }
        { Height = Height 11; Coordinates = { X = 11; Y = 0 } }
        { Height = Height 12; Coordinates = { X = 12; Y = 0 } }
        { Height = Height 13; Coordinates = { X = 13; Y = 0 } }
        { Height = Height 14; Coordinates = { X = 14; Y = 0 } }
        { Height = Height 15; Coordinates = { X = 15; Y = 0 } }
        { Height = Height 16; Coordinates = { X = 16; Y = 0 } }
        { Height = Height 17; Coordinates = { X = 17; Y = 0 } }
        { Height = Height 18; Coordinates = { X = 18; Y = 0 } }
        { Height = Height 19; Coordinates = { X = 19; Y = 0 } }
        { Height = Height 20; Coordinates = { X = 20; Y = 0 } }
        { Height = Height 21; Coordinates = { X = 21; Y = 0 } }
        { Height = Height 22; Coordinates = { X = 22; Y = 0 } }
        { Height = Height 23; Coordinates = { X = 23; Y = 0 } }
        { Height = Height 24; Coordinates = { X = 24; Y = 0 } }
        { Height = Height 25; Coordinates = { X = 25; Y = 0 } }
        { Height = Height 26; Coordinates = { X = 26; Y = 0 } }
    ]
    let expected = [
        previousPoints @ [{ Height = End; Coordinates = { X = 27; Y = 0 } }]
        previousPoints @ [{ Height = Height 1; Coordinates = { X = 26; Y = 1 } }]
    ]
    
    let map = ReadInput InputForBranchTests
    let getNextAcceptablePoints = GetNextAcceptablePoints (IsNextPointHeightAcceptable Upwards) map
    let result = GetNextStepBranches getNextAcceptablePoints previousPoints
    
    Assert.Equal (expected.Length, result.Length)
    Assert.Equivalent (expected[0], result[0])
    Assert.Equivalent (expected[1], result[1])

[<Fact>]
let ``Shortest route steps example 2`` () =
    let result = GetShortestRouteNumberOfSteps InputForBranchTests
    Assert.Equal (27, result)