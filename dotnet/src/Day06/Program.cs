using Day06;

var input = File.ReadAllText("input.txt");
var first4CharMarker = Solution.FindFirstDistinctMarker(input);
Console.WriteLine($"First 4-char marker is found after: {first4CharMarker} character");

var first14CharMarker = Solution.FindFirstDistinctMarker(input, 14);
Console.WriteLine($"First 14-char marker is found after: {first14CharMarker} character");