namespace Day07;

public class DirectoryTree
{
    private const string MainDir = "/";
    private const string DirUp = "..";
    private const string CommandStart = "$";
    private const string CommandList = "ls";
    private const string CommandChangeDir = "cd";
    private const string Separator = " ";
    private const string ListedDirectory = "dir";

    private enum ConsoleLine
    {
        Command,
        Output
    }

    private enum ConsoleCommand
    {
        ChangeDirectory,
        ListDirectory
    }

    private enum ChangeDirectoryCommand
    {
        Main,
        Up,
        Down
    }

    private MyDirectory MainDirectory;

    public DirectoryTree(string consoleOutput)
    {
        MainDirectory = new MyDirectory
        {
            Parent = null,
            Name = MainDir
        };
        var currentDirectory = MainDirectory;

        var lines = consoleOutput.Split(Environment.NewLine);
        foreach (var line in lines)
        {
            var lineType = GetLineType(line);
            switch (lineType)
            {
                case ConsoleLine.Command:
                    currentDirectory = ProcessCommandsAndGetNewCurrentDir(line, currentDirectory);
                    break;
                case ConsoleLine.Output:
                    ProcessLineOutput(line, currentDirectory);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }

    private MyDirectory ProcessCommandsAndGetNewCurrentDir(string input, MyDirectory currentDirectory)
    {
        var commandWithoutKeyword = input[(CommandStart.Length + Separator.Length)..];
        var commandType = GetCommandType(commandWithoutKeyword);
        switch (commandType)
        {
            case ConsoleCommand.ChangeDirectory:
                var changeDirWithoutKeyword = commandWithoutKeyword[(CommandChangeDir.Length + Separator.Length)..];
                return ProcessChangeDirectory(changeDirWithoutKeyword, currentDirectory);
            case ConsoleCommand.ListDirectory:
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }

        return currentDirectory;
    }

    private MyDirectory ProcessChangeDirectory(string input, MyDirectory currentDirectory)
    {
        var direction = GetDirectoryChangeDirection(input);
        switch (direction)
        {
            case ChangeDirectoryCommand.Main:
                return MainDirectory;
            case ChangeDirectoryCommand.Up:
                return currentDirectory.Parent ?? currentDirectory;
            case ChangeDirectoryCommand.Down:
                var changeDirectoryName = input;
                return currentDirectory.SubDirectories.First(x => x.Name == changeDirectoryName);
            default:
                throw new ArgumentOutOfRangeException();
        }
    }

    private static void ProcessLineOutput(string input, MyDirectory currentDirectory)
    {
        var pureOutput = input.Split(Separator);
        if (pureOutput[0] == ListedDirectory)
        {
            currentDirectory.SubDirectories.Add(new MyDirectory
            {
                Parent = currentDirectory,
                Name = pureOutput[1]
            });
            return;
        }

        currentDirectory.Files.Add(new MyFile
        {
            Size = long.Parse(pureOutput[0]),
            Name = pureOutput[1]
        });
    }

    private static ConsoleLine GetLineType(string line)
    {
        return line.StartsWith(CommandStart)
            ? ConsoleLine.Command
            : ConsoleLine.Output;
    }

    private static ConsoleCommand GetCommandType(string lineWithoutKeyword)
    {
        return lineWithoutKeyword.StartsWith(CommandList)
            ? ConsoleCommand.ListDirectory
            : ConsoleCommand.ChangeDirectory;
    }

    private static ChangeDirectoryCommand GetDirectoryChangeDirection(string dirChangeWithoutKeyword)
    {
        if (dirChangeWithoutKeyword.StartsWith(MainDir))
            return ChangeDirectoryCommand.Main;
        if (dirChangeWithoutKeyword.StartsWith(DirUp))
            return ChangeDirectoryCommand.Up;
        return ChangeDirectoryCommand.Down;
    }

    private class MyDirectory
    {
        public MyDirectory()
        {
            Name = string.Empty;
            Parent = null;
            SubDirectories = new List<MyDirectory>();
            Files = new List<MyFile>();
        }

        public string Name { get; init; }
        public MyDirectory? Parent { get; init; }
        public List<MyDirectory> SubDirectories { get; }
        public List<MyFile> Files { get; }
    }

    private record MyFile
    {
        public string Name { get; init; }
        public long Size { get; init; }
    }

    public long GetDirectoriesSizeSumAtMost(uint maxSize)
    {
        var results = new Dictionary<MyDirectory, long>();
        GetSizeOfDirAndAddToList(MainDirectory, results, maxSize);
        return results.Select(x => x.Value).Sum();
    }

    private static long GetSizeOfDirAndAddToList(MyDirectory dir, Dictionary<MyDirectory, long> results, uint maxSize)
    {
        var fromFiles = dir
            .Files
            .Select(x => x.Size)
            .Sum();
        var fromSubDirectories = dir
            .SubDirectories
            .Select(x => GetSizeOfDirAndAddToList(x, results, maxSize))
            .Sum();
        var sum = fromFiles + fromSubDirectories;
        if (sum <= maxSize)
            results.Add(dir, sum);
        return sum;
    }

    public long GetSizeOfSmallestDeletedFolderToRunUpdate(uint diskSize, uint diskSpaceRequired)
    {
        var results = new Dictionary<MyDirectory, long>();
        GetSizeOfDirAndAddToList(MainDirectory, results, diskSize);
        var diskSpaceOccupied = results
            .First(x => x.Key.Name == MainDir)
            .Value;
        var spaceRequiredToBeRemoved = diskSpaceRequired - (diskSize - diskSpaceOccupied);
        var possibilities = results
            .Where(x => x.Value >= spaceRequiredToBeRemoved)
            .Select(x => x.Value);
        return possibilities.Min();
    }
}