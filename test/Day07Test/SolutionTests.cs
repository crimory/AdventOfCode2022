using Day07;

namespace Day07Test;

public class SolutionTests
{
    [Fact]
    public void TotalSumOfDirectoriesWithDuplicates()
    {
        const string input = @"$ cd /
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
        var result = Solution.SumOfDirectoriesSizes(input);
        Assert.Equal(95_437, result);
    }
    
    [Fact]
    public void SizeOfSmallestDeletedFolderToRunUpdate()
    {
        const string input = @"$ cd /
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
        var result = Solution.SizeOfSmallestDeletedFolderToRunUpdate(input);
        Assert.Equal(24_933_642, result);
    }
}