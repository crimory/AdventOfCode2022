using Day08;

var input = File.ReadAllText("input.txt");
var amountOfTreesVisible = Solution.AmountOfTreesVisible(input);
Console.WriteLine($"Amount of trees visible: {amountOfTreesVisible}");

var highestPossibleScenicScore = Solution.HighestPossibleScenicScore(input);
Console.WriteLine($"Highest possible scenic score: {highestPossibleScenicScore}");