open Day10
open System

let instructions = IO.File.ReadAllText "input.txt"

let signalStrengthSum = instructions |> Solution.SumSignalStrength [ 20 .. 40 .. 220 ]
Console.WriteLine $"Sum of signal strength is: {signalStrengthSum}"

let crtOutput = instructions |> Solution.GetCrtOutput
Console.WriteLine "CRT output is:"
crtOutput
|> List.map Console.WriteLine
|> ignore