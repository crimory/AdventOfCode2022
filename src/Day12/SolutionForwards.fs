module Day12.SolutionForwards

open Day12.Solution
open System

type SolutionDirection = Upwards | Downwards

let private areNextPointCoordinatesAcceptable currentCoordinates previousCoordinates =
    match currentCoordinates, previousCoordinates with
        | { X = cx; Y = cy }, { X = px; Y = py } when cy = py -> (cx - px) |> Math.Abs = 1
        | { X = cx; Y = cy }, { X = px; Y = py } when cx = px -> (cy - py) |> Math.Abs = 1
        | _ -> false

let internal GetPossibleNextPoints map currentPoint =
    map
    |> List.filter (fun x -> x.Coordinates |> areNextPointCoordinatesAcceptable currentPoint.Coordinates)

let internal IsNextPointHeightAcceptable solutionDirection currentPoint nextPoint =
    let currentHeight = currentPoint |> currentPointHeight
    let nextHeight = nextPoint |> currentPointHeight
    match solutionDirection, currentHeight, nextHeight with
    | Upwards, current, next when (next - current) <= 1 -> true
    | Downwards, current, next when (current - next) <= 1 -> true
    | _ -> false

let internal GetNextAcceptablePoints solutionDirection map currentPoint =
    GetPossibleNextPoints map currentPoint
    |> List.filter (fun x -> x |> IsNextPointHeightAcceptable solutionDirection currentPoint)

let internal GetNextStepBranches solutionDirection map (previousPoints: MapPoint list) =
    let currentPoint = previousPoints |> List.last
    let acceptableNewPoints =
        GetNextAcceptablePoints solutionDirection map currentPoint
        |> List.filter (fun x -> previousPoints |> List.contains x |> not)
    acceptableNewPoints
    |> List.map (fun x -> previousPoints @ [x])

let private TakeMaximumOf maxLength list =
    list
    |> List.take (if list.Length > maxLength then maxLength else list.Length)

let rec internal GetFullBranches startHeight targetHeight solutionDirection map (initialPointsVisited: MapPoint list) (initialPreviousPaths: MapPoint list list) =
    let pointsVisited =
        match initialPointsVisited with
        | [] -> [ map |> List.find (fun x -> x.Height = startHeight) ]
        | a -> a
    let previousPaths =
        match initialPreviousPaths with
        | [] -> [ [ map |> List.find (fun x -> x.Height = startHeight) ] ]
        | a -> a
    let completedPaths, incompletePaths =
        previousPaths
        |> List.partition (fun x -> (x |> List.last).Height = targetHeight )
    match completedPaths with
    | [] ->
        let potentialNextPaths =
            incompletePaths
            |> List.map (GetNextStepBranches solutionDirection map)
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
        |> GetFullBranches startHeight targetHeight solutionDirection map (pointsVisited @ nextPointsFiltered)
    | completed -> completed

let GetShortestRouteNumberOfSteps input =
    let map = ReadInput input
    GetFullBranches Start End Upwards map [] []
    |> List.map (fun x -> x.Length - 1)
    |> List.min

let GetShortestRouteNumberOfStepsToAnyLowestPoint input =
    let map = ReadInput input
    let mapWithoutStart =
        map
        |> List.map (fun x ->
            if x.Height = Start
            then { x with Height = Height 1 }
            else x)
    GetFullBranches End (Height 1) Downwards mapWithoutStart [] []
    |> List.map (fun x -> x.Length - 1)
    |> List.min