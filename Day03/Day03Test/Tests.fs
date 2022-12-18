module Tests

open Day03.Solution
open Day03.SolutionInternals
open Xunit

[<Theory>]
[<InlineData ("vJrwpWtwJgWrhcsFMMfFFhFp", "vJrwpWtwJgWr", "hcsFMMfFFhFp")>]
[<InlineData ("jqHRNqRjqzjGDLGLrsFMfFZSrLrFZsSL", "jqHRNqRjqzjGDLGL", "rsFMfFZSrLrFZsSL")>]
[<InlineData ("PmmdzqPrVvPwwTWBwg", "PmmdzqPrV", "vPwwTWBwg")>]
let ``Split line per rucksack compartment`` input expectedFirstHalf expectedSecondHalf =
    let (resultFirstHalf, resultSecondHalf) = splitLineInHalf input
    Assert.Equal (expectedFirstHalf, resultFirstHalf)
    Assert.Equal (expectedSecondHalf, resultSecondHalf)

[<Theory>]
[<InlineData ("vJrwpWtwJgWrhcsFMMfFFhFp", "p")>]
[<InlineData ("jqHRNqRjqzjGDLGLrsFMfFZSrLrFZsSL", "L")>]
[<InlineData ("PmmdzqPrVvPwwTWBwg", "P")>]
[<InlineData ("wMqvLMZHhHMvwLHjbvcjnnSBnvTQFn", "v")>]
[<InlineData ("ttgJtRGJQctTZtZT", "t")>]
[<InlineData ("CrZsJsPPZsGzwwsLwLmpwMDw", "s")>]
let ``Find repeated item`` input expectedItem =
    let result = findRepetitionItem input
    Assert.Equal (expectedItem, result)

[<Fact>]
let ``Get priority summary`` () =
    let input =
        """vJrwpWtwJgWrhcsFMMfFFhFp
        jqHRNqRjqzjGDLGLrsFMfFZSrLrFZsSL
        PmmdzqPrVvPwwTWBwg
        wMqvLMZHhHMvwLHjbvcjnnSBnvTQFn
        ttgJtRGJQctTZtZT
        CrZsJsPPZsGzwwsLwLmpwMDw"""
    let result = getPrioritySum input
    Assert.Equal (157, result)

[<Theory>]
[<InlineData ("""vJrwpWtwJgWrhcsFMMfFFhFp
jqHRNqRjqzjGDLGLrsFMfFZSrLrFZsSL
PmmdzqPrVvPwwTWBwg""", "r")>]
[<InlineData ("""wMqvLMZHhHMvwLHjbvcjnnSBnvTQFn
ttgJtRGJQctTZtZT
CrZsJsPPZsGzwwsLwLmpwMDw""", "Z")>]
let ``Get badge of 3 rucksack groups`` input expectedBadge =
    let result = getBadge input
    Assert.Equal (expectedBadge, result)

[<Fact>]
let ``Cut text into 3-elf groups`` () =
    let input =
        """vJrwpWtwJgWrhcsFMMfFFhFp
        jqHRNqRjqzjGDLGLrsFMfFZSrLrFZsSL
        PmmdzqPrVvPwwTWBwg
        wMqvLMZHhHMvwLHjbvcjnnSBnvTQFn
        ttgJtRGJQctTZtZT
        CrZsJsPPZsGzwwsLwLmpwMDw"""
    let expectedOutput =
        [ """vJrwpWtwJgWrhcsFMMfFFhFp
jqHRNqRjqzjGDLGLrsFMfFZSrLrFZsSL
PmmdzqPrVvPwwTWBwg"""; """wMqvLMZHhHMvwLHjbvcjnnSBnvTQFn
ttgJtRGJQctTZtZT
CrZsJsPPZsGzwwsLwLmpwMDw""" ]
    let result = splitTextIntoElfGroups 3 input
    Assert.Equivalent (expectedOutput, result, strict = true)

[<Fact>]
let ``Get priority summary for each 3-elf badge`` () =
    let input =
        """vJrwpWtwJgWrhcsFMMfFFhFp
        jqHRNqRjqzjGDLGLrsFMfFZSrLrFZsSL
        PmmdzqPrVvPwwTWBwg
        wMqvLMZHhHMvwLHjbvcjnnSBnvTQFn
        ttgJtRGJQctTZtZT
        CrZsJsPPZsGzwwsLwLmpwMDw"""
    let result = getPrioritySumPerElfBadge input
    Assert.Equal (70, result)