module Day12.SolutionForwards

open Day12.Solution
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

let private TakeMaximumOf maxLength list =
    list
    |> List.take (if list.Length > maxLength then maxLength else list.Length)

let rec internal GetFullBranches map (initialPointsVisited: MapPoint list) (initialPreviousPaths: MapPoint list list) =
    let pointsVisited =
        match initialPointsVisited with
        | [] -> [ map |> List.find (fun x -> x.Height = Start) ]
        | a -> a
    let previousPaths =
        match initialPreviousPaths with
        | [] -> [ [ map |> List.find (fun x -> x.Height = Start) ] ]
        | a -> a
    let completedPaths, incompletePaths =
        previousPaths
        |> List.partition (fun x -> (x |> List.last).Height = End )
    match completedPaths with
    | [] ->
        let potentialNextPaths =
            incompletePaths
            |> List.map (GetNextStepBranches map)
            |> List.collect id
        let nextPointsFiltered =
            potentialNextPaths
            |> List.map (fun x -> x |> List.last)
            |> List.distinct
            |> List.filter (fun x -> pointsVisited |> List.contains x |> not)
        let nextPaths =
            potentialNextPaths
            |> List.filter (fun x -> nextPointsFiltered |> List.contains (x |> List.last))
            |> List.groupBy (fun x -> x |> List.last)
            |> List.map (fun (_, groupedPaths) -> groupedPaths |> List.sortBy (fun x -> x.Length) |> List.take 1)
            |> List.collect id
        nextPaths
        |> GetFullBranches map (pointsVisited @ nextPointsFiltered)
    | completed -> completed

let GetShortestRouteNumberOfSteps input =
    let map = ReadInput input
    GetFullBranches map [] []
    |> List.map (fun x -> x.Length - 1)
    |> List.min