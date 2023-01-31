module Tests

open System
open Day10.Solution
open Xunit

let Input = @"addx 15
addx -11
addx 6
addx -3
addx 5
addx -1
addx -8
addx 13
addx 4
noop
addx -1
addx 5
addx -1
addx 5
addx -1
addx 5
addx -1
addx 5
addx -1
addx -35
addx 1
addx 24
addx -19
addx 1
addx 16
addx -11
noop
noop
addx 21
addx -15
noop
noop
addx -3
addx 9
addx 1
addx -3
addx 8
addx 1
addx 5
noop
noop
noop
noop
noop
addx -36
noop
addx 1
addx 7
noop
noop
noop
addx 2
addx 6
noop
noop
noop
noop
noop
addx 1
noop
noop
addx 7
addx 1
noop
addx -13
addx 13
addx 7
noop
addx 1
addx -33
noop
noop
noop
addx 2
noop
noop
noop
addx 8
noop
addx -1
addx 2
addx 1
noop
addx 17
addx -9
addx 1
addx 1
addx -3
addx 11
noop
noop
addx 1
noop
addx 1
noop
noop
addx -13
addx -19
addx 1
addx 3
addx 26
addx -30
addx 12
addx -1
addx 3
addx 1
noop
noop
noop
addx -9
addx 18
addx 1
addx 2
noop
noop
addx 9
noop
noop
noop
addx -1
addx 2
addx -37
addx 1
addx 3
noop
addx 15
addx -21
addx 22
addx -6
addx 1
noop
addx 2
addx 1
noop
addx -10
noop
noop
addx 20
addx 1
addx 2
addx 2
addx -6
addx -11
noop
noop
noop"

[<Fact>]
let ``Read instructions`` () =
    let input = @"noop
addx 3
addx -5"
    let expected = [ Instruction.Noop; AddX 3; AddX -5 ]
    let result = ReadInstructions input
    Assert.Equivalent (expected, result)

[<Fact>]
let ``Build instructions output`` () =
    let instructions = [ Instruction.Noop; AddX 3; AddX -5 ]
    let expected = [ 1; 1; 1; 1; 4; 4; -1 ]
    let result = BuildCycleOutput instructions
    Assert.Equivalent (expected, result)

[<Fact>]
let ``Simple example of summing signal strength`` () =
    let indices = [ 20 .. 40 .. 220 ]
    let result = SumSignalStrength indices Input
    Assert.Equal (13140L, result)

[<Fact>]
let ``Test CRT output`` () =
    let expected = [ "##..##..##..##..##..##..##..##..##..##.."
                     "###...###...###...###...###...###...###."
                     "####....####....####....####....####...."
                     "#####.....#####.....#####.....#####....."
                     "######......######......######......####"
                     "#######.......#######.......#######....." ]
    let result = GetCrtOutput Input
    Assert.Equivalent (expected, result)