module Day03.Solution

let splitLineInHalf (line: string) =
    let purifiedLine = line.Trim ()
    let halfLength = purifiedLine.Length / 2
    let firstHalf = purifiedLine.Substring (0, halfLength)
    let secondHalf = purifiedLine.Substring (halfLength, halfLength)
    (firstHalf, secondHalf)

let findRepetitionItem text =
    let firstCompartment, secondCompartment = splitLineInHalf text
    firstCompartment
    |> String.filter secondCompartment.Contains
    |> Seq.head

let priorityValues =
        [ 1 .. 52 ]
        |> List.zip ([ 'a' .. 'z' ] @ [ 'A' .. 'Z' ])
        |> dict

let getPrioritySum (text: string) =
    text.Split '\n'
    |> Array.map findRepetitionItem
    |> Array.map (fun c -> priorityValues[c])
    |> Array.sum

let getBadge (text: string) =
    let foldingForCommonLetter accumulator line =
        match accumulator, line with
        | "", _ -> line
        | acc, lineText ->
            acc |> String.filter lineText.Contains
    text.Split '\n'
    |> Array.fold foldingForCommonLetter ""
    |> Seq.head
    
let splitTextIntoElfGroups elfGroupSize (text: string) =
    text.Split '\n'
    |> Array.map (fun x -> x.Trim ())
    |> Array.chunkBySize elfGroupSize
    |> Array.map (fun x -> x |> String.concat "\n")

let getPrioritySumPerElfBadge (text: string) =
    text
    |> splitTextIntoElfGroups 3
    |> Array.map getBadge
    |> Array.map (fun c -> priorityValues[c])
    |> Array.sum