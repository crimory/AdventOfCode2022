module Tests

open System
open Xunit
open Day11.Solution

let Input = @"Monkey 0:
  Starting items: 79, 98
  Operation: new = old * 19
  Test: divisible by 23
    If true: throw to monkey 2
    If false: throw to monkey 3

Monkey 1:
  Starting items: 54, 65, 75, 74
  Operation: new = old + 6
  Test: divisible by 19
    If true: throw to monkey 2
    If false: throw to monkey 0

Monkey 2:
  Starting items: 79, 60, 97
  Operation: new = old * old
  Test: divisible by 13
    If true: throw to monkey 1
    If false: throw to monkey 3

Monkey 3:
  Starting items: 74
  Operation: new = old + 3
  Test: divisible by 17
    If true: throw to monkey 0
    If false: throw to monkey 1"

[<Fact>]
let ``Reading the monkey setup`` () =
    let expected = [
        {
            Index = 0u<MonkeyIndex>
            Items = [ 79u<WorryLevel>; 98u<WorryLevel> ]
            Operation = { Element1 = Old; Sign = Multiply; Element2 = Number 19u }
            Test = DivisibleBy 23u
            TestPositive = ThrowToMonkey 2u<MonkeyIndex>
            TestNegative = ThrowToMonkey 3u<MonkeyIndex>
        }
        {
            Index = 1u<MonkeyIndex>
            Items = [ 54u<WorryLevel>; 65u<WorryLevel>; 75u<WorryLevel>; 74u<WorryLevel> ]
            Operation = { Element1 = Old; Sign = Plus; Element2 = Number 6u }
            Test = DivisibleBy 19u
            TestPositive = ThrowToMonkey 2u<MonkeyIndex>
            TestNegative = ThrowToMonkey 0u<MonkeyIndex>
        }
        {
            Index = 2u<MonkeyIndex>
            Items = [ 79u<WorryLevel>; 60u<WorryLevel>; 97u<WorryLevel> ]
            Operation = { Element1 = Old; Sign = Multiply; Element2 = Old }
            Test = DivisibleBy 13u
            TestPositive = ThrowToMonkey 1u<MonkeyIndex>
            TestNegative = ThrowToMonkey 3u<MonkeyIndex>
        }
        {
            Index = 3u<MonkeyIndex>
            Items = [ 74u<WorryLevel> ]
            Operation = { Element1 = Old; Sign = Plus; Element2 = Number 3u }
            Test = DivisibleBy 17u
            TestPositive = ThrowToMonkey 0u<MonkeyIndex>
            TestNegative = ThrowToMonkey 1u<MonkeyIndex>
        }
    ]
    let result = ReadMonkeySetup Input
    Assert.Equivalent (expected, result)
    expected
    |> List.zip result
    |> List.map (fun expectedAndResult -> Assert.Equal (expectedAndResult |> fst, expectedAndResult |> snd))
    |> ignore

