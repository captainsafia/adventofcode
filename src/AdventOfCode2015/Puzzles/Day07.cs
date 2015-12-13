﻿// Copyright (c) Martin Costello, 2015. All rights reserved.
// Licensed under the Apache 2.0 license. See the LICENSE file in the project root for full license information.

namespace MartinCostello.AdventOfCode2015.Puzzles
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.IO;
    using System.Linq;

    /// <summary>
    /// A class representing the puzzle for <c>http://adventofcode.com/day/7</c>. This class cannot be inherited.
    /// </summary>
    internal sealed class Day07 : IPuzzle
    {
        /// <inheritdoc />
        public int Solve(string[] args)
        {
            if (args.Length != 1)
            {
                Console.Error.WriteLine("No input file path specified.");
                return -1;
            }

            if (!File.Exists(args[0]))
            {
                Console.Error.WriteLine("The input file path specified cannot be found.");
                return -1;
            }

            IList<string> instructions = File.ReadAllLines(args[0]);

            // Get the wire values for the initial instructions
            IDictionary<string, ushort> values = GetWireValues(instructions);

            ushort a = values["a"];

            Console.WriteLine("The signal for wire a is {0:N0}.", a);

            // Replace the input value for b with the value for a, then re-calculate
            int indexForB = instructions.IndexOf("44430 -> b");
            instructions[indexForB] = string.Format(CultureInfo.InvariantCulture, "{0} -> b", a);

            values = GetWireValues(instructions);

            a = values["a"];

            Console.WriteLine("The new signal for wire a is {0:N0}.", a);

            return 0;
        }

        /// <summary>
        /// Gets the wire values for the specified instructions.
        /// </summary>
        /// <param name="instructions">The instructions to get the wire values for.</param>
        /// <returns>An <see cref="IDictionary{TKey, TValue}"/> containing the values for wires keyed by their Ids.</returns>
        internal static IDictionary<string, ushort> GetWireValues(IEnumerable<string> instructions)
        {
            // Create a map of wire Ids to the instructions to get their value
            var instructionMap = instructions
                .Select((p) => p.Split(new string[] { " -> " }, StringSplitOptions.None))
                .ToDictionary((p) => p.Last(), (p) => p.First().Split(' '));

            Dictionary<string, ushort> result = new Dictionary<string, ushort>();

            // Loop through the instructions until we have reduced each instruction to a value
            while (result.Count != instructionMap.Count)
            {
                foreach (var pair in instructionMap)
                {
                    string wireId = pair.Key;

                    if (result.ContainsKey(wireId))
                    {
                        // We already have the value for this wire
                        continue;
                    }

                    string[] words = pair.Value;
                    ushort? solvedValue = null;

                    string firstOperand = words.FirstOrDefault();
                    string secondOperand;

                    if (words.Length == 1)
                    {
                        // "123 -> x" or " -> "lx -> a"
                        ushort value;

                        // Is the instruction a value or a previously solved value?
                        if (ushort.TryParse(firstOperand, out value) || result.TryGetValue(firstOperand, out value))
                        {
                            result[wireId] = value;
                        }
                    }
                    else if (words.Length == 2 && firstOperand == "NOT")
                    {
                        // "NOT e -> f" or "NOT 1 -> g"
                        secondOperand = words.ElementAtOrDefault(1);

                        ushort value;

                        // Is the second operand a value or a previously solved value?
                        if (ushort.TryParse(secondOperand, out value) ||
                            result.TryGetValue(secondOperand, out value))
                        {
                            result[wireId] = (ushort)~value;
                        }
                    }
                    else if (words.Length == 3)
                    {
                        secondOperand = words.ElementAtOrDefault(2);

                        ushort firstValue;
                        ushort secondValue;

                        // Are both operands a value or a previously solved value?
                        if ((ushort.TryParse(firstOperand, out firstValue) || result.TryGetValue(firstOperand, out firstValue)) &&
                            (ushort.TryParse(secondOperand, out secondValue) || result.TryGetValue(secondOperand, out secondValue)))
                        {
                            string operation = words.ElementAtOrDefault(1);
                            solvedValue = TrySolveValueForOperation(operation, firstValue, secondValue);
                        }
                    }

                    // The value for this wire Id has been solved
                    if (solvedValue.HasValue)
                    {
                        result[wireId] = solvedValue.Value;
                    }
                }
            }

            return result;
        }

        /// <summary>
        /// Tries to solve the value for the specified operation and values.
        /// </summary>
        /// <param name="operation">The operation.</param>
        /// <param name="firstValue">The first value.</param>
        /// <param name="secondValue">The second value.</param>
        /// <returns>The solved value for the specified parameters if solved; otherwise <see langword="null"/>.</returns>
        private static ushort? TrySolveValueForOperation(string operation, ushort firstValue, ushort secondValue)
        {
            if (operation == "AND")
            {
                // "x AND y -> z"
                return (ushort)(firstValue & secondValue);
            }
            else if (operation == "OR")
            {
                // "i OR j => k"
                return (ushort)(firstValue | secondValue);
            }
            else if (operation == "LSHIFT")
            {
                // "p LSHIFT 2"
                return (ushort)(firstValue << secondValue);
            }
            else if (operation == "RSHIFT")
            {
                // "q RSHIFT 3"
                return (ushort)(firstValue >> secondValue);
            }

            return null;
        }
    }
}
