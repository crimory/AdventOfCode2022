namespace Day11

open System

module Solution =
    type OperationElement = Old | Number of uint
    type OperationSign = Plus | Minus | Divide | Multiply
    type Operation = {
        Element1: OperationElement
        Sign: OperationSign
        Element2: OperationElement
    }
    type MonkeyTest = DivisibleBy of uint
    [<Measure>] type MonkeyIndex
    [<Measure>] type WorryLevel
    type MonkeyTestOutcome = ThrowToMonkey of uint<MonkeyIndex>
    type Monkey = {
        Index: uint<MonkeyIndex>
        Items: uint<WorryLevel> list
        Operation: Operation
        Test: MonkeyTest
        TestPositive: MonkeyTestOutcome
        TestNegative: MonkeyTestOutcome
    }
    type MonkeyTurnOutput = {
        Monkeys: Monkey list
        NumberOfInspections: (uint<MonkeyIndex> * uint) list
    }
    let internal ReadMonkeySetup (input: string) =
        let defaultMonkey =
            {
                Index = 0u<MonkeyIndex>
                Items = []
                Operation = { Element1 = Old; Sign = Plus; Element2 = Old }
                Test = DivisibleBy 1u
                TestPositive = ThrowToMonkey 0u<MonkeyIndex>
                TestNegative = ThrowToMonkey 0u<MonkeyIndex>
            }
        let processMonkeyLine (singleMonkeyLine: string) (monkeyAccumulator: Monkey) =
            let processLineFifthAndSix (text: string) =
                let textParts = text.Split ": "
                match textParts[1] with
                | correct when correct.StartsWith "throw to monkey" ->
                    let parts = correct.Split " "
                    let monkeyIndex =
                        parts[3]
                        |> uint
                    monkeyIndex * 1u<MonkeyIndex>
                    |> ThrowToMonkey
                | _ -> failwith "unknown test outcome"
            match singleMonkeyLine with
            | firstLine when firstLine.StartsWith "Monkey " ->
                let parts = firstLine.Split " "
                let monkeyIndex =
                    parts[1].TrimEnd ':'
                    |> uint
                { monkeyAccumulator with Index = monkeyIndex * 1u<MonkeyIndex> }
            | secondLine when secondLine.StartsWith "Starting items: " ->
                let parts = secondLine.Split ": "
                let items =
                    parts[1].Split ", "
                    |> Array.map uint
                    |> Array.map (fun x -> x * 1u<WorryLevel>)
                    |> Array.toList
                { monkeyAccumulator with Items = items }
            | thirdLine when thirdLine.StartsWith "Operation: " ->
                let parts = thirdLine.Split " = "
                let operationNewParts = parts[1].Split " "
                let oldOrNumber text =
                    match text with
                    | "old" -> Old
                    | x -> x |> uint |> Number
                let readSign text =
                    match text with
                    | "+" -> Plus
                    | "-" -> Minus
                    | "/" -> Divide
                    | "*" -> Multiply
                    | _ -> failwith "unknown sign"
                let firstElement = oldOrNumber operationNewParts[0]
                let operationSign = readSign operationNewParts[1]
                let secondElement = oldOrNumber operationNewParts[2]
                { monkeyAccumulator with Operation = { Element1 = firstElement; Sign = operationSign; Element2 = secondElement } }
            | forthLine when forthLine.StartsWith "Test: " ->
                let parts = forthLine.Split ": "
                let testPart =
                    match parts[1] with
                    | testText when testText.StartsWith "divisible by" ->
                        let parts = testText.Split " "
                        parts[2] |> uint |> DivisibleBy
                    | _ -> failwith "unknown test"
                { monkeyAccumulator with Test = testPart }
            | fifthLine when fifthLine.StartsWith "If true:" ->
                let testOutput = processLineFifthAndSix fifthLine
                { monkeyAccumulator with TestPositive = testOutput }
            | sixthLine when sixthLine.StartsWith "If false:" ->
                let testOutput = processLineFifthAndSix sixthLine
                { monkeyAccumulator with TestNegative = testOutput }
            | _ -> monkeyAccumulator
        let processMonkey (singleMonkeyInput: string) =
            singleMonkeyInput.Split Environment.NewLine
            |> Array.map (fun x -> x.Trim ())
            |> Array.fold (fun acc x -> processMonkeyLine x acc) defaultMonkey
        input.Split $"{Environment.NewLine}{Environment.NewLine}"
        |> Array.map processMonkey
        |> Array.toList
    
    let private getSpecificMonkey (monkeys: Monkey list) index =
        monkeys
        |> List.find (fun x -> x.Index = index)
    
    let private singleMonkeyBusinessTurn monkeyIndex (monkeys: Monkey list) (item: uint<WorryLevel>) =
        let currentMonkey = (monkeyIndex |> getSpecificMonkey monkeys)
        let worryLevelDuringInspection =
            let getElement oldValue element =
                match element with
                | Old -> oldValue |> uint
                | Number x -> x
            let element1 = getElement item currentMonkey.Operation.Element1
            let element2 = getElement item currentMonkey.Operation.Element2
            match currentMonkey.Operation.Sign with
            | Plus -> element1 + element2
            | Minus -> element1 - element2
            | Divide -> element1 / element2
            | Multiply -> element1 * element2
        let worryLevelAfterInspection = worryLevelDuringInspection / 3u
        let testOutcome =
            match currentMonkey.Test with
            | DivisibleBy a -> worryLevelAfterInspection % a = 0u
        let monkeyIndexToThrowItemTo =
            match testOutcome with
            | true ->
                match currentMonkey.TestPositive with
                | ThrowToMonkey a -> a
            | false ->
                match currentMonkey.TestNegative with
                | ThrowToMonkey a -> a
        let monkeyToThrowTo = monkeyIndexToThrowItemTo |> getSpecificMonkey monkeys
        let indicesOfMonkeysToChange = [ monkeyIndex; monkeyIndexToThrowItemTo ]
        [ { currentMonkey with Items = currentMonkey.Items |> List.filter (fun x -> x <> item) } ]
        @ [ { monkeyToThrowTo with Items = monkeyToThrowTo.Items @ [ worryLevelAfterInspection * 1u<WorryLevel> ] } ]
        @ (monkeys |> List.filter (fun x -> indicesOfMonkeysToChange |> List.contains x.Index |> not))
        |> List.sortBy (fun x -> x.Index)
    
    let internal MonkeyBusinessSingleMonkeyTurn (monkeys: MonkeyTurnOutput) (monkeyIndex: uint<MonkeyIndex>) =
        let currentMonkey = (monkeyIndex |> getSpecificMonkey monkeys.Monkeys)
        let singleMonkeyBusinessTurnForCurrentIndex = singleMonkeyBusinessTurn monkeyIndex
        let newMonkeys =
            currentMonkey.Items
            |> List.fold singleMonkeyBusinessTurnForCurrentIndex monkeys.Monkeys
        let previousNumberOfInspectionsRecord = monkeys.NumberOfInspections |> List.tryFind (fun (index, number) -> index = monkeyIndex)
        let previousNumberOfInspections =
            match previousNumberOfInspectionsRecord with
            | None -> 0u
            | Some (_, number) -> number
        let currentNumberOfInspections = currentMonkey.Items |> List.length |> uint
        let otherInspections = monkeys.NumberOfInspections |> List.filter (fun (index, _) -> index <> monkeyIndex)
        { Monkeys = newMonkeys; NumberOfInspections = otherInspections @ [ (monkeyIndex, previousNumberOfInspections + currentNumberOfInspections) ] }
    
    let internal MonkeyBusinessMonkeySetupRound (monkeys: MonkeyTurnOutput) =
        monkeys.Monkeys
        |> List.map (fun x -> x.Index)
        |> List.fold MonkeyBusinessSingleMonkeyTurn monkeys
    
    let MonkeyBusinessScore (input: string) =
        let monkeyBusinessRounds = 20
        let monkeys = ReadMonkeySetup input
        let monkeyOutput =
            [ 1 .. monkeyBusinessRounds ]
            |> List.fold (fun acc _ -> MonkeyBusinessMonkeySetupRound acc) { Monkeys = monkeys; NumberOfInspections = [] }
        let mostActiveMonkeysInspections =
            monkeyOutput.NumberOfInspections
            |> List.map snd
            |> List.sortDescending
            |> List.take 2
        mostActiveMonkeysInspections[0] * mostActiveMonkeysInspections[1]