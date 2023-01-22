module Tests

open System
open Xunit
open Day14.Solution

let Input = "498,4 -> 498,6 -> 496,6
503,4 -> 502,4 -> 502,9 -> 494,9"

[<Fact>]
let ``Build map`` () =
    let result = ReadInput Input
    Assert.Contains({ X = 498; Y = 4 }, result)
    Assert.Contains({ X = 498; Y = 5 }, result)
    Assert.Contains({ X = 498; Y = 6 }, result)
    Assert.Contains({ X = 497; Y = 6 }, result)
    Assert.Contains({ X = 496; Y = 6 }, result)
    Assert.Contains({ X = 503; Y = 4 }, result)
    Assert.Contains({ X = 502; Y = 4 }, result)
    Assert.Contains({ X = 502; Y = 5 }, result)
    Assert.Contains({ X = 502; Y = 6 }, result)
    Assert.Contains({ X = 502; Y = 7 }, result)
    Assert.Contains({ X = 502; Y = 8 }, result)
    Assert.Contains({ X = 502; Y = 9 }, result)
    Assert.Contains({ X = 501; Y = 9 }, result)
    Assert.Contains({ X = 500; Y = 9 }, result)
    Assert.Contains({ X = 499; Y = 9 }, result)
    Assert.Contains({ X = 498; Y = 9 }, result)
    Assert.Contains({ X = 497; Y = 9 }, result)
    Assert.Contains({ X = 496; Y = 9 }, result)
    Assert.Contains({ X = 495; Y = 9 }, result)
    Assert.Contains({ X = 494; Y = 9 }, result)

[<Theory>]
[<InlineData (500, 0, false, false)>]
[<InlineData (500, 8, false, true)>]
[<InlineData (500, 0, true, true)>]
[<InlineData (498, 3, false, false)>]
let ``Check next sand unit state`` xInput yInput isSettled expectedIsSettled =
    let map = ReadInput Input
    let sandUnit = { Position = { X = xInput; Y = yInput }; State = if isSettled then Settled else Falling }
    let result = GetSandUnitNextState map sandUnit
    Assert.Equal((if expectedIsSettled then Settled else Falling), result.State)

[<Fact>]
let ``Get settled sand units count`` () =
    let result = GetNumberOfSandUnitsThatSettle Input
    Assert.Equal(24, result)

[<Fact>]
let ``Get settled sand units count, including floor`` () =
    let result = GetNumberOfSandUnitsThatSettleWithFloor Input
    Assert.Equal(93, result)