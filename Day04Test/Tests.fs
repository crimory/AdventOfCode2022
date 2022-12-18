module Tests

open Day04.Solution
open Xunit

[<Fact>]
let ``List of assignment pairs contains 2 where 1 fully contains the other`` () =
    let input =
        """2-4,6-8
2-3,4-5
5-7,7-9
2-8,3-7
6-6,4-6
2-6,4-8"""

    let result = numberOfAssignmentsFullyContained input
    Assert.Equal(2, result)

[<Theory>]
[<InlineData("2-4,6-8", 2, 4, 6, 8)>]
[<InlineData("2-3,4-5", 2, 3, 4, 5)>]
[<InlineData("5-7,7-9", 5, 7, 7, 9)>]
[<InlineData("2-8,3-7", 2, 8, 3, 7)>]
[<InlineData("6-6,4-6", 6, 6, 4, 6)>]
[<InlineData("2-6,4-8", 2, 6, 4, 8)>]
let ``Read assignment pairs`` line expectedE1R1 expectedE1R2 expectedE2R1 expectedE2R2 =
    let result = readAssignmentPair line

    let expected =
        { FirstElf = { FirstRoom = expectedE1R1; LastRoom = expectedE1R2 }
          SecondElf = { FirstRoom = expectedE2R1; LastRoom = expectedE2R2 } }

    Assert.Equal(expected, result)

[<Fact>]
let ``List of assignment pairs contains 4 where there is an overlap`` () =
    let input =
        """2-4,6-8
2-3,4-5
5-7,7-9
2-8,3-7
6-6,4-6
2-6,4-8"""

    let result = numberOfAssignmentsOverlapping input
    Assert.Equal(4, result)

[<Fact>]
let ``List of assignment pairs contains 2 where there is an overlap`` () =
    let input =
        """1-3,2-4
5-7,1-5"""

    let result = numberOfAssignmentsOverlapping input
    Assert.Equal(2, result)