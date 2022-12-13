﻿// Copyright (c) Martin Costello, 2015. All rights reserved.
// Licensed under the Apache 2.0 license. See the LICENSE file in the project root for full license information.

namespace MartinCostello.AdventOfCode.Puzzles.Y2022;

/// <summary>
/// A class representing the puzzle for <c>https://adventofcode.com/2022/day/14</c>. This class cannot be inherited.
/// </summary>
[Puzzle(2022, 14, "", RequiresData = true, IsHidden = true)]
public sealed class Day14 : Puzzle
{
#pragma warning disable IDE0022
#pragma warning disable SA1600

    public int Solution { get; private set; }

    public static int Solve(IList<string> values)
    {
        return -1;
    }

    /// <inheritdoc />
    protected override async Task<PuzzleResult> SolveCoreAsync(string[] args, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(args);

        var values = await ReadResourceAsLinesAsync();

        Solution = Solve(values);

        if (Verbose)
        {
            Logger.WriteLine("{0}", Solution);
        }

        return PuzzleResult.Create(Solution);
    }
}