﻿// Copyright (c) Martin Costello, 2015. All rights reserved.
// Licensed under the Apache 2.0 license. See the LICENSE file in the project root for full license information.

namespace MartinCostello.AdventOfCode.Puzzles.Y2021;

public sealed class Day08Tests : PuzzleTest
{
    public Day08Tests(ITestOutputHelper outputHelper)
        : base(outputHelper)
    {
    }

    ////[Fact]
    ////public void Y2021_Day08_CountDigits_Returns_Correct_Value_1()
    ////{
    ////    // Arrange
    ////    string[] entries = { "acedgfb cdfbe gcdfa fbcad dab cefabd cdfgeb eafb cagedb ab | cdfeb fcadb cdfeb cdbaf" };
    ////
    ////    // Act
    ////    int actual = Day08.CountDigits(entries);
    ////
    ////    // Assert
    ////    actual.ShouldBe(expected);
    ////}

    [Fact]
    public void Y2021_Day08_CountDigits_Returns_Correct_Value()
    {
        // Arrange
        string[] entries =
        {
            "be cfbegad cbdgef fgaecd cgeb fdcge agebfd fecdb fabcd edb | fdgacbe cefdb cefbgd gcbe",
            "edbfga begcd cbg gc gcadebf fbgde acbgfd abcde gfcbed gfec | fcgedb cgb dgebacf gc",
            "fgaebd cg bdaec gdafb agbcfd gdcbef bgcad gfac gcb cdgabef | cg cg fdcagb cbg",
            "fbegcd cbd adcefb dageb afcb bc aefdc ecdab fgdeca fcdbega | efabcd cedba gadfec cb",
            "aecbfdg fbg gf bafeg dbefa fcge gcbea fcaegb dgceab fcbdga | gecf egdcabf bgf bfgea",
            "fgeab ca afcebg bdacfeg cfaedg gcfdb baec bfadeg bafgc acf | gebdcfa ecba ca fadegcb",
            "dbcfg fgd bdegcaf fgec aegbdf ecdfab fbedc dacgb gdcebf gf | cefg dcbef fcge gbcadfe",
            "bdfegc cbegaf gecbf dfcage bdacg ed bedf ced adcbefg gebcd | ed bcgafe cdgba cbgef",
            "egadfb cdbfeg cegd fecab cgb gbdefca cg fgcdab egfdb bfceg | gbdfcae bgc cg cgb",
            "gcafb gcf dcaebfg ecagb gf abcdeg gaef cafbge fdbac fegbdc | fgae cfgab fg bagce",
        };

        // Act
        int actual = Day08.CountDigits(entries);

        // Assert
        actual.ShouldBe(26);
    }

    [Fact]
    public async Task Y2021_Day08_Solve_Returns_Correct_Solution()
    {
        // Act
        var puzzle = await SolvePuzzleAsync<Day08>();

        // Assert
        puzzle.Count.ShouldBe(294);
    }
}
