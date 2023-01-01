namespace Day08;

public static class Solution
{
    public static int AmountOfTreesVisible(string input)
    {
        var trees = GetTrees(input);
        var numberOfRows = GetNumberOfRows(trees);
        var numberOfTrees = GetNumberOfTrees(trees);
        var treeVisibilityMapping = new List<List<bool>>();
        for (var rowIndex = 0; rowIndex < numberOfRows; rowIndex++)
        {
            treeVisibilityMapping.Add(new List<bool>());
            for (var treeIndex = 0; treeIndex < numberOfTrees; treeIndex++)
            {
                if (IsAtEdgeByIndex(rowIndex, treeIndex, numberOfRows, numberOfTrees))
                {
                    treeVisibilityMapping[rowIndex].Add(true);
                    continue;
                }
                treeVisibilityMapping[rowIndex].Add(IsVisibleByIndex(trees, rowIndex, treeIndex));
            }
        }

        return treeVisibilityMapping.Select(x => x.Count(y => y)).Sum();
    }

    private static int GetNumberOfRows(List<List<int>> trees) => trees.Count;
    private static int GetNumberOfTrees(List<List<int>> trees) => trees.First().Count;

    internal static List<List<int>> GetTrees(string input)
    {
        var lines = input.Split(Environment.NewLine);
        return lines
            .Select(x => x.ToList()
                .Select(y => int.Parse(y.ToString()))
                .ToList()
            )
            .ToList();
    }

    public static int HighestPossibleScenicScore(string input)
    {
        var trees = GetTrees(input);
        var scenicScores = new List<List<int>>();
        for (var i = 0; i < GetNumberOfRows(trees); i++)
        {
            scenicScores.Add(new List<int>());
            for (var j = 0; j < GetNumberOfTrees(trees); j++)
            {
                scenicScores[i].Add(GetScenicScore(trees, i, j));
            }
        }
        return scenicScores.SelectMany(x => x.Select(y => y)).Max();
    }

    internal static int GetScenicScore(List<List<int>> trees, int rowIndex, int treeIndex)
    {
        var top = GetDirectionalScenicSubScore(trees, rowIndex, treeIndex, LookingDirection.Top);
        var bottom = GetDirectionalScenicSubScore(trees, rowIndex, treeIndex, LookingDirection.Bottom);
        var left = GetDirectionalScenicSubScore(trees, rowIndex, treeIndex, LookingDirection.Left);
        var right = GetDirectionalScenicSubScore(trees, rowIndex, treeIndex, LookingDirection.Right);
        return top * bottom * left * right;
    }

    internal static int GetDirectionalScenicSubScore(List<List<int>> trees,
        int rowIndex, int treeIndex, LookingDirection direction)
    {
        var treesProceedingCurrent = GetProceedingTrees(trees, rowIndex, treeIndex, direction);
        var heightOfCurrentTree = trees[rowIndex][treeIndex];
        var count = 0;
        foreach (var nextTree in treesProceedingCurrent)
        {
            if (nextTree >= heightOfCurrentTree)
                return count + 1;
            count++;
        }

        return count;
    }

    internal static bool IsAtEdgeByIndex(int rowIndex, int treeIndex, int rows, int trees)
    {
        return rowIndex == 0 || treeIndex == 0 || rowIndex == rows - 1 || treeIndex == trees - 1;
    }
    
    internal static bool IsVisibleByIndex(List<List<int>> trees, int rowIndex, int treeIndex)
    {
        if (IsVisibleByIndex(trees, rowIndex, treeIndex, LookingDirection.Top))
            return true;
        if (IsVisibleByIndex(trees, rowIndex, treeIndex, LookingDirection.Bottom))
            return true;
        if (IsVisibleByIndex(trees, rowIndex, treeIndex, LookingDirection.Left))
            return true;
        if (IsVisibleByIndex(trees, rowIndex, treeIndex, LookingDirection.Right))
            return true;
        
        return false;
    }

    public enum LookingDirection
    {
        Top,
        Bottom,
        Left,
        Right
    }
    
    internal static bool IsVisibleByIndex(List<List<int>> trees, int rowIndex, int treeIndex, LookingDirection direction)
    {
        var treesProceedingCurrent = GetProceedingTrees(trees, rowIndex, treeIndex, direction);
        var heightOfCurrentTree = trees[rowIndex][treeIndex];
        return !treesProceedingCurrent.Any(x => x >= heightOfCurrentTree);
    }

    private static List<int> GetProceedingTrees(List<List<int>> trees, int rowIndex, int treeIndex, LookingDirection direction)
    {
        var treesProceedingCurrent = new List<int>();
        switch (direction)
        {
            case LookingDirection.Top:
                for (var i = rowIndex - 1; i >= 0; i--)
                    treesProceedingCurrent.Add(trees[i][treeIndex]);
                break;
            case LookingDirection.Bottom:
                for (var i = rowIndex + 1; i < GetNumberOfRows(trees); i++)
                    treesProceedingCurrent.Add(trees[i][treeIndex]);
                break;
            case LookingDirection.Left:
                for (var i = treeIndex - 1; i >= 0; i--)
                    treesProceedingCurrent.Add(trees[rowIndex][i]);
                break;
            case LookingDirection.Right:
                for (var i = treeIndex + 1; i < GetNumberOfTrees(trees); i++)
                    treesProceedingCurrent.Add(trees[rowIndex][i]);
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(direction), direction, null);
        }

        return treesProceedingCurrent;
    }
}