namespace Day08;

public static class Solution
{
    public static int AmountOfTreesVisible(string input)
    {
        var trees = GetTrees(input);
        var numberOfRows = trees.Count;
        var numberOfTrees = trees.First().Count;
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
        return -1;
    }

    internal static int GetScenicScore(List<List<int>> trees, int rowIndex, int treeIndex)
    {
        return -1;
    }

    internal static int GetDirectionalScenicSubScore(List<List<int>> trees,
        int rowIndex, int treeIndex, LookingDirection direction)
    {
        
        return -1;
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
        var treesProceedingCurrent = new List<int>();
        var startIndex = -1;
        var endIndex = -1;
        switch (direction)
        {
            case LookingDirection.Top:
                startIndex = 0;
                endIndex = rowIndex - 1;
                break;
            case LookingDirection.Bottom:
                startIndex = rowIndex + 1;
                endIndex = trees.Count - 1;
                break;
            case LookingDirection.Left:
                startIndex = 0;
                endIndex = treeIndex - 1;
                break;
            case LookingDirection.Right:
                startIndex = treeIndex + 1;
                endIndex = trees.First().Count - 1;
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(direction), direction, null);
        }

        if (direction is LookingDirection.Top or LookingDirection.Bottom)
        {
            for (var i = startIndex; i <= endIndex; i++)
            {
                treesProceedingCurrent.Add(trees[i][treeIndex]);
            }
        }
        else
        {
            for (var i = startIndex; i <= endIndex; i++)
            {
                treesProceedingCurrent.Add(trees[rowIndex][i]);
            }
        }
        
        var heightOfCurrentTree = trees[rowIndex][treeIndex];
        return !treesProceedingCurrent.Any(x => x >= heightOfCurrentTree);
    }
}