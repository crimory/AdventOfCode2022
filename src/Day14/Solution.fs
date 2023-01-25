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
            | y1, y2 when y1 < y2 -> [| y1..y2 |]
            | y1, y2 when y1 > y2 -> [| y2..y1 |]
            | _ -> [| y1 |]
        yRange
        |> Array.map (fun y -> { X = x1; Y = y })
    | x1, y1, x2, y2 when y1 = y2 ->
        let xRange =
            match x1, x2 with
            | x1, x2 when x1 < x2 -> [| x1..x2 |]
            | x1, x2 when x1 > x2 -> [| x2..x1 |]
            | _ -> [| x1 |]
        xRange
        |> Array.map (fun x -> { X = x; Y = y1 })
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
    |> Array.concat

let internal ReadInput (input: string) =
    input.Split Environment.NewLine
    |> Array.map ReadLine
    |> Array.concat

let private GetSandUnitNextState (map: bool[,]) (sandUnit: SandUnit) =
    match sandUnit.State with
    | SandState.Settled -> sandUnit
    | SandState.Falling ->
        let nextPositionBelow = { X = sandUnit.Position.X; Y = sandUnit.Position.Y + 1 }
        let nextPositionLeft = { X = sandUnit.Position.X - 1; Y = sandUnit.Position.Y + 1 }
        let nextPositionRight = { X = sandUnit.Position.X + 1; Y = sandUnit.Position.Y + 1 }
        
        match map.GetLength 0, map.GetLength 1 with
        | x, y when nextPositionBelow.X >= x || nextPositionBelow.Y >= y ->
            { sandUnit with Position = nextPositionBelow }
        | _ ->
            let isPositionBelowTaken = map[nextPositionBelow.X, nextPositionBelow.Y]
            let isPositionLeftTaken = map[nextPositionLeft.X, nextPositionLeft.Y]
            let isPositionRightTaken = map[nextPositionRight.X, nextPositionRight.Y]
            
            match isPositionBelowTaken, isPositionLeftTaken, isPositionRightTaken with
            | true, true, true -> { sandUnit with State = SandState.Settled }
            | false, _, _ -> { Position = nextPositionBelow; State = SandState.Falling }
            | true, false, _ -> { Position = nextPositionLeft; State = SandState.Falling }
            | true, true, false -> { Position = nextPositionRight; State = SandState.Falling }

let private GetProperMap (inputMap: Point[]) =
    let mapMaximumY = inputMap |> Array.map (fun p -> p.Y) |> Array.max
    let mapMaximumX = inputMap |> Array.map (fun p -> p.X) |> Array.max
    let map = Array2D.create (mapMaximumX+1) (mapMaximumY+1) false
    inputMap |> Array.iter (fun p -> map[p.X, p.Y] <- true)
    map

let private CountSandUnitsThatSettled (getEndCondition: SandUnit -> bool) (sandStartingUnit: SandUnit) (map: bool[,]) =
    let mutable sandUnit = sandStartingUnit
    let mutable sandUnitsCount = 0
    while getEndCondition sandUnit || sandUnitsCount = 0 do
        sandUnit <- sandStartingUnit
        while sandUnit.State = SandState.Falling && sandUnit.Position.Y <= ((map.GetLength 1) - 1) do
            sandUnit <- GetSandUnitNextState map sandUnit
        match sandUnit.State with
        | SandState.Settled ->
            sandUnitsCount <- sandUnitsCount + 1
            map[sandUnit.Position.X, sandUnit.Position.Y] <- true
        | SandState.Falling ->
            ()
    sandUnitsCount

let GetNumberOfSandUnitsThatSettle (input: string) =
    let sandStartingUnit = { Position = { X = 500; Y = 0 }; State = SandState.Falling }
    let mapRaw = ReadInput input
    let map = GetProperMap mapRaw
    CountSandUnitsThatSettled (fun sandUnit -> sandUnit.State = SandState.Settled) sandStartingUnit map

let GetNumberOfSandUnitsThatSettleWithFloor (input: string) =
    let sandStartingUnit = { Position = { X = 500; Y = 0 }; State = SandState.Falling }
    
    let mapRaw = ReadInput input
    let mapMaximumY = mapRaw |> Array.map (fun p -> p.Y) |> Array.max
    let farLeftFloorPoint = { X = sandStartingUnit.Position.X - mapMaximumY - 2; Y = mapMaximumY + 2 }
    let farRightFloorPoint = { X = sandStartingUnit.Position.X + mapMaximumY + 2; Y = mapMaximumY + 2 }
    let floorPoints =
        GetLinePoints farLeftFloorPoint farRightFloorPoint
    let map = GetProperMap (Array.concat [ mapRaw; floorPoints ])
    
    CountSandUnitsThatSettled (fun sandUnit -> sandUnit <> { sandStartingUnit with State = SandState.Settled }) sandStartingUnit map
