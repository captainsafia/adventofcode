﻿// Copyright (c) Martin Costello, 2015. All rights reserved.
// Licensed under the Apache 2.0 license. See the LICENSE file in the project root for full license information.

namespace MartinCostello.AdventOfCode.Puzzles.Y2015
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Text;

    /// <summary>
    /// A class representing the puzzle for <c>http://adventofcode.com/2015/day/10</c>. This class cannot be inherited.
    /// </summary>
    internal sealed class Day10 : Puzzle2015
    {
        /// <summary>
        /// Gets the solution to the puzzle.
        /// </summary>
        internal int Solution { get; private set; }

        /// <inheritdoc />
        protected override int MinimumArguments => 2;

        /// <summary>
        /// Gets the 'look-and-say' representation of a <see cref="string"/>.
        /// </summary>
        /// <param name="value">The value to get the 'look-and-say' result for.</param>
        /// <returns>The 'look-and-say' representation of <paramref name="value"/>.</returns>
        internal static string AsLookAndSay(string value)
        {
            var input = new Queue<char>(value);
            var output = new StringBuilder();

            while (input.Count > 0)
            {
                char current = input.Dequeue();
                int count = 1;

                while (input.Count > 0 && input.Peek() == current)
                {
                    input.Dequeue();
                    count++;
                }

                output.AppendFormat(CultureInfo.InvariantCulture, "{0}{1}", count, current);
            }

            return output.ToString();
        }

        /// <inheritdoc />
        protected override int SolveCore(string[] args)
        {
            string value = args[0];
            int iterations = ParseInt32(args[1], NumberStyles.Integer & ~NumberStyles.AllowLeadingSign);

            string result = value;

            for (int i = 0; i < iterations; i++)
            {
                result = AsLookAndSay(result);
            }

            Console.WriteLine(
                "The length of the result for input '{0}' after {1:N0} iterations is {2:N0}.",
                value,
                iterations,
                Solution = result.Length);

            return 0;
        }
    }
}