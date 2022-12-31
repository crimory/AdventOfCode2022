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
            Name = MainDir,
            Files = new List<MyFile>(),
            SubDirectories = new List<MyDirectory>()
        };
        var currentDirectory = MainDirectory;
        
        var lines = consoleOutput.Split(Environment.NewLine);
        foreach (var line in lines)
        {
            var lineType = GetLineType(line);
            switch (lineType)
            {
                case ConsoleLine.Command:
                    var commandWithoutKeyword = line[(CommandStart.Length + Separator.Length)..];
                    var commandType = GetCommandType(commandWithoutKeyword);
                    switch (commandType)
                    {
                        case ConsoleCommand.ChangeDirectory:
                            var changeDirWithoutKeyword = commandWithoutKeyword[(CommandChangeDir.Length + Separator.Length)..];
                            var direction = GetDirectoryChangeDirection(changeDirWithoutKeyword);
                            switch (direction)
                            {
                                case ChangeDirectoryCommand.Main:
                                    currentDirectory = MainDirectory;
                                    break;
                                case ChangeDirectoryCommand.Up:
                                    currentDirectory = currentDirectory.Parent;
                                    break;
                                case ChangeDirectoryCommand.Down:
                                    var changeDirectoryName = changeDirWithoutKeyword;
                                    currentDirectory =
                                        currentDirectory.SubDirectories.Find(x => x.Name == changeDirectoryName);
                                    break;
                                default:
                                    throw new ArgumentOutOfRangeException();
                            }
                            break;
                        case ConsoleCommand.ListDirectory:
                            break;
                        default:
                            throw new ArgumentOutOfRangeException();
                    }
                    break;
                case ConsoleLine.Output:
                    var pureOutput = line.Split(Separator);
                    if (pureOutput[0] == ListedDirectory)
                    {
                        currentDirectory.SubDirectories.Add(new MyDirectory
                        {
                            Parent = currentDirectory,
                            Name = pureOutput[1],
                            Files = new List<MyFile>(),
                            SubDirectories = new List<MyDirectory>()
                        });
                        break;
                    }

                    currentDirectory.Files.Add(new MyFile
                    {
                        Size = long.Parse(pureOutput[0]),
                        Name = pureOutput[1]
                    });
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
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
        public string Name { get; init; }
        public MyDirectory? Parent { get; init; }
        public List<MyDirectory> SubDirectories { get; init; }
        public List<MyFile> Files { get; init; }
    }

    private class MyFile
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
        var fromFiles =
            dir
            .Files
            .Select(x => x.Size)
            .Sum();
        var fromSubDirectories =
            dir
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

