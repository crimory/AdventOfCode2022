namespace Day07;

public static class Solution
{
    public static long SumOfDirectoriesSizes(string input, uint maxDirSize = 100_000)
    {
        var directories = new DirectoryTree(input);
        return directories.GetDirectoriesSizeSumAtMost(maxDirSize);
    }
    
    public static long SizeOfSmallestDeletedFolderToRunUpdate(
        string input,
        uint diskSize = 70_000_000,
        uint spaceRequiredForTheUpdate = 30_000_000)
    {
        var directories = new DirectoryTree(input);
        return directories.GetSizeOfSmallestDeletedFolderToRunUpdate(diskSize, spaceRequiredForTheUpdate);
    }
}