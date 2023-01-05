namespace Day09

open System

module Solution =
    type Point = { X: int; Y: int }
    type ShortRope = { Head: Point; Tail: Point }
    type LongRope = { Head: Point; Tails: Point list }
    type Direction = Left | Right | Up | Down
    type Move = { Direction: Direction; Amount: uint }
    
    let internal ReadMoves (input: string) =
        let lines = input.Split(Environment.NewLine)
        let parseDirection (input: string) =
            match input with
            | "L" | "l" -> Left
            | "R" | "r" -> Right
            | "U" | "u" -> Up
            | "D" | "d" -> Down
            | _ -> failwith "todo"
        lines
        |> Array.map (fun x -> x.Split " ")
        |> Array.map (fun x -> { Direction = x[0] |> parseDirection; Amount = x[1] |> uint })
        
    let private SingleFollowPointMovement tailPoint movedHeadPoint =
        let distanceX = movedHeadPoint.X - tailPoint.X
        let distanceY = movedHeadPoint.Y - tailPoint.Y
        let newX =
            match distanceX with
            | 2 -> movedHeadPoint.X - 1
            | -2 -> movedHeadPoint.X + 1
            | _ -> movedHeadPoint.X
        let newY =
            match distanceY with
            | 2 -> movedHeadPoint.Y - 1
            | -2 -> movedHeadPoint.Y + 1
            | _ -> movedHeadPoint.Y
        match distanceX |> Math.Abs, distanceY |> Math.Abs with
        | 2, _ | _, 2 -> { X = newX; Y = newY }
        | _ -> tailPoint
        
    let private SingleMovePoint direction point =
        match point, direction with
        | head, Left -> { head with X = head.X - 1 }
        | head, Right -> { head with X = head.X + 1 }
        | head, Down -> { head with Y = head.Y - 1 }
        | head, Up -> { head with Y = head.Y + 1 }
        
    let internal SingleMoveRope direction (rope: ShortRope) =
        let newHead = rope.Head |> SingleMovePoint direction
        let newTail = newHead |> SingleFollowPointMovement rope.Tail
        { Head = newHead; Tail = newTail }
    
    let private MoveShortRope move rope =
        let getListOfTailPositionsAndNewRope ropeAccumulator =
            let newRopeAccumulator = SingleMoveRope move.Direction ropeAccumulator
            newRopeAccumulator.Tail, newRopeAccumulator
        let result =
            [ 1 .. move.Amount |> int ]
            |> List.mapFold (fun acc _ -> getListOfTailPositionsAndNewRope acc) rope
        result |> fst |> List.distinct, result |> snd
    
    let HowManyPositionsWasTailAt input =
        let moves = ReadMoves input
        let initialRope = { Head = { X = 0; Y = 0 }; Tail = { X = 0; Y = 0 } }
        let tailPositions =
            moves
            |> Array.toList
            |> List.mapFold (fun acc move -> MoveShortRope move acc) initialRope
            |> fst
            |> List.collect id
            |> List.distinct
        tailPositions
        |> List.length
        
    let internal SingleMoveLongRope direction (rope: LongRope) =
        let newHead = rope.Head |> SingleMovePoint direction
        let getPartOfTailAndAccumulator tail accumulator =
            let newAccumulator = SingleFollowPointMovement tail accumulator
            newAccumulator, newAccumulator
        let newTails =
            rope.Tails
            |> List.mapFold (fun acc tail -> getPartOfTailAndAccumulator tail acc) newHead
            |> fst
        { Head = newHead; Tails = newTails }
    
    let internal MoveLongRope move rope =
        let getListOfTailPositionsAndNewRope ropeAccumulator =
            let newRopeAccumulator = SingleMoveLongRope move.Direction ropeAccumulator
            newRopeAccumulator.Tails |> List.last, newRopeAccumulator
        let result =
            [ 1 .. move.Amount |> int ]
            |> List.mapFold (fun acc _ -> getListOfTailPositionsAndNewRope acc) rope
        result |> fst |> List.distinct, result |> snd
    
    let HowManyPositionsWasLongTailAt input =
        let moves = ReadMoves input
        let initialRope = { Head = { X = 0; Y = 0 }; Tails = [
            { X = 0; Y = 0 }
            { X = 0; Y = 0 }
            { X = 0; Y = 0 }
            { X = 0; Y = 0 }
            { X = 0; Y = 0 }
            { X = 0; Y = 0 }
            { X = 0; Y = 0 }
            { X = 0; Y = 0 }
            { X = 0; Y = 0 }
        ] }
        let tailPositions =
            moves
            |> Array.toList
            |> List.mapFold (fun acc move -> MoveLongRope move acc) initialRope
            |> fst
            |> List.collect id
            |> List.distinct
        tailPositions
        |> List.length