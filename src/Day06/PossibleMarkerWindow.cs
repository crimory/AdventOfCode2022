namespace Day06;

internal class PossibleMarkerWindow
{
    private int _markerIndex;
    private readonly int _markerSize;
    private List<char> _restOfTheInput;
    private List<char> _potentialMarker;

    public PossibleMarkerWindow(List<char> inputList, uint markerLength)
    {
        var typedMarkerLength = (int)markerLength;
        _potentialMarker = inputList.Take(typedMarkerLength).ToList();
        _restOfTheInput = inputList.Skip(typedMarkerLength).ToList();
        _markerIndex = typedMarkerLength;
        _markerSize = typedMarkerLength;
    }

    public bool MoveWindowIfPossible()
    {
        if (!_restOfTheInput.Any())
            return false;
            
        _potentialMarker.Add(_restOfTheInput.First());
        _potentialMarker = _potentialMarker.Skip(1).ToList();
        _restOfTheInput = _restOfTheInput.Skip(1).ToList();
        _markerIndex++;
        return true;
    }

    public (bool, int) IsMarkerAndMarkerIndex()
    {
        return _potentialMarker.Distinct().ToList().Count == _markerSize
            ? (true, _markerIndex)
            : (false, -1);
    }
}