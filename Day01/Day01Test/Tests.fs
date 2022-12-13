module Tests

open Xunit
open Day01.Solution

[<Fact>]
let ``Divide by elves`` () =
    let input =
        """1
        2
        3

        4

        5
        6

        7
        8
        9

        10"""
    let expected = [[1;2;3];[4];[5;6];[7;8;9];[10]]
    
    let result = separateByElves input
    Assert.Equivalent (expected, result, strict = true)

[<Fact>]
let ``Get max calories from elves`` () =
    let input = [[1;2;3];[4];[5;6];[7;8;9];[10]]
    let expected = 24
    let result = input |> getMostCalories
    Assert.Equal (expected, result)

[<Fact>]
let ``Get max calories from 3 elves`` () =
    let input = [[1;2;3];[4];[5;6];[7;8;9];[10]]
    let expected = 45
    let result = input |> getMostCaloriesFirstElves 3
    Assert.Equal (expected, result)