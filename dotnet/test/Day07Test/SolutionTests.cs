using Day07;

namespace Day07Test;

public class SolutionTests
{
    private const string Input = @"$ cd /
$ ls
dir a
14848514 b.txt
8504156 c.dat
dir d
$ cd a
$ ls
dir e
29116 f
2557 g
62596 h.lst
$ cd e
$ ls
584 i
$ cd ..
$ cd ..
$ cd d
$ ls
4060174 j
8033020 d.log
5626152 d.ext
7214296 k";
    
    [Fact]
    public void TotalSumOfDirectoriesWithDuplicates()
    {
        var result = Solution.SumOfDirectoriesSizes(Input);
        Assert.Equal(95_437, result);
    }
    
    [Fact]
    public void SizeOfSmallestDeletedFolderToRunUpdate()
    {
        var result = Solution.SizeOfSmallestDeletedFolderToRunUpdate(Input);
        Assert.Equal(24_933_642, result);
    }
}