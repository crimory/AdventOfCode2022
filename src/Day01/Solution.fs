module Day01.Solution

open System

let separateByElves (caloriesText:string) =
    let badIndex = -1
    let elfIndexBasedOnCalorieText elfIndex calorieText =
        match calorieText with
        | "" -> badIndex, elfIndex + 1
        | _ -> elfIndex, elfIndex
        
    let caloriesList =
        (caloriesText.Split Environment.NewLine)
        |> Array.toList
    let elfIndices =
        caloriesList
        |> List.mapFold elfIndexBasedOnCalorieText 1
        |> fst
    caloriesList
    |> List.zip elfIndices
    |> List.filter (fun (index, _) -> index > badIndex)
    |> List.map (fun (index, text) -> index, int text)
    |> List.groupBy fst
    |> List.map (fun (_, values) -> (values |> List.map snd))

let getMostCalories (calories: int list list) =
    calories
    |> List.map (fun singleElfCalories -> singleElfCalories |> List.sum)
    |> List.max

let getMostCaloriesFirstElves numberOfElves (calories: int list list) =
    calories
    |> List.map (fun singleElfCalories -> singleElfCalories |> List.sum)
    |> List.sortDescending
    |> List.take numberOfElves
    |> List.sum