namespace Day06;

public static class Solution
{
    public static int FindFirstDistinctMarkerOriginal(string input, int markerLength = 4)
    {
        if (input.Length < markerLength)
            return -1;
        
        for (var i = 0; i <= input.Length - markerLength; i++)
        {
            var potentialMarker = input.Substring(i, markerLength);
            if (potentialMarker.Distinct().ToList().Count == markerLength)
                return i + markerLength;
        }
        
        return -1;
    }

    public static int FindFirstDistinctMarker(string input, uint markerLength = 4)
    {
        var movingWindow = new PossibleMarkerWindow(input.ToList(), markerLength);
        do
        {
            var (isMarker, markerIndex) = movingWindow.IsMarkerAndMarkerIndex();
            if (isMarker)
                return markerIndex;
        } while (movingWindow.MoveWindowIfPossible());
        
        return -1;
    }
}