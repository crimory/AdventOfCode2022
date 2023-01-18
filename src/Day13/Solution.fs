module Day13.Solution

open System

type PacketPart = ListOfValues of PacketPart list | Value of int
type PacketPair = { LeftItems: PacketPart; RightItems: PacketPart }
type internal FoldAccumulator = { ValueIndex: int; BracketCounter: int }
type ComparisonResult = CorrectOrder | IncorrectOrder | NotDecided

let private InnerDivide (line: string) =
    line
    |> Seq.mapFold (fun acc c ->
        match c with
        | '[' ->
            (acc.ValueIndex, c),
            { acc with BracketCounter = acc.BracketCounter + 1 }
        | ']' ->
            (acc.ValueIndex, c),
            { acc with BracketCounter = acc.BracketCounter - 1 }
        | ',' when acc.BracketCounter = 0 ->
            ((if acc.BracketCounter = 0 then -1 else acc.ValueIndex), c),
            { acc with ValueIndex = acc.ValueIndex + 1 }
        | _ ->
            (acc.ValueIndex, c),
            acc
        ) { ValueIndex = 0; BracketCounter = 0 }
    |> fst
    |> Seq.groupBy fst
    |> Seq.map (fun (i, l) -> i, l |> Seq.map snd |> String.Concat)
    |> Seq.filter (fun (i, _) -> i >= 0)
    |> Seq.map snd
    |> Seq.toList

let internal DivideString (input: string) =
    let inputDivided = InnerDivide input
    match inputDivided.Length = 1 && (inputDivided |> List.last) = input with
    | true -> input.Substring(1, input.Length - 2) |> InnerDivide
    | false -> inputDivided

let rec internal ReadPacketPart (input: string) =
    match input with
    | "[]" -> ListOfValues []
    | a when (a.Contains '[' || a.Contains ']' || a.Contains ',') |> not ->
        Value (int a)
    | a ->
        a
        |> DivideString
        |> List.map ReadPacketPart
        |> ListOfValues

let private ReadPacketPair (input: string) =
    let parts =
        input.Split Environment.NewLine
        |> Array.map (fun x -> x |> ReadPacketPart)
    { LeftItems = parts[0]; RightItems = parts[1] }

let internal ReadInput (input: string) =
    input.Split $"{Environment.NewLine}{Environment.NewLine}"
    |> Array.map (fun x -> x |> ReadPacketPair)
    |> Array.toList

let rec internal ComparePacketOrder (left: PacketPart) (right: PacketPart) =
    match left, right with
    | Value a, Value b when a < b -> CorrectOrder
    | Value a, Value b when a > b -> IncorrectOrder
    | Value _, Value _ -> NotDecided
    | ListOfValues l, ListOfValues r when l.Length = r.Length ->
        r |> List.zip l
        |> List.fold (fun acc (x, y) ->
            match acc, x, y with
            | CorrectOrder, _, _ -> CorrectOrder
            | IncorrectOrder, _, _ -> IncorrectOrder
            | NotDecided, i, j -> ComparePacketOrder i j
            ) NotDecided
    | ListOfValues l, ListOfValues r when l.Length < r.Length ->
        let newRight = ListOfValues (r |> List.take l.Length)
        let currentOutput = ComparePacketOrder left newRight
        match currentOutput with
        | CorrectOrder | NotDecided -> CorrectOrder
        | IncorrectOrder -> IncorrectOrder
    | ListOfValues l, ListOfValues r when l.Length > r.Length ->
        let newLeft = ListOfValues (l |> List.take r.Length)
        let currentOutput = ComparePacketOrder newLeft right
        match currentOutput with
        | CorrectOrder -> CorrectOrder
        | IncorrectOrder | NotDecided -> IncorrectOrder
    | ListOfValues l, Value r ->
        ComparePacketOrder (ListOfValues l) (ListOfValues [Value r])
    | Value l, ListOfValues r ->
        ComparePacketOrder (ListOfValues [Value l]) (ListOfValues r)
    | _ -> IncorrectOrder

let GetSumOfPacketIndicesInCorrectOrder (input: string) =
    input
    |> ReadInput
    |> List.map (fun x -> ComparePacketOrder x.LeftItems x.RightItems)
    |> List.indexed
    |> List.map (fun (i, x) -> i + 1, x)
    |> List.filter (fun (_, x) -> x = CorrectOrder)
    |> List.sumBy fst

let private MappingPairwiseForPacketOrder left right newPacket newPacketAlreadyPlaced =
    match newPacketAlreadyPlaced with
    | true -> [ left ], true
    | false ->
        let newInMiddleLeft = ComparePacketOrder left newPacket
        let newInMiddleRight = ComparePacketOrder newPacket right
        match newInMiddleLeft, newInMiddleRight with
        | CorrectOrder, CorrectOrder ->
            [ left; newPacket ], true
        | _ ->
            [ left ], false

let private PutNewPacketInOrderWithCurrentOrder currentOrder newPacket =
    let outputBeforeFlattening, newPacketAlreadyPlaced =
        currentOrder
        |> List.pairwise
        |> List.mapFold (fun acc (left, right) ->
            MappingPairwiseForPacketOrder left right newPacket acc)
            false
    let outputFlat = (outputBeforeFlattening |> List.concat) @ [ currentOrder |> List.last ]
    match newPacketAlreadyPlaced with
    | true -> outputFlat
    | false -> outputFlat @ [ newPacket ]

let private PutNewPacketInOrderWithRest currentOrder newPacket =
    match currentOrder with
    | [] -> [ newPacket ]
    | [ a ] ->
        match ComparePacketOrder a newPacket with
        | CorrectOrder | NotDecided -> [ a; newPacket ]
        | IncorrectOrder -> [ newPacket; a ]
    | a ->
        match (ComparePacketOrder newPacket a.Head) with
        | CorrectOrder -> newPacket :: a
        | _ -> PutNewPacketInOrderWithCurrentOrder a newPacket

let GetDecoderKey (dividerPackets: PacketPart list) (input: string) =
    let inputPackets =
        input
        |> ReadInput
        |> List.map (fun x -> [ x.LeftItems; x.RightItems ])
        |> List.concat
    dividerPackets @ inputPackets
    |> List.fold PutNewPacketInOrderWithRest []
    |> List.indexed
    |> List.map (fun (i, x) -> i + 1, x)
    |> List.filter (fun (_, x) -> dividerPackets |> List.contains x)
    |> List.map fst
    |> List.reduce (*)