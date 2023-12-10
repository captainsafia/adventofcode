﻿// Copyright (c) Martin Costello, 2015. All rights reserved.
// Licensed under the Apache 2.0 license. See the LICENSE file in the project root for full license information.

using System.Diagnostics;

namespace MartinCostello.AdventOfCode.Puzzles.Y2023;

/// <summary>
/// A class representing the puzzle for <c>https://adventofcode.com/2023/day/10</c>. This class cannot be inherited.
/// </summary>
[Puzzle(2023, 10, "Pipe Maze", RequiresData = true)]
public sealed class Day10 : Puzzle
{
    private static readonly Size[] Directions =
    [
        new(-1, 0),
        new(1, 0),
        new(0, -1),
        new(0, 1),
    ];

    /// <summary>
    /// Gets the number of steps to the furthest position in the pipe from the starting location.
    /// </summary>
    public int Steps { get; private set; }

    /// <summary>
    /// Gets the number of tiles enclosed by the loop of pipe containing the animal.
    /// </summary>
    public int Tiles { get; private set; }

    /// <summary>
    /// Walks the pipe maze and returns the number of steps to
    /// the furthest position from the starting location.
    /// </summary>
    /// <param name="sketch">The sketch of the pipe maze.</param>
    /// <param name="cancellationToken">The cancellation token to use.</param>
    /// <returns>
    /// The number of steps to the furthest position in the pipe
    /// from the starting location and the number of tiles enclosed
    /// by the loop of pipe containing the animal.
    /// </returns>
    public static (int Steps, int Tiles) Walk(IList<string> sketch, CancellationToken cancellationToken)
    {
        (Point start, Rectangle bounds) = FindStart(sketch);
        char startPipe = GetStartShape(start, bounds, sketch);

        var maze = new Graph<Point>();
        var tiles = new HashSet<Point>();

        for (int y = 0; y < sketch.Count; y++)
        {
            string row = sketch[y];

            for (int x = 0; x < row.Length; x++)
            {
                char pipe = row[x];

                var location = new Point(x, y);
                var connections = maze.Edges[location] = [];

                if (pipe is 'S')
                {
                    pipe = startPipe;
                }
                else if (pipe is '.')
                {
                    tiles.Add(location);
                }

                foreach (var offset in Directions)
                {
                    var neighbor = location + offset;

                    if (!bounds.Contains(neighbor))
                    {
                        continue;
                    }

                    char other = sketch[neighbor.Y][neighbor.X];

                    if (other is 'S')
                    {
                        other = startPipe;
                    }

                    if (CanConnect((location, pipe), (neighbor, other)))
                    {
                        connections.Add(neighbor);
                    }
                }
            }
        }

        var mainLoop = PathFinding.BreadthFirst(maze, start, cancellationToken);
        int steps = mainLoop.Count / 2;

        List<Point> maybeEnclosed = [];

        foreach (var location in tiles)
        {
            var space = PathFinding.BreadthFirst(maze, location, cancellationToken);

            if (space.All((p) => p.Neighbors().All((r) => mainLoop.Contains(r) || tiles.Contains(r))))
            {
                maybeEnclosed.Add(location);
            }
        }

        // 7F    J-L
        // ||    7-F
        // JL
        foreach (var location in maybeEnclosed)
        {
        }

        return (steps, maybeEnclosed.Count);

        static (Point Location, Rectangle Bounds) FindStart(IList<string> sketch)
        {
            var start = Point.Empty;
            var bounds = new Rectangle(0, 0, sketch[0].Length, sketch.Count);

            for (int y = 0; y < sketch.Count; y++)
            {
                string row = sketch[y];

                for (int x = 0; x < row.Length; x++)
                {
                    if (row[x] is 'S')
                    {
                        return (new(x, y), bounds);
                    }
                }
            }

            throw new UnreachableException("Failed to find the starting location.");
        }

        static char GetStartShape(Point start, Rectangle bounds, IList<string> sketch)
        {
            char[] candidates = ['|', '-', 'L', 'J', '7', 'F'];

            foreach (char candidate in candidates)
            {
                int connections = 0;

                foreach (var offset in Directions)
                {
                    var neighbor = start + offset;

                    if (!bounds.Contains(neighbor))
                    {
                        continue;
                    }

                    char other = sketch[neighbor.Y][neighbor.X];

                    if (CanConnect((start, candidate), (neighbor, other)))
                    {
                        connections++;
                    }
                }

                if (connections == 2)
                {
                    return candidate;
                }
            }

            throw new UnreachableException("Failed to find the type of the starting location's pipe.");
        }

        static bool CanConnect((Point Location, char Pipe) origin, (Point Location, char Pipe) other)
        {
            return origin.Pipe switch
            {
                '.' => other.Pipe switch
                {
                    '.' => true,
                    _ => false,
                },
                '|' => other.Pipe switch
                {
                    '|' => other.Location.IsAbove(origin.Location) || other.Location.IsBelow(origin.Location),
                    'L' => other.Location.IsBelow(origin.Location),
                    'J' => other.Location.IsBelow(origin.Location),
                    '7' => other.Location.IsAbove(origin.Location),
                    'F' => other.Location.IsAbove(origin.Location),
                    _ => false,
                },
                '-' => other.Pipe switch
                {
                    '-' => other.Location.IsLeftOf(origin.Location) || other.Location.IsRightOf(origin.Location),
                    'L' => other.Location.IsLeftOf(origin.Location),
                    'J' => other.Location.IsRightOf(origin.Location),
                    '7' => other.Location.IsRightOf(origin.Location),
                    'F' => other.Location.IsLeftOf(origin.Location),
                    _ => false,
                },
                'L' => other.Pipe switch
                {
                    '|' => other.Location.IsAbove(origin.Location),
                    '-' => other.Location.IsRightOf(origin.Location),
                    'J' => other.Location.IsRightOf(origin.Location),
                    '7' => other.Location.IsAbove(origin.Location) || other.Location.IsRightOf(origin.Location),
                    'F' => other.Location.IsAbove(origin.Location),
                    _ => false,
                },
                'J' => other.Pipe switch
                {
                    '|' => other.Location.IsAbove(origin.Location),
                    '-' => other.Location.IsLeftOf(origin.Location),
                    'L' => other.Location.IsLeftOf(origin.Location),
                    '7' => other.Location.IsAbove(origin.Location),
                    'F' => other.Location.IsAbove(origin.Location) || other.Location.IsLeftOf(origin.Location),
                    _ => false,
                },
                '7' => other.Pipe switch
                {
                    '|' => other.Location.IsBelow(origin.Location),
                    '-' => other.Location.IsLeftOf(origin.Location),
                    'L' => other.Location.IsBelow(origin.Location) || other.Location.IsLeftOf(origin.Location),
                    'J' => other.Location.IsBelow(origin.Location),
                    'F' => other.Location.IsLeftOf(origin.Location),
                    _ => false,
                },
                'F' => other.Pipe switch
                {
                    '|' => other.Location.IsBelow(origin.Location),
                    '-' => other.Location.IsRightOf(origin.Location),
                    'L' => other.Location.IsBelow(origin.Location),
                    'J' => other.Location.IsBelow(origin.Location) || other.Location.IsRightOf(origin.Location),
                    '7' => other.Location.IsRightOf(origin.Location),
                    _ => false,
                },
                _ => false,
            };
        }
    }

    /// <inheritdoc />
    protected override async Task<PuzzleResult> SolveCoreAsync(string[] args, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(args);

        var values = await ReadResourceAsLinesAsync(cancellationToken);

        (Steps, Tiles) = Walk(values, cancellationToken);

        if (Verbose)
        {
            Logger.WriteLine("It takes {0} steps to get to the point furthest from the starting position.", Steps);
            Logger.WriteLine("{0} tiles are enclosed by the loop.", Tiles);
        }

        return PuzzleResult.Create(Steps, Tiles);
    }
}
