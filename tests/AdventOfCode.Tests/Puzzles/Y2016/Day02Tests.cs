﻿// Copyright (c) Martin Costello, 2015. All rights reserved.
// Licensed under the Apache 2.0 license. See the LICENSE file in the project root for full license information.

namespace MartinCostello.AdventOfCode.Puzzles.Y2016
{
    using Xunit;

    /// <summary>
    /// A class containing tests for the <see cref="Day02"/> class. This class cannot be inherited.
    /// </summary>
    public static class Day02Tests
    {
        [Theory]
        [InlineData(new[] { "UDLR" }, 5)]
        [InlineData(new[] { "ULL", "RRDDD", "LURDL", "UUUUD" }, 1985)]
        public static void Y2016_Day02_GetBathroomCode_Returns_Correct_Solution(string[] instructions, int expected)
        {
            // Act
            int actual = Day02.GetBathroomCode(instructions);

            // Assert
            Assert.Equal(expected, actual);
        }

        [Fact]
        public static void Y2016_Day02_Solve_Returns_Correct_Solution()
        {
            // Act
            var puzzle = PuzzleTestHelpers.SolvePuzzle<Day02>();

            // Assert
            Assert.Equal(14894, puzzle.BathroomCode);
        }
    }
}
