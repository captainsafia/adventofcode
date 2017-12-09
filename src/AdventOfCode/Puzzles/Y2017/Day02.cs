// Copyright (c) Martin Costello, 2015. All rights reserved.
// Licensed under the Apache 2.0 license. See the LICENSE file in the project root for full license information.

namespace MartinCostello.AdventOfCode.Puzzles.Y2017
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Linq;

    /// <summary>
    /// A class representing the puzzle for <c>http://adventofcode.com/2017/day/2</c>. This class cannot be inherited.
    /// </summary>
    internal sealed class Day02 : Puzzle2017
    {
        /// <summary>
        /// Gets the checksum of the spreadsheet using the difference between the minimum and maximum.
        /// </summary>
        public int ChecksumForDifference { get; private set; }

        /// <summary>
        /// Gets the checksum of the spreadsheet using the evenly divisible values.
        /// </summary>
        public int ChecksumForEvenlyDivisible { get; private set; }

        /// <summary>
        /// Calculates the checksum of the spreadsheet encoded in the specified string.
        /// </summary>
        /// <param name="spreadsheet">An <see cref="IEnumerable{T}"/> containing the rows of integers in the spreadsheet.</param>
        /// <param name="forEvenlyDivisible">Whether to compute the checksum using the evenly divisible values.</param>
        /// <returns>
        /// The checksum of the spreadsheet encoded by <paramref name="spreadsheet"/>.
        /// </returns>
        public static int CalculateChecksum(IEnumerable<IEnumerable<int>> spreadsheet, bool forEvenlyDivisible)
        {
            var sequence =
                forEvenlyDivisible ?
                spreadsheet.Select(ComputeDivisionOfEvenlyDivisible) :
                spreadsheet.Select(ComputeDifference);

            return sequence.Sum();
        }

        /// <summary>
        /// Computes the difference for the specified row of a spreadsheet.
        /// </summary>
        /// <param name="row">The values in the row of the spreadsheet.</param>
        /// <returns>
        /// The difference between the minimum and maximum values in the row.
        /// </returns>
        public static int ComputeDifference(IEnumerable<int> row)
        {
            int minimum = row.Min();
            int maximum = row.Max();

            return maximum - minimum;
        }

        /// <summary>
        /// Computes the result of dividing the two evenly divisible values for the specified row of a spreadsheet.
        /// </summary>
        /// <param name="row">The values in the row of the spreadsheet.</param>
        /// <returns>
        /// The result of dividing the two evenly divisible values in the row.
        /// </returns>
        public static int ComputeDivisionOfEvenlyDivisible(IEnumerable<int> row)
        {
            IList<int> values = row.ToList();

            for (int i = 0; i < values.Count - 1; i++)
            {
                int x = values[i];

                for (int j = i + 1; j < values.Count; j++)
                {
                    int y = values[j];

                    if (x % y == 0)
                    {
                        return Math.Abs(x / y);
                    }
                    else if (y % x == 0)
                    {
                        return Math.Abs(y / x);
                    }
                }
            }

            return 0;
        }

        /// <inheritdoc />
        protected override int SolveCore(string[] args)
        {
            var lines = ReadResourceAsLines();
            var spreadsheet = ParseSpreadsheet(lines);

            ChecksumForDifference = CalculateChecksum(spreadsheet, forEvenlyDivisible: false);
            ChecksumForEvenlyDivisible = CalculateChecksum(spreadsheet, forEvenlyDivisible: true);

            Console.WriteLine($"The checksum for the spreadsheet using differences is {ChecksumForDifference:N0}.");
            Console.WriteLine($"The checksum for the spreadsheet using even division is {ChecksumForEvenlyDivisible:N0}.");

            return 0;
        }

        /// <summary>
        /// Parses the lines of the specified spreadsheet.
        /// </summary>
        /// <param name="rows">The rows of the spreadsheet.</param>
        /// <returns>
        /// An <see cref="IEnumerable{T}"/> containing the columns for each row of the spreadsheet.
        /// </returns>
        private static IEnumerable<IEnumerable<int>> ParseSpreadsheet(ICollection<string> rows)
        {
            var spreadsheet = new List<IEnumerable<int>>(rows.Count);

            foreach (string line in rows)
            {
                IList<int> columns = line
                    .Split('\t')
                    .Select((p) => int.Parse(p, CultureInfo.InvariantCulture))
                    .ToList();

                spreadsheet.Add(columns);
            }

            return spreadsheet;
        }
    }
}