[<Fact>]
let ``Run first turn of the first round of monkey business`` () =
    let expected = [
        {
            Index = 0u<MonkeyIndex>
            Items = []
            Operation = { Element1 = Old; Sign = Multiply; Element2 = Number 19u }
            Test = DivisibleBy 23u
            TestPositive = ThrowToMonkey 2u<MonkeyIndex>
            TestNegative = ThrowToMonkey 3u<MonkeyIndex>
        }
        {
            Index = 1u<MonkeyIndex>
            Items = [ 54u<WorryLevel>; 65u<WorryLevel>; 75u<WorryLevel>; 74u<WorryLevel> ]
            Operation = { Element1 = Old; Sign = Plus; Element2 = Number 6u }
            Test = DivisibleBy 19u
            TestPositive = ThrowToMonkey 2u<MonkeyIndex>
            TestNegative = ThrowToMonkey 0u<MonkeyIndex>
        }
        {
            Index = 2u<MonkeyIndex>
            Items = [ 79u<WorryLevel>; 60u<WorryLevel>; 97u<WorryLevel> ]
            Operation = { Element1 = Old; Sign = Multiply; Element2 = Old }
            Test = DivisibleBy 13u
            TestPositive = ThrowToMonkey 1u<MonkeyIndex>
            TestNegative = ThrowToMonkey 3u<MonkeyIndex>
        }
        {
            Index = 3u<MonkeyIndex>
            Items = [ 74u<WorryLevel>; 500u<WorryLevel>; 620u<WorryLevel> ]
            Operation = { Element1 = Old; Sign = Plus; Element2 = Number 3u }
            Test = DivisibleBy 17u
            TestPositive = ThrowToMonkey 0u<MonkeyIndex>
            TestNegative = ThrowToMonkey 1u<MonkeyIndex>
        }
    ]
    let monkeySetup = ReadMonkeySetup Input
    let monkeyIndex = 0u<MonkeyIndex>
    let result = MonkeyBusinessSingleMonkeyTurn { Monkeys = monkeySetup; NumberOfInspections = [] } monkeyIndex
    let _, resultNumberOfInspections = result.NumberOfInspections |> List.find (fun (index, _) -> index = monkeyIndex);
    
    Assert.Equivalent (expected, result.Monkeys)
    Assert.Equal (2u, resultNumberOfInspections)
    expected
    |> List.zip result.Monkeys
    |> List.map Assert.Equal
    |> ignore

[<Fact>]
let ``Run first round of monkey business`` () =
    let expected = [
        {
            Index = 0u<MonkeyIndex>
            Items = [ 20u<WorryLevel>; 23u<WorryLevel>; 27u<WorryLevel>; 26u<WorryLevel>; ]
            Operation = { Element1 = Old; Sign = Multiply; Element2 = Number 19u }
            Test = DivisibleBy 23u
            TestPositive = ThrowToMonkey 2u<MonkeyIndex>
            TestNegative = ThrowToMonkey 3u<MonkeyIndex>
        }
        {
            Index = 1u<MonkeyIndex>
            Items = [ 2080u<WorryLevel>; 25u<WorryLevel>; 167u<WorryLevel>; 207u<WorryLevel>; 401u<WorryLevel>; 1046u<WorryLevel>; ]
            Operation = { Element1 = Old; Sign = Plus; Element2 = Number 6u }
            Test = DivisibleBy 19u
            TestPositive = ThrowToMonkey 2u<MonkeyIndex>
            TestNegative = ThrowToMonkey 0u<MonkeyIndex>
        }
        {
            Index = 2u<MonkeyIndex>
            Items = []
            Operation = { Element1 = Old; Sign = Multiply; Element2 = Old }
            Test = DivisibleBy 13u
            TestPositive = ThrowToMonkey 1u<MonkeyIndex>
            TestNegative = ThrowToMonkey 3u<MonkeyIndex>
        }
        {
            Index = 3u<MonkeyIndex>
            Items = []
            Operation = { Element1 = Old; Sign = Plus; Element2 = Number 3u }
            Test = DivisibleBy 17u
            TestPositive = ThrowToMonkey 0u<MonkeyIndex>
            TestNegative = ThrowToMonkey 1u<MonkeyIndex>
        }
    ]
    let expectedNumberOfInspections =
        [
            (0u<MonkeyIndex>, 2u)
            (1u<MonkeyIndex>, 4u)
            (2u<MonkeyIndex>, 3u)
            (3u<MonkeyIndex>, 5u)
        ]
    let monkeySetup = ReadMonkeySetup Input
    let result = MonkeyBusinessMonkeySetupRound { Monkeys = monkeySetup; NumberOfInspections = [] }
    
    Assert.Equivalent (expected, result.Monkeys)
    expected
    |> List.zip result.Monkeys
    |> List.map Assert.Equal
    |> ignore
    expectedNumberOfInspections
    |> List.zip result.NumberOfInspections
    |> List.map (fun ((expectedIndex, expectedNumber), (resultIndex, resultNumber)) ->
        Assert.Equal (expectedIndex, resultIndex)
        Assert.Equal (expectedNumber, resultNumber))

[<Fact>]
let ``Observing monkey business score`` () =
    let result = MonkeyBusinessScore Input
    Assert.Equivalent (10605u, result)