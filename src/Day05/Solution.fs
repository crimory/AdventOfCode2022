namespace Day05
module Solution =
    type Crate = Label of char
    type StackOfCrates = { Index: int; Crates: Crate list }
    type Ship = Stacks of StackOfCrates list
    
    type Instruction = { HowManyCrates: int; FromStackIndex: int; ToStackIndex: int }
    
    let internal separateShipFromCranePlan (text: string) =
        let list = text.Split "\n\n"
        (list[0], list[1])
        
    let internal readShipPlan (text: string) =
        let lines =
            text.Split "\n"
            |> Array.toList
        
        let stackIndices =
            lines
            |> List.last
            |> Seq.chunkBySize 4
            |> Seq.toList
            |> List.map (fun x -> x[1])
            |> List.map string
            |> List.map int
        
        lines
        |> List.take (lines.Length - 1)
        |> List.map (fun x -> x |> Seq.chunkBySize 4 |> Seq.toList)
        |> List.map (fun x -> x |> List.map (fun y -> y |> Seq.item 1))
        |> List.transpose
        |> List.map (fun x -> x |> List.filter (fun y -> y <> ' '))
        |> List.map (fun x -> x |> List.map Label)
        |> List.zip stackIndices
        |> List.map (fun (index, list) -> { Index = index; Crates = list })
        |> Stacks
    
    let internal readCraneInstructions (text: string) =
        text.Split "\n"
        |> Array.map (fun x -> x.Split " ")
        |> Array.map (fun x -> { HowManyCrates = x[1] |> int; FromStackIndex = x[3] |> int; ToStackIndex = x[5] |> int })
        |> Array.toList
    
    let private performSingleInstruction instruction ship returnNewStackOfCrates =
        let (Stacks stackOfCratesList) = ship
        let matchingStack =
            stackOfCratesList
            |> List.find (fun x -> x.Index = instruction.FromStackIndex)
        let retrievalSublist =
            matchingStack.Crates
            |> List.take instruction.HowManyCrates
        
        stackOfCratesList
        |> List.map (fun x -> x |> returnNewStackOfCrates instruction retrievalSublist)
        |> Stacks
    
    let private returnNewStackOfCrates instruction retrievalSublist oldStack =
        match oldStack.Index, instruction.FromStackIndex, instruction.ToStackIndex, oldStack.Crates, instruction.HowManyCrates with
        | index, indexFrom, _, crates, howMany when index = indexFrom ->
            { Index = index; Crates = crates |> List.skip howMany }
        | index, _, indexTo, crates, _ when index = indexTo ->
            { Index = index; Crates = retrievalSublist @ crates }
        | index, _, _, crates, _ -> { Index = index; Crates = crates }
        
    let internal performSingleInstructionCraneMover9000 instruction ship =
        let returnNewStackOfCratesCraneMover9000 instruction retrievalSublist oldStack =
            returnNewStackOfCrates instruction (retrievalSublist |> List.rev) oldStack
        performSingleInstruction instruction ship returnNewStackOfCratesCraneMover9000
    
    let internal performSingleInstructionCraneMover9001 instruction ship =
        performSingleInstruction instruction ship returnNewStackOfCrates

    let private processCraneMoverPlan text performSingleInstruction =
        let shipPlan, cranePlan = text |> separateShipFromCranePlan
        let ship = shipPlan |> readShipPlan
        let instructions = cranePlan |> readCraneInstructions
        
        let finalShipState =
            instructions
            |> List.fold (fun acc x -> performSingleInstruction x acc ) ship
        
        let (Stacks finalStacks) = finalShipState
        let finalTopCrateLabels =
            finalStacks
            |> List.map (fun x -> x.Crates |> List.head)
            |> List.map (fun x ->
                let (Label label) = x
                label
                )
            |> List.toArray
        
        finalTopCrateLabels |> System.String.Concat
    
    let processCraneMover9000Plan text =
        performSingleInstructionCraneMover9000
        |> processCraneMoverPlan text
        
    let processCraneMover9001Plan text =
        performSingleInstructionCraneMover9001
        |> processCraneMoverPlan text