module Tests

open Xunit
open Day13.Solution

let Input = @"[1,1,3,1,1]
[1,1,5,1,1]

[[1],[2,3,4]]
[[1],4]

[9]
[[8,7,6]]

[[4,4],4,4]
[[4,4],4,4,4]

[7,7,7,7]
[7,7,7]

[]
[3]

[[[]]]
[[]]

[1,[2,[3,[4,[5,6,7]]]],8,9]
[1,[2,[3,[4,[5,6,0]]]],8,9]"

[<Theory>]
[<InlineData ("1,2", "1", "2")>]
[<InlineData ("[1,2]", "1", "2")>]
[<InlineData ("[[1],[2,3,4]]", "[1]", "[2,3,4]")>]
[<InlineData ("[1],[2,3,4]", "[1]", "[2,3,4]")>]
[<InlineData ("[1],[2,[3,4,[]]]", "[1]", "[2,[3,4,[]]]")>]
[<InlineData ("[[1],[2,[3,4,[]]]]", "[1]", "[2,[3,4,[]]]")>]
let ``DivideString`` (input: string) expectedPartOne expectedPartTwo =
    let expected =
        if (expectedPartTwo <> "")
        then [ expectedPartOne; expectedPartTwo ]
        else [ expectedPartOne ]
    let result = DivideString input
    Assert.Equal (expected.Length, result.Length)
    expected
    |> List.zip result
    |> List.map Assert.Equal

[<Fact>]
let ``ReadPacketPart example 01`` () =
    let expected = ListOfValues [ ListOfValues [ Value 1 ]; ListOfValues [ Value 2; Value 3; Value 4 ] ]
    let result = ReadPacketPart "[[1],[2,3,4]]"
    Assert.Equal (expected, result)

[<Fact>]
let ``ReadPacketPart example 02`` () =
    let expected = ListOfValues [ ListOfValues [ Value 8; Value 7; Value 6 ] ]
    let result = ReadPacketPart "[[8,7,6]]"
    Assert.Equal (expected, result)

[<Fact>]
let ``ReadPacketPart example 03`` () =
    let expected = ListOfValues [ ListOfValues [ ListOfValues [] ] ]
    let result = ReadPacketPart "[[[]]]"
    Assert.Equal (expected, result)

[<Fact>]
let ``Reading input`` () =
    let expected = [
        {
            LeftItems = ListOfValues [ Value 1; Value 1; Value 3; Value 1; Value 1 ]
            RightItems = ListOfValues [ Value 1; Value 1; Value 5; Value 1; Value 1 ]
        }
        {
            LeftItems = ListOfValues [ ListOfValues [ Value 1 ]; ListOfValues [ Value 2; Value 3; Value 4 ] ]
            RightItems = ListOfValues [ ListOfValues [ Value 1 ]; Value 4 ]
        }
        {
            LeftItems = ListOfValues [ Value 9 ]
            RightItems = ListOfValues [ ListOfValues [ Value 8; Value 7; Value 6 ] ]
        }
        {
            LeftItems = ListOfValues [ ListOfValues [ Value 4; Value 4 ]; Value 4; Value 4 ]
            RightItems = ListOfValues [ ListOfValues [ Value 4; Value 4 ]; Value 4; Value 4; Value 4 ]
        }
        {
            LeftItems = ListOfValues [ Value 7; Value 7; Value 7; Value 7 ]
            RightItems = ListOfValues [ Value 7; Value 7; Value 7 ]
        }
        {
            LeftItems = ListOfValues [ ]
            RightItems = ListOfValues [ Value 3 ]
        }
        {
            LeftItems = ListOfValues [ ListOfValues [ ListOfValues [] ] ]
            RightItems = ListOfValues [ ListOfValues [] ]
        }
        {
            LeftItems = ListOfValues [ Value 1; ListOfValues [ Value 2; ListOfValues [ Value 3; ListOfValues [ Value 4; ListOfValues [ Value 5; Value 6; Value 7 ] ] ] ]; Value 8; Value 9 ]
            RightItems = ListOfValues [ Value 1; ListOfValues [ Value 2; ListOfValues [ Value 3; ListOfValues [ Value 4; ListOfValues [ Value 5; Value 6; Value 0 ] ] ] ]; Value 8; Value 9 ]
        }
    ]
    let result = ReadInput Input
    Assert.Equal (8, result.Length)
    expected
    |> List.zip result
    |> List.map Assert.Equal

[<Fact>]
let ``Compare values example 01`` () =
    let result = ComparePacketOrder (Value 1) (Value 1)
    Assert.Equal (NotDecided, result)
    let result = ComparePacketOrder (Value 2) (Value 1)
    Assert.Equal (IncorrectOrder, result)
    let result = ComparePacketOrder (Value 1) (Value 2)
    Assert.Equal (CorrectOrder, result)

[<Fact>]
let ``Compare values example 02`` () =
    let result = ComparePacketOrder
                     (ListOfValues [ Value 1; Value 2 ])
                     (ListOfValues [ Value 1; Value 2 ])
    Assert.Equal (NotDecided, result)
    let result = ComparePacketOrder
                     (ListOfValues [ Value 1; Value 1 ])
                     (ListOfValues [ Value 1; Value 2 ])
    Assert.Equal (CorrectOrder, result)
    let result = ComparePacketOrder
                     (ListOfValues [ Value 1; Value 2 ])
                     (ListOfValues [ Value 1; Value 1 ])
    Assert.Equal (IncorrectOrder, result)
    let result = ComparePacketOrder
                     (ListOfValues [ Value 1; Value 1 ])
                     (ListOfValues [ Value 1; Value 1; Value 1 ])
    Assert.Equal (CorrectOrder, result)
    let result = ComparePacketOrder
                     (ListOfValues [ Value 1; Value 1; Value 1 ])
                     (ListOfValues [ Value 1; Value 1 ])
    Assert.Equal (IncorrectOrder, result)

[<Fact>]
let ``Compare values example 03`` () =
    let result = ComparePacketOrder
                     (ListOfValues [ Value 1; ])
                     (Value 1)
    Assert.Equal (NotDecided, result)
    let result = ComparePacketOrder
                     (ListOfValues [ Value 1; ])
                     (Value 2)
    Assert.Equal (CorrectOrder, result)
    let result = ComparePacketOrder
                     (ListOfValues [ Value 2; ])
                     (Value 1)
    Assert.Equal (IncorrectOrder, result)
    let result = ComparePacketOrder
                     (ListOfValues [ Value 1; Value 2 ])
                     (Value 1)
    Assert.Equal (IncorrectOrder, result)

[<Fact>]
let ``Test whole input`` () =
    let result = GetSumOfPacketIndicesInCorrectOrder Input
    Assert.Equal (13, result)