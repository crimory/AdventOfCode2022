module Tests

open Xunit
open Day05.Solution

[<Fact>]
let ``Process whole ship example`` () =
    let input =
        """    [D]    
[N] [C]    
[Z] [M] [P]
 1   2   3 

move 1 from 2 to 1
move 3 from 1 to 3
move 2 from 2 to 1
move 1 from 1 to 2"""

    let result = processCraneMover9000Plan input
    Assert.Equal ("CMZ", result)

[<Fact>]
let ``Separate ship and crane plan example`` () =
    let input =
        """    [D]    
[N] [C]    
[Z] [M] [P]
 1   2   3 

move 1 from 2 to 1
move 3 from 1 to 3
move 2 from 2 to 1
move 1 from 1 to 2"""

    let expectedShip =
        """    [D]    
[N] [C]    
[Z] [M] [P]
 1   2   3 """

    let expectedCranePlan =
        """move 1 from 2 to 1
move 3 from 1 to 3
move 2 from 2 to 1
move 1 from 1 to 2"""

    let resultingShip, resultingCranePlan = separateShipFromCranePlan input
    Assert.Equal (expectedShip, resultingShip)
    Assert.Equal (expectedCranePlan, resultingCranePlan)

[<Fact>]
let ``Read ship plan example`` () =
    let input =
        """    [D]    
[N] [C]    
[Z] [M] [P]
 1   2   3 """

    let expectedOutput =
        Stacks
            [ { Index = 1
                Crates = [ Label 'N'; Label 'Z' ] }
              { Index = 2
                Crates = [ Label 'D'; Label 'C'; Label 'M' ] }
              { Index = 3
                Crates = [ Label 'P' ] }]

    let result = readShipPlan input
    Assert.Equal (expectedOutput, result)
    
[<Fact>]
let ``Read list of instructions example`` () =
    let input =
        """move 1 from 2 to 1
move 3 from 1 to 3
move 2 from 2 to 1
move 1 from 1 to 2"""

    let expectedOutput =
        [
            { HowManyCrates = 1; FromStackIndex = 2; ToStackIndex = 1 }
            { HowManyCrates = 3; FromStackIndex = 1; ToStackIndex = 3 }
            { HowManyCrates = 2; FromStackIndex = 2; ToStackIndex = 1 }
            { HowManyCrates = 1; FromStackIndex = 1; ToStackIndex = 2 }
        ]

    let result = readCraneInstructions input
    Assert.Equivalent (expectedOutput, result, strict = true)

[<Fact>]
let ``Perform single instruction`` () =
    let shipInput =
        Stacks
            [ { Index = 1
                Crates = [ Label 'N'; Label 'Z' ] }
              { Index = 2
                Crates = [ Label 'D'; Label 'C'; Label 'M' ] }
              { Index = 3
                Crates = [ Label 'P' ] }]
    let instruction = { HowManyCrates = 2; FromStackIndex = 2; ToStackIndex = 3 }
    
    let expectedOutput =
        Stacks
            [ { Index = 1
                Crates = [ Label 'N'; Label 'Z' ] }
              { Index = 2
                Crates = [ Label 'M' ] }
              { Index = 3
                Crates = [ Label 'C'; Label 'D'; Label 'P' ] }]

    let result = shipInput |> performSingleInstructionCraneMover9000 instruction
    Assert.Equal (expectedOutput, result)

[<Fact>]
let ``Process whole ship with CrateMover 9001`` () =
    let input =
        """    [D]    
[N] [C]    
[Z] [M] [P]
 1   2   3 

move 1 from 2 to 1
move 3 from 1 to 3
move 2 from 2 to 1
move 1 from 1 to 2"""

    let result = processCraneMover9001Plan input
    Assert.Equal ("MCD", result)
