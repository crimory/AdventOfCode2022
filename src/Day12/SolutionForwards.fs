module Day12.SolutionForwards

open Solution
open System

let private areNextPointCoordinatesAcceptable currentCoordinates previousCoordinates =
    match currentCoordinates, previousCoordinates with
        | { X = cx; Y = cy }, { X = px; Y = py } when cy = py -> (cx - px) |> Math.Abs = 1
        | { X = cx; Y = cy }, { X = px; Y = py } when cx = px -> (cy - py) |> Math.Abs = 1
        | _ -> false

let internal GetPossibleNextPoints map currentPoint =
    map
    |> List.filter (fun x -> x.Coordinates |> areNextPointCoordinatesAcceptable currentPoint.Coordinates)

let internal IsNextPointHeightAcceptable currentPoint nextPoint =
    let currentHeight = currentPoint |> currentPointHeight
    let nextHeight = nextPoint |> currentPointHeight
    match currentHeight, nextHeight with
    | current, next when (next - current) <= 1 -> true
    | _ -> false

let internal GetNextAcceptablePoints map currentPoint =
    GetPossibleNextPoints map currentPoint
    |> List.filter (fun x -> x |> IsNextPointHeightAcceptable currentPoint)

let internal GetNextStepBranches map (previousPoints: MapPoint list) =
    let currentPoint = previousPoints |> List.last
    let acceptableNewPoints =
        GetNextAcceptablePoints map currentPoint
        |> List.filter (fun x -> previousPoints |> List.contains x |> not)
    acceptableNewPoints
    |> List.map (fun x -> previousPoints @ [x])

let rec internal GetFullBranches map (initialPreviousPaths: MapPoint list list) =
    let previousPaths =
        match initialPreviousPaths with
        | [] -> [ [ map |> List.find (fun x -> x.Height = Start) ] ]
        | a -> a
    let completePaths, incompletePaths =
        previousPaths
        |> List.partition (fun x -> (x |> List.last).Height = End )
    match completePaths with
    | [] ->
        incompletePaths
        |> List.map (GetNextStepBranches map)
        |> List.collect id
        |> GetFullBranches map
    | completed -> completed

let GetShortestRouteNumberOfSteps input =
    let map = ReadInput input
    GetFullBranches map []
    |> List.map (fun x -> x.Length - 1)
    |> List.min