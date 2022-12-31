namespace Day06;

internal class PossibleMarkerWindow
{
    private int markerIndex;
    private readonly int markerSize;
    private List<char> restOfTheInput;
    private List<char> potentialMarker;

    public PossibleMarkerWindow(List<char> inputList, uint markerLength)
    {
        var typedMarkerLength = (int)markerLength;
        potentialMarker = inputList.Take(typedMarkerLength).ToList();
        restOfTheInput = inputList.Skip(typedMarkerLength).ToList();
        markerIndex = typedMarkerLength;
        markerSize = typedMarkerLength;
    }

    public bool MoveWindowIfPossible()
    {
        if (!restOfTheInput.Any())
            return false;
            
        potentialMarker.Add(restOfTheInput.First());
        potentialMarker = potentialMarker.Skip(1).ToList();
        restOfTheInput = restOfTheInput.Skip(1).ToList();
        markerIndex++;
        return true;
    }

    public (bool, int) IsMarkerAndMarkerIndex()
    {
        return potentialMarker.Distinct().ToList().Count == markerSize
            ? (true, MarkerIndex: markerIndex)
            : (false, -1);
    }
}