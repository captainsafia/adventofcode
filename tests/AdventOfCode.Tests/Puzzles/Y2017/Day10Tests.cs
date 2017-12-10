// Copyright (c) Martin Costello, 2015. All rights reserved.
// Licensed under the Apache 2.0 license. See the LICENSE file in the project root for full license information.

namespace MartinCostello.AdventOfCode.Puzzles.Y2017
{
    using Xunit;

    /// <summary>
    /// A class containing tests for the <see cref="Day10"/> class. This class cannot be inherited.
    /// </summary>
    public static class Day10Tests
    {
        [Theory]
        [InlineData(5, new[] { 3, 4, 1, 5 }, 12)]
        public static void Y2017_Day10_FindProductOfFirstTwoHashElements_Returns_Correct_Value(
            int size,
            int[] lengths,
            int expected)
        {
            // Act
            int actual = Day10.FindProductOfFirstTwoHashElements(size, lengths);

            // Assert
            Assert.Equal(expected, actual);
        }

        [Fact]
        public static void Y2017_Day10_Solve_Returns_Correct_Solution()
        {
            // Act
            var puzzle = PuzzleTestHelpers.SolvePuzzle<Day10>();

            // Assert
            Assert.Equal(11413, puzzle.ProductOfFirstTwoElements);
        }
    }
}
