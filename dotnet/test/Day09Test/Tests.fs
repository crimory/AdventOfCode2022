module Tests

open Xunit
open Day09.Solution

let Input = @"R 4
U 4
L 3
D 1
R 4
D 1
L 5
R 2"

let LongInput = @"R 5
U 8
L 8
D 3
R 17
D 10
L 25
U 20"

[<Fact>]
let ``Read moves correctly`` () =
    let result = ReadMoves Input
    let expected = [
        { Direction = Right; Amount = 4u }
        { Direction = Up; Amount = 4u }
        { Direction = Left; Amount = 3u }
        { Direction = Down; Amount = 1u }
        { Direction = Right; Amount = 4u }
        { Direction = Down; Amount = 1u }
        { Direction = Left; Amount = 5u }
        { Direction = Right; Amount = 2u }
    ]
    Assert.Equivalent (expected, result)

type SingleMoveTestCases () as this =
    inherit TheoryData<ShortRope, Direction, Point>()
    do this.Add({Head={X=0;Y=0};Tail={X=0;Y=0}}, Direction.Left, {X=0;Y=0})
       this.Add({Head={X=0;Y=0};Tail={X=0;Y=0}}, Direction.Down, {X=0;Y=0})
       this.Add({Head={X=1;Y=1};Tail={X=0;Y=0}}, Direction.Left, {X=0;Y=0})
       this.Add({Head={X=1;Y=1};Tail={X=0;Y=0}}, Direction.Right, {X=1;Y=1})
       this.Add({Head={X=1;Y=1};Tail={X=0;Y=0}}, Direction.Up, {X=1;Y=1})
       this.Add({Head={X=1;Y=0};Tail={X=0;Y=0}}, Direction.Right, {X=1;Y=0})

[<Theory; ClassData(typeof<SingleMoveTestCases>)>]
let ``Perform single rope move`` rope direction expectedTail =
    let result = SingleMoveShortRope direction rope
    Assert.Equal (expectedTail, result.Tail)
    
[<Fact>]
let ``Move long rope test example 01`` () =
    let input = { Head = { X = 5; Y = 0 }; Tails = [
            { X = 4; Y = 0 }
            { X = 3; Y = 0 }
            { X = 2; Y = 0 }
            { X = 1; Y = 0 }
            { X = 0; Y = 0 }
            { X = 0; Y = 0 }
            { X = 0; Y = 0 }
            { X = 0; Y = 0 }
            { X = 0; Y = 0 }
        ] }
    let expected = { Head = { X = 5; Y = 8 }; Tails = [
            { X = 5; Y = 7 }
            { X = 5; Y = 6 }
            { X = 5; Y = 5 }
            { X = 5; Y = 4 }
            { X = 4; Y = 4 }
            { X = 3; Y = 3 }
            { X = 2; Y = 2 }
            { X = 1; Y = 1 }
            { X = 0; Y = 0 }
        ] }
    let move = { Direction = Up; Amount = 8u }
    let result = MoveLongRope move input |> snd
    Assert.Equal (expected, result)

[<Fact>]
let ``Main test of HowManyPositionsWasTailAt`` () =
    let result = HowManyPositionsWasTailAt Input
    Assert.Equal (13, result)

[<Fact>]
let ``Main test of HowManyPositionsWasLongTailAt`` () =
    let result = HowManyPositionsWasLongTailAt Input
    Assert.Equal (1, result)  

[<Fact>]
let ``Main test of longer example for HowManyPositionsWasLongTailAt`` () =
    let result = HowManyPositionsWasLongTailAt LongInput
    Assert.Equal (36, result)