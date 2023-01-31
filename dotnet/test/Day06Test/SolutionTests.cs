using Day06;

namespace Day06Test;

public class SolutionTests
{
    [Theory]
    [InlineData("mjqjpqmgbljsphdztnvjfqwrcgsmlb", 7)]
    [InlineData("bvwbjplbgvbhsrlpgdmjqwftvncz", 5)]
    [InlineData("nppdvjthqldpwncqszvftbrmjlhg", 6)]
    [InlineData("nznrnfrfntjfmvfwmzdfjlvtqnbhcprsg", 10)]
    [InlineData("zcfzfwzzqfrljwzlrfnpqdbhtmscgvjw", 11)]
    [InlineData("qwe", -1)]
    [InlineData("qweqa", 5)]
    [InlineData("qweqw", -1)]
    [InlineData("", -1)]
    public void FirstStartOfPacketMarker(string input, int expectedReturnedMarkerIndex)
    {
        var result = Solution.FindFirstDistinctMarker(input);
        Assert.Equal(expectedReturnedMarkerIndex, result);
    }
    
    [Theory]
    [InlineData("mjqjpqmgbljsphdztnvjfqwrcgsmlb", 19)]
    [InlineData("bvwbjplbgvbhsrlpgdmjqwftvncz", 23)]
    [InlineData("nppdvjthqldpwncqszvftbrmjlhg", 23)]
    [InlineData("nznrnfrfntjfmvfwmzdfjlvtqnbhcprsg", 29)]
    [InlineData("zcfzfwzzqfrljwzlrfnpqdbhtmscgvjw", 26)]
    public void FirstStartOfMessageMarker(string input, int expectedReturnedMarkerIndex)
    {
        var result = Solution.FindFirstDistinctMarker(input, 14);
        Assert.Equal(expectedReturnedMarkerIndex, result);
    }
}