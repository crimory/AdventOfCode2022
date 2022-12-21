module Day04.Solution

type Assignment = { FirstRoom: int; LastRoom: int }

type AssignmentPair =
    { FirstElf: Assignment
      SecondElf: Assignment }

let internal readAssignmentPair (line: string) =
    let separatorBetweenElves = ','
    let separatorBetweenRooms = '-'

    let parsedNumbers =
        line.Split separatorBetweenElves
        |> Array.map (fun elfAssignment -> elfAssignment.Split separatorBetweenRooms)
        |> Array.collect id
        |> Array.map int

    { FirstElf =
        { FirstRoom = parsedNumbers[0]
          LastRoom = parsedNumbers[1] }
      SecondElf =
        { FirstRoom = parsedNumbers[2]
          LastRoom = parsedNumbers[3] } }

let private numberOfAssignmentsFiltered (text: string) filtering =
    text.Split '\n'
    |> Array.map readAssignmentPair
    |> Array.filter filtering
    |> Array.length

let numberOfAssignmentsFullyContained text =
    let isAssignmentContainedInPair pair =
        (pair.FirstElf.FirstRoom <= pair.SecondElf.FirstRoom
         && pair.FirstElf.LastRoom >= pair.SecondElf.LastRoom)
        || (pair.SecondElf.FirstRoom <= pair.FirstElf.FirstRoom
            && pair.SecondElf.LastRoom >= pair.FirstElf.LastRoom)
    
    isAssignmentContainedInPair |> numberOfAssignmentsFiltered text

let private numberContainedBetween min max input =
    input >= min && input <= max

let numberOfAssignmentsOverlapping text =
    let isAssignmentOverlappingInPair pair =
        numberContainedBetween pair.SecondElf.FirstRoom pair.SecondElf.LastRoom pair.FirstElf.FirstRoom
        || numberContainedBetween pair.SecondElf.FirstRoom pair.SecondElf.LastRoom pair.FirstElf.LastRoom
        || numberContainedBetween pair.FirstElf.FirstRoom pair.FirstElf.LastRoom pair.SecondElf.FirstRoom
        || numberContainedBetween pair.FirstElf.FirstRoom pair.FirstElf.LastRoom pair.SecondElf.LastRoom

    isAssignmentOverlappingInPair |> numberOfAssignmentsFiltered text
