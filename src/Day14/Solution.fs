module Day14.Solution

open System

type Point = { X: int; Y: int }
type Line = { Start: Point; End: Point }
type SandStartingPoint = { Position: Point }
type SandState = Falling | Settled
type SandUnit = { Position: Point; State: SandState }

let private GetLinePoints startAnchor endAnchor =
    match startAnchor.X, startAnchor.Y, endAnchor.X, endAnchor.Y with
    | x1, y1, x2, y2 when x1 = x2 ->
        let yRange =
            match y1, y2 with
            | y1, y2 when y1 < y2 -> [ y1..y2 ]
            | y1, y2 when y1 > y2 -> [ y2..y1 ]
            | _ -> [ y1 ]
        yRange
        |> List.map (fun y -> { X = x1; Y = y })
    | x1, y1, x2, y2 when y1 = y2 ->
        let xRange =
            match x1, x2 with
            | x1, x2 when x1 < x2 -> [ x1..x2 ]
            | x1, x2 when x1 > x2 -> [ x2..x1 ]
            | _ -> [ x1 ]
        xRange
        |> List.map (fun x -> { X = x; Y = y1 })
    | _ ->
        failwith "Invalid line"

let private ReadLine (line: string) =
    let anchorPoints =
        line.Split " -> "
        |> Array.map (fun s ->
            let parts = s.Split ","
            { X = parts[0] |> int; Y = parts[1] |> int })
    anchorPoints
    |> Array.pairwise
    |> Array.map (fun (startAnchor, endAnchor) ->
        GetLinePoints startAnchor endAnchor)
    |> Array.toList
    |> List.concat

let internal ReadInput (input: string) =
    input.Split Environment.NewLine
    |> Array.toList
    |> List.map ReadLine
    |> List.concat

let internal GetSandUnitNextState (map: Point list) (sandUnit: SandUnit) =
    match sandUnit.State with
    | SandState.Settled -> sandUnit
    | SandState.Falling ->
        let nextPositionBelow = { X = sandUnit.Position.X; Y = sandUnit.Position.Y + 1 }
        let nextPositionLeft = { X = sandUnit.Position.X - 1; Y = sandUnit.Position.Y + 1 }
        let nextPositionRight = { X = sandUnit.Position.X + 1; Y = sandUnit.Position.Y + 1 }
        
        let isPositionBelowTaken = map |> List.contains nextPositionBelow
        let isPositionLeftTaken = map |> List.contains nextPositionLeft
        let isPositionRightTaken = map |> List.contains nextPositionRight
        
        match isPositionBelowTaken, isPositionLeftTaken, isPositionRightTaken with
        | true, true, true -> { sandUnit with State = SandState.Settled }
        | false, _, _ -> { Position = nextPositionBelow; State = SandState.Falling }
        | true, false, _ -> { Position = nextPositionLeft; State = SandState.Falling }
        | true, true, false -> { Position = nextPositionRight; State = SandState.Falling }

let GetNumberOfSandUnitsThatSettle (input: string) =
    let sandStartingUnit = { Position = { X = 500; Y = 0 }; State = SandState.Falling }
    let mutable map = ReadInput input
    let mapMaximumY = map |> List.map (fun p -> p.Y) |> List.max
    let mutable sandUnit = sandStartingUnit
    let mutable sandUnitsCount = 0
    while sandUnit.State = SandState.Settled || sandUnitsCount = 0 do
        sandUnit <-
            [ 1..mapMaximumY ]
            |> List.fold (fun acc _ ->
                GetSandUnitNextState map acc
                ) sandStartingUnit
        match sandUnit.State with
        | SandState.Settled ->
            sandUnitsCount <- sandUnitsCount + 1
            map <- map @ [ sandUnit.Position ]
        | SandState.Falling ->
            ()
    sandUnitsCount

let GetNumberOfSandUnitsThatSettleWithFloor (input: string) =
    let sandStartingUnit = { Position = { X = 500; Y = 0 }; State = SandState.Falling }
    
    let mutable map = ReadInput input
    let mapMaximumY = map |> List.map (fun p -> p.Y) |> List.max
    let farLeftFloorPoint = { X = sandStartingUnit.Position.X - mapMaximumY - 2; Y = mapMaximumY + 2 }
    let farRightFloorPoint = { X = sandStartingUnit.Position.X + mapMaximumY + 2; Y = mapMaximumY + 2 }
    let floorPoints =
        GetLinePoints farLeftFloorPoint farRightFloorPoint
    map <- map @ floorPoints
    
    let mutable sandUnit = sandStartingUnit
    let mutable sandUnitsCount = 0
    while sandUnit <> { sandStartingUnit with State = SandState.Settled } || sandUnitsCount = 0 do
        sandUnit <-
            [ 1..(mapMaximumY + 2) ]
            |> List.fold (fun acc _ ->
                GetSandUnitNextState map acc
                ) sandStartingUnit
        match sandUnit.State with
        | SandState.Settled ->
            sandUnitsCount <- sandUnitsCount + 1
            map <- map @ [ sandUnit.Position ]
        | SandState.Falling ->
            ()
    sandUnitsCount
