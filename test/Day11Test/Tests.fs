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

[<Fact>]
let ``Run first step of the first round of monkey business`` () =
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
    let result = MonkeyBusinessSingleMonkeyTurns 0u<MonkeyIndex> monkeySetup
    Assert.Equivalent (expected, result)

[<Fact>]
let ``Observing monkey business score`` () =
    let result = MonkeyBusinessScore Input
    Assert.Equivalent (10605, result)