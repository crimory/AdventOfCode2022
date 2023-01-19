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

let internal GetNextAcceptablePoints isNextPointHeightAcceptable map currentPoint =
    GetPossibleNextPoints map currentPoint
    |> List.filter (fun x -> x |> isNextPointHeightAcceptable currentPoint)

let internal GetNextStepBranches getNextAcceptablePoints (previousPoints: MapPoint list) =
    let currentPoint = previousPoints |> List.last
    let acceptableNewPoints =
        getNextAcceptablePoints currentPoint
        |> List.filter (fun x -> previousPoints |> List.contains x |> not)
    acceptableNewPoints
    |> List.map (fun x -> previousPoints @ [x])

let internal GetFullBranches getNextStepBranches startPoint endHeight =
    let rec InnerGetFullBranches (pointsVisited: MapPoint list) (previousPaths: MapPoint list list) =
        let completedPaths, incompletePaths =
            previousPaths
            |> List.partition (fun x -> (x |> List.last).Height = endHeight)
        match completedPaths with
        | [] ->
            let potentialNextPaths =
                incompletePaths
                |> List.map getNextStepBranches
                |> List.concat
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
                |> List.concat
            nextPaths
            |> InnerGetFullBranches (pointsVisited @ nextPointsFiltered)
        | completed -> completed
    let initialPointsVisited = [ startPoint ]
    let initialPreviousPaths = [ [ startPoint ] ]
    InnerGetFullBranches initialPointsVisited initialPreviousPaths

let internal GetFullBranchesFromStartToEnd map =
    let startPoint = map |> List.find (fun x -> x.Height = Start)
    let isNextPointHeightAcceptable = IsNextPointHeightAcceptable Upwards
    let getNextStepBranches = GetNextStepBranches (GetNextAcceptablePoints isNextPointHeightAcceptable map)
    GetFullBranches getNextStepBranches startPoint End

let GetShortestRouteNumberOfSteps input =
    let map = ReadInput input
    GetFullBranchesFromStartToEnd map
    |> List.map (fun x -> x.Length - 1)
    |> List.min

let internal GetFullBranchesFromEndToMinHeight map =
    let startPoint = map |> List.find (fun x -> x.Height = End)
    let isNextPointHeightAcceptable = IsNextPointHeightAcceptable Downwards
    let getNextStepBranches = GetNextStepBranches (GetNextAcceptablePoints isNextPointHeightAcceptable map)
    GetFullBranches getNextStepBranches startPoint (Height 1)

let GetShortestRouteNumberOfStepsToAnyLowestPoint input =
    let map = ReadInput input
    GetFullBranchesFromEndToMinHeight map
    |> List.map (fun x -> x.Length - 1)
    |> List.min