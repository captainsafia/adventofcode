﻿// Copyright (c) Martin Costello, 2015. All rights reserved.
// Licensed under the Apache 2.0 license. See the LICENSE file in the project root for full license information.

namespace MartinCostello.AdventOfCode.Puzzles.Y2015
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    /// <summary>
    /// A class representing the puzzle for <c>http://adventofcode.com/2015/day/24</c>. This class cannot be inherited.
    /// </summary>
    internal sealed class Day24 : Puzzle2015
    {
        /// <summary>
        /// Gets the quantum entanglement of the first group
        /// of packages of the optimum package configuration.
        /// </summary>
        internal long QuantumEntanglementOfFirstGroup { get; private set; }

        /// <summary>
        /// Gets the quantum entanglement of the first group of packages of
        /// the ideal configuration for the specified packages and their weights.
        /// </summary>
        /// <param name="compartments">
        /// The number of comparments in the sleigh.
        /// </param>
        /// <param name="weights">
        /// The weights of the packages to find the quantum entanglement for.
        /// </param>
        /// <returns>
        /// The quantum entanglement of the first group of packages of the ideal
        /// configuration of the packages with weights specified by <paramref name="weights"/>.
        /// </returns>
        internal static long GetQuantumEntanglementOfIdealConfiguration(int compartments, IList<int> weights)
        {
            // How much should each compartment weigh?
            int total = weights.Sum() / compartments;

            var optimumConfiguration = Maths.GetCombinations(total, weights)
                .Select((p) => new { Count = p.Count, QuantumEntanglement = p.Aggregate((x, y) => x * y) })
                .OrderBy((p) => p.Count)
                .ThenBy((p) => p.QuantumEntanglement)
                .First();

            return optimumConfiguration.QuantumEntanglement;
        }

        /// <inheritdoc />
        protected override int SolveCore(string[] args)
        {
            IList<int> weights = ReadResourceAsLines()
                .Select((p) => ParseInt32(p))
                .ToList();

            int compartments = args.Length == 1 ? ParseInt32(args[0]) : 3;

            QuantumEntanglementOfFirstGroup = GetQuantumEntanglementOfIdealConfiguration(compartments, weights);

            Console.WriteLine(
                "The quantum entanglement of the ideal configuration of {0:N0} packages is {1:N0}.",
                weights.Count,
                QuantumEntanglementOfFirstGroup);

            return 0;
        }
    }
}