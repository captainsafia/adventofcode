// Copyright (c) Martin Costello, 2015. All rights reserved.
// Licensed under the Apache 2.0 license. See the LICENSE file in the project root for full license information.

namespace MartinCostello.AdventOfCode.Puzzles.Y2017
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    /// <summary>
    /// A class representing the puzzle for <c>http://adventofcode.com/2017/day/5</c>. This class cannot be inherited.
    /// </summary>
    internal sealed class Day05 : Puzzle2017
    {
        /// <summary>
        /// Gets the number of steps required to exit the input instructions for version 1 of the CPU.
        /// </summary>
        public int StepsToExitV1 { get; private set; }

        /// <summary>
        /// Gets the number of steps required to exit the input instructions for version 2 of the CPU.
        /// </summary>
        public int StepsToExitV2 { get; private set; }

        /// <summary>
        /// Executes the specified program and CPU version.
        /// </summary>
        /// <param name="program">The program to execute represented as CPU jump offsets.</param>
        /// <param name="version">The version of the CPU to use.</param>
        /// <returns>
        /// The value of the program counter when the program terminates.
        /// </returns>
        public static int Execute(IEnumerable<int> program, int version)
        {
            var jumps = new List<int>(program);

            int counter = 0;
            int index = 0;

            while (index >= 0 && index < jumps.Count)
            {
                int offset = jumps[index];

                if (version == 2 && offset >= 3)
                {
                    jumps[index]--;
                }
                else
                {
                    jumps[index]++;
                }

                index += offset;
                counter++;
            }

            return counter;
        }

        /// <inheritdoc />
        protected override int SolveCore(string[] args)
        {
            IList<int> program = ReadResourceAsLines()
                .Select((p) => ParseInt32(p))
                .ToList();

            StepsToExitV1 = Execute(program, 1);
            StepsToExitV2 = Execute(program, 2);

            Console.WriteLine($"It takes {StepsToExitV1:N0} to reach the exit using version 1.");
            Console.WriteLine($"It takes {StepsToExitV2:N0} to reach the exit using version 2.");

            return 0;
        }
    }
}