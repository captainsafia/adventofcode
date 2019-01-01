﻿// Copyright (c) Martin Costello, 2015. All rights reserved.
// Licensed under the Apache 2.0 license. See the LICENSE file in the project root for full license information.

namespace MartinCostello.AdventOfCode.Puzzles.Y2015
{
    using System.Linq;
    using Xunit;
    using Xunit.Abstractions;

    /// <summary>
    /// A class containing tests for the <see cref="Day17"/> class. This class cannot be inherited.
    /// </summary>
    public sealed class Day17Tests : PuzzleTest
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Day17Tests"/> class.
        /// </summary>
        /// <param name="outputHelper">The <see cref="ITestOutputHelper"/> to use.</param>
        public Day17Tests(ITestOutputHelper outputHelper)
            : base(outputHelper)
        {
        }

        [Fact]
        public static void Y2015_Day17_GetContainerCombinationCount()
        {
            // Arrange
            int volume = 25;
            int[] containerVolumes = new[] { 20, 15, 10, 5, 5 };

            // Act
            var result = Day17.GetContainerCombinations(volume, containerVolumes);

            // Assert
            Assert.Equal(4, result.Count);
            Assert.Equal(3, result.GroupBy((p) => p.Count).OrderBy((p) => p.Key).First().Count());
        }

        [Fact]
        public void Y2015_Day17_Solve_Returns_Correct_Solution()
        {
            // Arrange
            string[] args = new[] { "150" };

            // Act
            var puzzle = SolvePuzzle<Day17>(args);

            // Assert
            Assert.Equal(1304, puzzle.Combinations);
            Assert.Equal(18, puzzle.CombinationsWithMinimumContainers);
        }
    }
}
