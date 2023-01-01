using Day08;

namespace Day08Test;

public class SolutionTests
{
    private const string Input = @"30373
25512
65332
33549
35390";
    
    [Fact]
    public void AmountOfTreesVisible_Correct()
    {
        var result = Solution.AmountOfTreesVisible(Input);
        
        Assert.Equal(21, result);
    }
    
    [Fact]
    public void HighestPossibleScenicScore_Correct()
    {
        var result = Solution.HighestPossibleScenicScore(Input);
        
        Assert.Equal(8, result);
    }
    
    [Theory]
    [InlineData(1, 2, 4)]
    [InlineData(3, 2, 8)]
    public void GetScenicScoreTests(int rowIndex, int treeIndex, int expectedScenicScore)
    {
        var trees = Solution.GetTrees(Input);
        var result = Solution.GetScenicScore(trees, rowIndex, treeIndex);
        
        Assert.Equal(expectedScenicScore, result);
    }
    
    [Theory]
    [InlineData(1, 2, Solution.LookingDirection.Top, 1)]
    [InlineData(1, 2, Solution.LookingDirection.Left, 1)]
    [InlineData(1, 2, Solution.LookingDirection.Right, 2)]
    [InlineData(1, 2, Solution.LookingDirection.Bottom, 2)]
    [InlineData(3, 2, Solution.LookingDirection.Top, 2)]
    [InlineData(3, 2, Solution.LookingDirection.Left, 2)]
    [InlineData(3, 2, Solution.LookingDirection.Right, 2)]
    [InlineData(3, 2, Solution.LookingDirection.Bottom, 1)]
    public void GetDirectionalScenicSubScoreTests(int rowIndex, int treeIndex, Solution.LookingDirection direction, int expectedScenicScore)
    {
        var trees = Solution.GetTrees(Input);
        var result = Solution.GetDirectionalScenicSubScore(trees, rowIndex, treeIndex, direction);
        
        Assert.Equal(expectedScenicScore, result);
    }
    
    [Theory]
    [InlineData(0, 1, 3, 3, true)]
    [InlineData(1, 1, 3, 3, false)]
    [InlineData(1, 2, 2, 3, true)]
    public void IsAtEdgeByIndexTests(int rowIndex, int treeIndex, int rows, int trees, bool expectedResult)
    {
        var result = Solution.IsAtEdgeByIndex(rowIndex, treeIndex, rows, trees);
        Assert.Equal(expectedResult, result);
    }
    
    [Theory]
    [InlineData("1111", "1211", "1111", Solution.LookingDirection.Top, true)]
    [InlineData("1311", "1211", "1111", Solution.LookingDirection.Top, false)]
    [InlineData("1311", "1211", "1111", Solution.LookingDirection.Left, true)]
    [InlineData("1311", "3211", "1111", Solution.LookingDirection.Left, false)]
    [InlineData("1311", "3211", "1111", Solution.LookingDirection.Bottom, true)]
    [InlineData("1211", "2211", "1211", Solution.LookingDirection.Bottom, false)]
    public void IsVisibleByIndexAndDirection11Tests(
        string treesRow1, string treesRow2, string treesRow3,
        Solution.LookingDirection direction, bool expectedVisibility)
    {
        var trees = new List<List<int>>
        {
            treesRow1.ToList().Select(x => int.Parse(x.ToString())).ToList(),
            treesRow2.ToList().Select(x => int.Parse(x.ToString())).ToList(),
            treesRow3.ToList().Select(x => int.Parse(x.ToString())).ToList()
        };
        var result = Solution.IsVisibleByIndex(trees, 1, 1, direction);
        Assert.Equal(expectedVisibility, result);
    }
    
    [Theory]
    [InlineData("1111", "1211", "1111", true)]
    [InlineData("1311", "3211", "1111", true)]
    [InlineData("1311", "3241", "1511", false)]
    public void IsVisibleByIndex11Tests(
        string treesRow1, string treesRow2, string treesRow3, bool expectedVisibility)
    {
        var trees = new List<List<int>>
        {
            treesRow1.ToList().Select(x => int.Parse(x.ToString())).ToList(),
            treesRow2.ToList().Select(x => int.Parse(x.ToString())).ToList(),
            treesRow3.ToList().Select(x => int.Parse(x.ToString())).ToList()
        };
        var result = Solution.IsVisibleByIndex(trees, 1, 1);
        Assert.Equal(expectedVisibility, result);
    }
}