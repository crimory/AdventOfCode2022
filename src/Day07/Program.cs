using Day07;

var input = File.ReadAllText("input.txt");
var sumOfDirectoriesSizes = Solution.SumOfDirectoriesSizes(input);
Console.WriteLine($"Sum of directories sizes, lower, than 100 000 is: {sumOfDirectoriesSizes}");

var sizeOfSmallestDeletedFolderToRunUpdate = Solution.SizeOfSmallestDeletedFolderToRunUpdate(input);
Console.WriteLine($"Directory size to be deleted is: {sizeOfSmallestDeletedFolderToRunUpdate}");