open System
open Day13.Solution

let instructions = IO.File.ReadAllText "input.txt"

let sumOfCorrectPacketsIndices = instructions |> GetSumOfPacketIndicesInCorrectOrder
Console.WriteLine $"Sum of indices of correct order indices: {sumOfCorrectPacketsIndices}"

let dividerPackets =
    [
        ListOfValues [ ListOfValues [ Value 2 ] ]
        ListOfValues [ ListOfValues [ Value 6 ] ]
    ]
let decoderKey = instructions |> GetDecoderKey dividerPackets
Console.WriteLine $"Decoder key (multiplication of divider packets indices): {decoderKey}"