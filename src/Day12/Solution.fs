module Day12.Solution

open System

type MapHeight = Start | End | Height of int
type MapCoordinates = { X: int; Y: int }
type MapPoint = { Height: MapHeight; Coordinates: MapCoordinates }
type Map = MapPoint list

let internal ReadInput (input: string) =
    let readMapHeight char =
        match char with
        | 'S' -> Start
        | 'E' -> End
        | c -> Height (c |> int |> (+) -96)
    let readMapPoint lineIndex charIndex char =
        { Height = char |> readMapHeight; Coordinates = { X = charIndex; Y = lineIndex } }
    let readLine lineIndex (line: string) =
        line
        |> Seq.mapi (fun charIndex x -> x |> readMapPoint lineIndex charIndex)
        |> Seq.toList
    input.Split Environment.NewLine
    |> Array.toList
    |> List.mapi readLine
    |> List.collect id

let internal currentPointHeight currentPoint =
    let HeightOfSpecialMarkers = dict [ Start, 0; End, 27 ]
    match currentPoint.Height with
    | Start -> HeightOfSpecialMarkers[Start]
    | End -> HeightOfSpecialMarkers[End]
    | Height h -> h