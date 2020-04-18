﻿// Copyright (c) Martin Costello, 2015. All rights reserved.
// Licensed under the Apache 2.0 license. See the LICENSE file in the project root for full license information.

namespace MartinCostello.AdventOfCode.Puzzles.Y2019
{
    using System;
    using System.Collections.Generic;

    /// <summary>
    /// A class representing the puzzle for <c>https://adventofcode.com/2019/day/1</c>. This class cannot be inherited.
    /// </summary>
    public sealed class Day01 : Puzzle2019
    {
        /// <summary>
        /// Gets the total fuel requirement for the modules.
        /// </summary>
        public int TotalFuelRequiredForModules { get; private set; }

        /// <summary>
        /// Gets the total fuel requirement for the rocket, including the fuel's own mass.
        /// </summary>
        public int TotalFuelRequiredForRocket { get; private set; }

        /// <summary>
        /// Gets the fuel requirements for the specified mass.
        /// </summary>
        /// <param name="mass">The mass to get the fuel requirements for.</param>
        /// <returns>
        /// The fuel requirements for the mass specified by <paramref name="mass"/>.
        /// </returns>
        public static int GetFuelRequirementsForMass(string mass)
            => GetFuelRequirementsForMass(ParseInt32(mass));

        /// <summary>
        /// Gets the fuel requirements for the specified mass, including the fuel itself.
        /// </summary>
        /// <param name="mass">The mass to get the fuel requirements for.</param>
        /// <returns>
        /// The fuel-inclusive fuel requirements for the mass specified by <paramref name="mass"/>.
        /// </returns>
        public static int GetFuelRequirementsForMassWithFuel(string mass)
            => GetFuelRequirementsForMassWithFuel(ParseInt32(mass));

        /// <inheritdoc />
        protected override int SolveCore(string[] args)
        {
            IList<string> masses = ReadResourceAsLines();

            TotalFuelRequiredForModules = GetFuelRequirementsForMasses(masses);
            TotalFuelRequiredForRocket = GetFuelRequirementsForRocket(masses);

            if (Verbose)
            {
                Logger.WriteLine("{0} fuel is required for the modules.", TotalFuelRequiredForModules);
                Logger.WriteLine("{0} fuel is required for the fully-fuelled rocket.", TotalFuelRequiredForRocket);
            }

            return 0;
        }

        /// <summary>
        /// Gets the fuel requirements for the specified mass.
        /// </summary>
        /// <param name="mass">The mass to get the fuel requirements for.</param>
        /// <returns>
        /// The fuel requirements for the mass specified by <paramref name="mass"/>.
        /// </returns>
        private static int GetFuelRequirementsForMass(int mass)
        {
            int requirement = mass / 3;

            requirement -= 2;

            return Math.Max(requirement, 0);
        }

        /// <summary>
        /// Gets the fuel requirements for the specified mass, including the fuel itself.
        /// </summary>
        /// <param name="mass">The mass to get the fuel requirements for.</param>
        /// <returns>
        /// The fuel-inclusive fuel requirements for the mass specified by <paramref name="mass"/>.
        /// </returns>
        private static int GetFuelRequirementsForMassWithFuel(int mass)
        {
            int fuel = GetFuelRequirementsForMass(mass);

            if (fuel > 0)
            {
                fuel += GetFuelRequirementsForMassWithFuel(fuel);
            }

            return fuel;
        }

        /// <summary>
        /// Gets the fuel requirements for a fully-fuelled rocket with the specified module masses.
        /// </summary>
        /// <param name="masses">The module masses to get the fuel requirements for a fully-fuelled rocket.</param>
        /// <returns>
        /// The fuel requirements for the rocket with the modules specified by <paramref name="masses"/>.
        /// </returns>
        private static int GetFuelRequirementsForMasses(IEnumerable<string> masses)
        {
            int total = 0;

            foreach (string mass in masses)
            {
                total += GetFuelRequirementsForMass(mass);
            }

            return total;
        }

        /// <summary>
        /// Gets the fuel requirements for a fully-fuelled rocket with the specified module masses.
        /// </summary>
        /// <param name="masses">The module masses to get the fuel requirements for a fully-fuelled rocket.</param>
        /// <returns>
        /// The fuel requirements for the rocket with the modules specified by <paramref name="masses"/>.
        /// </returns>
        private static int GetFuelRequirementsForRocket(IEnumerable<string> masses)
        {
            int total = 0;

            foreach (string mass in masses)
            {
                total += GetFuelRequirementsForMassWithFuel(mass);
            }

            return total;
        }
    }
}