﻿// Copyright (c) Martin Costello, 2015. All rights reserved.
// Licensed under the Apache 2.0 license. See the LICENSE file in the project root for full license information.

namespace MartinCostello.AdventOfCode.Puzzles.Y2020
{
    using System.Collections.Generic;
    using System.Linq;

    /// <summary>
    /// A class representing the puzzle for <c>https://adventofcode.com/2020/day/1</c>. This class cannot be inherited.
    /// </summary>
    public sealed class Day01 : Puzzle2020
    {
        /// <summary>
        /// Gets the product of the two input values that sum to a value of 2020.
        /// </summary>
        public int ProductOf2020SumFrom2 { get; private set; }

        /// <summary>
        /// Gets the product of the three input values that sum to a value of 2020.
        /// </summary>
        public int ProductOf2020SumFrom3 { get; private set; }

        /// <summary>
        /// Gets the product of a number of values from the specified set of values
        /// that which when added together equal 2020.
        /// </summary>
        /// <param name="values">The values to find the 2020 sum's product from.</param>
        /// <param name="take">The number of values to use to reach the 2020 target.</param>
        /// <returns>
        /// The product of the specified number of values that sum to a value of 2020.
        /// </returns>
        public static int Get2020Product(IEnumerable<string> values, int take)
        {
            int[] expenses = values
                .Select((p) => ParseInt32(p))
                .ToArray();

            var result = Maths.GetPermutations(expenses, take);

            return result
                .Where((p) => p.Sum() == 2020)
                .Select((p) => p.Aggregate((x, y) => x * y))
                .First();
        }

        /// <inheritdoc />
        protected override int SolveCore(string[] args)
        {
            IList<string> values = ReadResourceAsLines();

            ProductOf2020SumFrom2 = Get2020Product(values, 2);
            ProductOf2020SumFrom3 = Get2020Product(values, 3);

            if (Verbose)
            {
                Logger.WriteLine("The product of the two entries that sum to 2020 is {0}.", ProductOf2020SumFrom2);
                Logger.WriteLine("The product of the three entries that sum to 2020 is {0}.", ProductOf2020SumFrom3);
            }

            return 0;
        }
    }
}