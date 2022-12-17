module Tests

open Xunit
open Day02.Solution

[<Theory>]
[<InlineData ("A Y", 8)>]
[<InlineData ("B X", 1)>]
[<InlineData ("C Z", 6)>]
let ``Calculate score for a line with 2 signs`` text expectedScore =
    let result = calculateLineScoreTwoSigns text
    Assert.Equal (expectedScore, result)
    
[<Fact>]
let ``Calculate score for a strategy with 2 signs`` () =
    let input =
        """A Y
        B X
        C Z"""
    let result = calculateStrategyScoreTwoSigns input
    Assert.Equal (15, result)
    
[<Theory>]
[<InlineData ("A Y", 4)>]
[<InlineData ("B X", 1)>]
[<InlineData ("C Z", 7)>]
let ``Calculate score for a line with a sign and an outcome`` text expectedScore =
    let result = calculateLineScoreSignAndOutcome text
    Assert.Equal (expectedScore, result)

[<Fact>]
let ``Calculate score for a sign and an outcome`` () =
    let input =
        """A Y
        B X
        C Z"""
    let result = calculateStrategyScoreSignAndOutcome input
    Assert.Equal (12, result)