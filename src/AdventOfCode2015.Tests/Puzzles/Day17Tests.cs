﻿// Copyright (c) Martin Costello, 2015. All rights reserved.
// Licensed under the Apache 2.0 license. See the LICENSE file in the project root for full license information.

namespace MartinCostello.AdventOfCode2015.Puzzles
{
    using System.Linq;
    using Xunit;

    /// <summary>
    /// A class containing tests for the <see cref="Day17"/> class. This class cannot be inherited.
    /// </summary>
    public static class Day17Tests
    {
        [Fact]
        public static void Day17_GetContainerCombinationCount()
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
        public static void Day17_Solve_Returns_Correct_Solution()
        {
            // Arrange
            string[] args = new[] { @".\Input\Day17\input.txt", "150" };
            var target = new Day17();

            // Act
            int actual = target.Solve(args);

            // Assert
            Assert.Equal(0, actual);
            Assert.Equal(1304, target.Combinations);
            Assert.Equal(18, target.CombinationsWithMinimumContainers);
        }
    }
}
