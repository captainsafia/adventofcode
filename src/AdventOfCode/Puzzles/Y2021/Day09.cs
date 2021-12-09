﻿// Copyright (c) Martin Costello, 2015. All rights reserved.
// Licensed under the Apache 2.0 license. See the LICENSE file in the project root for full license information.

using System.Drawing;

namespace MartinCostello.AdventOfCode.Puzzles.Y2021;

/// <summary>
/// A class representing the puzzle for <c>https://adventofcode.com/2021/day/9</c>. This class cannot be inherited.
/// </summary>
[Puzzle(2021, 09, RequiresData = true)]
public sealed class Day09 : Puzzle
{
    /// <summary>
    /// Gets the sum of the risk levels of all the low points in the heightmap.
    /// </summary>
    public int SumOfRiskLevels { get; private set; }

    /// <summary>
    /// Gets the total area of the three largest basins in the heightmap.
    /// </summary>
    public int AreaOfThreeLargestBasins { get; private set; }

    /// <summary>
    /// Determines the level of risk of the low points in the specified heightmap.
    /// </summary>
    /// <param name="heightmap">The heightmap to analyze.</param>
    /// <returns>
    /// The sum of the risk levels of all the low points in the heightmap and
    /// the total area of the three largest basins in the heightmap.
    /// </returns>
    public static (int SumOfRiskLevels, int AreaOfThreeLargestBasins) AnalyzeRisk(IList<string> heightmap)
    {
        int width = heightmap[0].Length;
        int height = heightmap.Count;

        var heights = new Dictionary<Point, int>(width * height);

        for (int y = 0; y < height; y++)
        {
            string row = heightmap[y];

            for (int x = 0; x < width; x++)
            {
                heights[new(x, y)] = row[x] - '0';
            }
        }

        var basins = new SquareGrid(width, height);

        foreach ((var point, int value) in heights)
        {
            // The value of 9 does not count to a basin,
            // so is effectively the wall/frontier to it.
            if (value == 9)
            {
                basins.Walls.Add(point);
            }
            else
            {
                basins.Forests.Add(point);
            }
        }

        var lowPoints = new Dictionary<Point, int>();

        foreach (Point point in basins.Forests)
        {
            int lows = 0;
            int neighbors = 0;

            foreach (Point neighbor in basins.Neighbors(point))
            {
                neighbors++;

                if (heights[point] < heights[neighbor])
                {
                    lows++;
                }
            }

            if (lows == neighbors)
            {
                lowPoints[point] = heights[point];
            }
        }

        var graph = new Graph<Point>();

        foreach (Point point in heights.Keys)
        {
            var neighbors = new List<Point>(4);

            foreach (Point next in basins.Neighbors(point))
            {
                neighbors.Add(next);
            }

            graph.Edges[point] = neighbors;
        }

        var basinAreas = new List<int>(lowPoints.Count);

        foreach ((Point point, int value) in lowPoints)
        {
            var basin = PathFinding.BreadthFirst(graph, point);
            basinAreas.Add(basin.Count);
        }

        int sumOfRiskLevels = lowPoints.Values
            .Select((p) => p + 1)
            .Sum();

        int areaOfThreeLargestBasins = basinAreas
            .OrderByDescending((p) => p)
            .Take(3)
            .Aggregate((x, y) => x *= y);

        return (sumOfRiskLevels, areaOfThreeLargestBasins);
    }

    /// <inheritdoc />
    protected override async Task<PuzzleResult> SolveCoreAsync(string[] args, CancellationToken cancellationToken)
    {
        IList<string> heightmap = await ReadResourceAsLinesAsync();

        (SumOfRiskLevels, AreaOfThreeLargestBasins) = AnalyzeRisk(heightmap);

        if (Verbose)
        {
            Logger.WriteLine("The sum of the risk levels of all low points on the heightmap is {0:N0}.", SumOfRiskLevels);
            Logger.WriteLine("The area of the three largest basins in the heightmap is {0:N0}.", AreaOfThreeLargestBasins);
        }

        return PuzzleResult.Create(SumOfRiskLevels, AreaOfThreeLargestBasins);
    }
}