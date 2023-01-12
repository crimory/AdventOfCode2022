module Day12.SolutionBackwards

open Solution
open System

let private isPreviousPointHeightAcceptable currentPoint previousPoint =
    let currentHeight = currentPoint |> currentPointHeight
    let previousHeight = previousPoint |> currentPointHeight
    match currentHeight, previousHeight with
    | current, previous when current <= previous -> true
    | current, previous when current - previous = 1 -> true
    | _ -> false

let private getPossiblePreviousPoints map currentPoint =
    let arePreviousPointCoordinatesAcceptable currentCoordinates previousCoordinates =
        match currentCoordinates, previousCoordinates with
        | { X = cx; Y = cy }, { X = px; Y = py } when cy = py -> (cx - px) |> Math.Abs = 1
        | { X = cx; Y = cy }, { X = px; Y = py } when cx = px -> (cy - py) |> Math.Abs = 1
        | _ -> false
    map
    |> List.filter (fun x -> x.Coordinates |> arePreviousPointCoordinatesAcceptable currentPoint.Coordinates)

let internal GetAcceptablePreviousPoints map currentPoint =
    let previousPoints =
        getPossiblePreviousPoints map currentPoint
    match previousPoints with
    | a when a |> List.exists (fun x -> x.Height = Start) -> [ a |> List.find (fun x -> x.Height = Start) ]
    | a -> a |> List.filter (fun x -> x |> isPreviousPointHeightAcceptable currentPoint)

let private getBranch map (nextPoints: MapPoint list) =
    let acceptableNewPoints =
        GetAcceptablePreviousPoints map nextPoints.Head
        |> List.filter (fun x -> nextPoints.Tail |> List.contains x |> not)
    acceptableNewPoints
    |> List.map (fun x -> [x] @ nextPoints)

let rec internal GetFullBranchesFromEnd map (nextPaths: MapPoint list list) =
    let pathsToRunAlgorithmFor, completedPaths =
        nextPaths
        |> List.partition (fun x -> (x |> List.head).Height <> Start )
    match completedPaths with
    | [] ->
        pathsToRunAlgorithmFor
        |> List.map (fun x -> x |> getBranch map)
        |> List.collect id
        |> List.distinct
        |> GetFullBranchesFromEnd map
    | completed -> completed

let GetShortestRouteNumberOfSteps input =
    let map = ReadInput input
    GetFullBranchesFromEnd map [[ map |> List.find (fun x -> x.Height = End) ]]
    |> List.map (fun x -> (x |> List.length) - 1)
    |> List.min