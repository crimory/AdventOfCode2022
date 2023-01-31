module Tests

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

let calculateWorryLevelAfterInspectionFirstPart input =
    input / 3UL

[<Fact>]
let ``Reading the monkey setup`` () =
    let expected = [
        {
            Index = 0u<MonkeyIndex>
            Items = [ 79UL<WorryLevel>; 98UL<WorryLevel> ]
            Operation = { Element1 = Old; Sign = Multiply; Element2 = Number 19UL }
            Test = DivisibleBy 23UL
            TestPositive = ThrowToMonkey 2u<MonkeyIndex>
            TestNegative = ThrowToMonkey 3u<MonkeyIndex>
        }
        {
            Index = 1u<MonkeyIndex>
            Items = [ 54UL<WorryLevel>; 65UL<WorryLevel>; 75UL<WorryLevel>; 74UL<WorryLevel> ]
            Operation = { Element1 = Old; Sign = Plus; Element2 = Number 6UL }
            Test = DivisibleBy 19UL
            TestPositive = ThrowToMonkey 2u<MonkeyIndex>
            TestNegative = ThrowToMonkey 0u<MonkeyIndex>
        }
        {
            Index = 2u<MonkeyIndex>
            Items = [ 79UL<WorryLevel>; 60UL<WorryLevel>; 97UL<WorryLevel> ]
            Operation = { Element1 = Old; Sign = Multiply; Element2 = Old }
            Test = DivisibleBy 13UL
            TestPositive = ThrowToMonkey 1u<MonkeyIndex>
            TestNegative = ThrowToMonkey 3u<MonkeyIndex>
        }
        {
            Index = 3u<MonkeyIndex>
            Items = [ 74UL<WorryLevel> ]
            Operation = { Element1 = Old; Sign = Plus; Element2 = Number 3UL }
            Test = DivisibleBy 17UL
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
            Operation = { Element1 = Old; Sign = Multiply; Element2 = Number 19UL }
            Test = DivisibleBy 23UL
            TestPositive = ThrowToMonkey 2u<MonkeyIndex>
            TestNegative = ThrowToMonkey 3u<MonkeyIndex>
        }
        {
            Index = 1u<MonkeyIndex>
            Items = [ 54UL<WorryLevel>; 65UL<WorryLevel>; 75UL<WorryLevel>; 74UL<WorryLevel> ]
            Operation = { Element1 = Old; Sign = Plus; Element2 = Number 6UL }
            Test = DivisibleBy 19UL
            TestPositive = ThrowToMonkey 2u<MonkeyIndex>
            TestNegative = ThrowToMonkey 0u<MonkeyIndex>
        }
        {
            Index = 2u<MonkeyIndex>
            Items = [ 79UL<WorryLevel>; 60UL<WorryLevel>; 97UL<WorryLevel> ]
            Operation = { Element1 = Old; Sign = Multiply; Element2 = Old }
            Test = DivisibleBy 13UL
            TestPositive = ThrowToMonkey 1u<MonkeyIndex>
            TestNegative = ThrowToMonkey 3u<MonkeyIndex>
        }
        {
            Index = 3u<MonkeyIndex>
            Items = [ 74UL<WorryLevel>; 500UL<WorryLevel>; 620UL<WorryLevel> ]
            Operation = { Element1 = Old; Sign = Plus; Element2 = Number 3UL }
            Test = DivisibleBy 17UL
            TestPositive = ThrowToMonkey 0u<MonkeyIndex>
            TestNegative = ThrowToMonkey 1u<MonkeyIndex>
        }
    ]
    let monkeySetup = ReadMonkeySetup Input
    let monkeyIndex = 0u<MonkeyIndex>
    let result = MonkeyBusinessSingleMonkeyTurn calculateWorryLevelAfterInspectionFirstPart { Monkeys = monkeySetup; NumberOfInspections = [] } monkeyIndex
    let _, resultNumberOfInspections = result.NumberOfInspections |> List.find (fun (index, _) -> index = monkeyIndex);
    
    Assert.Equivalent (expected, result.Monkeys)
    Assert.Equal (2UL, resultNumberOfInspections)
    expected
    |> List.zip result.Monkeys
    |> List.map Assert.Equal
    |> ignore

[<Fact>]
let ``Run first round of monkey business`` () =
    let expected = [
        {
            Index = 0u<MonkeyIndex>
            Items = [ 20UL<WorryLevel>; 23UL<WorryLevel>; 27UL<WorryLevel>; 26UL<WorryLevel>; ]
            Operation = { Element1 = Old; Sign = Multiply; Element2 = Number 19UL }
            Test = DivisibleBy 23UL
            TestPositive = ThrowToMonkey 2u<MonkeyIndex>
            TestNegative = ThrowToMonkey 3u<MonkeyIndex>
        }
        {
            Index = 1u<MonkeyIndex>
            Items = [ 2080UL<WorryLevel>; 25UL<WorryLevel>; 167UL<WorryLevel>; 207UL<WorryLevel>; 401UL<WorryLevel>; 1046UL<WorryLevel>; ]
            Operation = { Element1 = Old; Sign = Plus; Element2 = Number 6UL }
            Test = DivisibleBy 19UL
            TestPositive = ThrowToMonkey 2u<MonkeyIndex>
            TestNegative = ThrowToMonkey 0u<MonkeyIndex>
        }
        {
            Index = 2u<MonkeyIndex>
            Items = []
            Operation = { Element1 = Old; Sign = Multiply; Element2 = Old }
            Test = DivisibleBy 13UL
            TestPositive = ThrowToMonkey 1u<MonkeyIndex>
            TestNegative = ThrowToMonkey 3u<MonkeyIndex>
        }
        {
            Index = 3u<MonkeyIndex>
            Items = []
            Operation = { Element1 = Old; Sign = Plus; Element2 = Number 3UL }
            Test = DivisibleBy 17UL
            TestPositive = ThrowToMonkey 0u<MonkeyIndex>
            TestNegative = ThrowToMonkey 1u<MonkeyIndex>
        }
    ]
    let expectedNumberOfInspections =
        [
            (0u<MonkeyIndex>, 2UL)
            (1u<MonkeyIndex>, 4UL)
            (2u<MonkeyIndex>, 3UL)
            (3u<MonkeyIndex>, 5UL)
        ]
    let monkeySetup = ReadMonkeySetup Input
    let result = MonkeyBusinessMonkeySetupRound calculateWorryLevelAfterInspectionFirstPart { Monkeys = monkeySetup; NumberOfInspections = [] }
    
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
    let result = MonkeyBusinessScore 20 calculateWorryLevelAfterInspectionFirstPart Input
    Assert.Equivalent (10605UL, result)

[<Theory>]
[<InlineData(1, 24UL)>]
[<InlineData(20, 10_197UL)>]
[<InlineData(1_000, 27_019_168UL)>]
[<InlineData(2_000, 108_263_829UL)>]
[<InlineData(3_000, 243_843_334UL)>]
[<InlineData(4_000, 433_783_826UL)>]
[<InlineData(5_000, 677_950_000UL)>]
[<InlineData(6_000, 976_497_976UL)>]
[<InlineData(7_000, 1_328_891_200UL)>]
[<InlineData(8_000, 1_736_135_168UL)>]
[<InlineData(9_000, 2_197_354_615UL)>]
[<InlineData(10_000, 2_713_310_158UL)>]
let ``Observing monkey business score second part`` howManyRounds expectedResult =
    let result = MonkeyBusinessScore howManyRounds id Input
    Assert.Equivalent (expectedResult, result)