﻿// Copyright (c) Martin Costello, 2015. All rights reserved.
// Licensed under the Apache 2.0 license. See the LICENSE file in the project root for full license information.

namespace MartinCostello.AdventOfCode.Puzzles.Y2018
{
    using Xunit;
    using Xunit.Abstractions;

    /// <summary>
    /// A class containing tests for the <see cref="Day02"/> class. This class cannot be inherited.
    /// </summary>
    public sealed class Day02Tests : PuzzleTest
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Day02Tests"/> class.
        /// </summary>
        /// <param name="outputHelper">The <see cref="ITestOutputHelper"/> to use.</param>
        public Day02Tests(ITestOutputHelper outputHelper)
            : base(outputHelper)
        {
        }

        [Theory]
        [InlineData("abcdef", 0, 0)]
        [InlineData("bababc", 1, 1)]
        [InlineData("abbcde", 1, 0)]
        [InlineData("abcccd", 0, 1)]
        [InlineData("aabcdd", 1, 0)]
        [InlineData("abcdee", 1, 0)]
        [InlineData("ababab", 0, 1)]
        public static void Y2018_Day02_GetBoxScore_Returns_Correct_Solution(
            string id,
            int expected2,
            int expected3)
        {
            // Act
            (int actual2, int actual3) = Day02.GetBoxScore(id);

            // Assert
            Assert.Equal(expected2, actual2);
            Assert.Equal(expected3, actual3);
        }

        [Theory]
        [InlineData(new[] { "abcdef", "bababc", "abbcde", "abcccd", "aabcdd", "abcdee", "ababab" }, 12)]
        public static void Y2018_Day02_CalculateChecksum_Returns_Correct_Solution(
            string[] boxIds,
            int expected)
        {
            // Act
            int actual = Day02.CalculateChecksum(boxIds);

            // Assert
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void Y2018_Day02_Solve_Returns_Correct_Solution()
        {
            // Act
            var puzzle = SolvePuzzle<Day02>();

            // Assert
            Assert.Equal(5880, puzzle.Checksum);
            Assert.Equal("tiwcdpbseqhxryfmgkvjujvza", puzzle.CommonLettersForBoxes);
        }
    }
}