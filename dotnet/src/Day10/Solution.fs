namespace Day10

open System

module Solution =
    type internal Instruction = Noop | AddX of int
    let internal ReadInstructions (input: string) =
        let readLine line =
            match line with
            | "Noop" | "noop" -> Instruction.Noop
            | x ->
                let parts = x.Split " "
                match parts[0] with
                | "AddX" | "addx" -> Instruction.AddX (int parts[1])
                | _ -> Instruction.Noop
        input.Split Environment.NewLine
        |> Array.toList
        |> List.map readLine
    
    let internal BuildCycleOutput instructions =
        let cycleOutputPerInstruction acc instruction =
            match instruction with
            | Instruction.Noop -> [ acc ], acc
            | AddX x -> [ acc; (acc + x) ], acc + x
        let cycleOutputAfterFirst2 =
            instructions
            |> List.mapFold cycleOutputPerInstruction 1
            |> fst
            |> List.collect id
        [ 1; 1 ] @ cycleOutputAfterFirst2
    
    let SumSignalStrength cycleIndices input =
        let instructions = ReadInstructions input
        let cyclesOutput = BuildCycleOutput instructions
        cycleIndices
        |> List.map (fun cycleIndex -> cyclesOutput[cycleIndex] * cycleIndex)
        |> List.sum
    
    let GetCrtOutput input =
        let instructions = ReadInstructions input
        let cyclesOutput = BuildCycleOutput instructions
        let getPixel index x =
            let normalizedIndex = if index % 40 = 0 then 40 else index % 40
            let shouldLightThePixel =
                [ for i in 0..2 -> i + x ]
                |> List.contains normalizedIndex
            match shouldLightThePixel with
            | true -> "#"
            | false -> "."
        let getLine indicesAndX =
            indicesAndX
            |> List.map (fun (index, x) -> getPixel index x)
            |> String.concat ""
        cyclesOutput
        |> List.indexed
        |> List.skip 1
        |> List.take 240
        |> List.splitInto 6
        |> List.map getLine