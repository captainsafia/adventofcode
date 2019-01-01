﻿// Copyright (c) Martin Costello, 2015. All rights reserved.
// Licensed under the Apache 2.0 license. See the LICENSE file in the project root for full license information.

namespace MartinCostello.AdventOfCode.Puzzles.Y2017
{
    using Xunit;
    using Xunit.Abstractions;

    /// <summary>
    /// A class containing tests for the <see cref="Day03"/> class. This class cannot be inherited.
    /// </summary>
    public sealed class Day03Tests : PuzzleTest
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Day03Tests"/> class.
        /// </summary>
        /// <param name="outputHelper">The <see cref="ITestOutputHelper"/> to use.</param>
        public Day03Tests(ITestOutputHelper outputHelper)
            : base(outputHelper)
        {
        }

        [Theory]
        [InlineData("1", 0)]
        [InlineData("12", 3)]
        [InlineData("23", 2)]
        [InlineData("1024", 31)]
        [InlineData("312051", 430)]
        public void Y2017_Day03_Solve_Returns_Correct_Solution_For_Steps(string square, int expected)
        {
            // Act
            var puzzle = SolvePuzzle<Day03>(square);

            // Assert
            Assert.Equal(expected, puzzle.Steps);
        }

        [Theory]
        [InlineData("312051", 312453)]
        public void Y2017_Day03_Solve_Returns_Correct_Solution_For_Storage(string square, int expected)
        {
            // Act
            var puzzle = SolvePuzzle<Day03>(square);

            // Assert
            Assert.Equal(expected, puzzle.FirstStorageLargerThanInput);
        }
    }
}
