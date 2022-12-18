﻿// Copyright (c) Martin Costello, 2015. All rights reserved.
// Licensed under the Apache 2.0 license. See the LICENSE file in the project root for full license information.

namespace MartinCostello.AdventOfCode.Puzzles.Y2022;

/// <summary>
/// A class representing the puzzle for <c>https://adventofcode.com/2022/day/18</c>. This class cannot be inherited.
/// </summary>
[Puzzle(2022, 18, "Boiling Boulders", RequiresData = true)]
public sealed class Day18 : Puzzle
{
    /// <summary>
    /// Gets the total surface area of the scanned lava droplet.
    /// </summary>
    public int TotalDropletSurfaceArea { get; private set; }

    /// <summary>
    /// Gets the external surface area of the scanned lava droplet.
    /// </summary>
    public int ExternalDropletSurfaceArea { get; private set; }

    /// <summary>
    /// Gets the total surface area of the lava droplet described by the specified cubes.
    /// </summary>
    /// <param name="cubes">The cubes that make up the lava droplet.</param>
    /// <param name="excludeInterior">Whether to exclude the internal surface area.</param>
    /// <returns>
    /// The total surface area of the lava droplet described by <paramref name="cubes"/>.
    /// </returns>
    public static int GetSurfaceArea(IList<string> cubes, bool excludeInterior)
    {
        var droplet = Parse(cubes);

        return GetTotalSurfaceArea(droplet, excludeInterior);

        static Droplet Parse(IList<string> cubes)
        {
            var points = cubes
                .Select((p) => p.AsNumberTriple<int>())
                .Select((p) => new Vector3(p.First, p.Second, p.Third));

            return new Droplet(points);
        }

        static int GetTotalSurfaceArea(Droplet droplet, bool excludeInterior)
        {
            if (droplet.Count == 1)
            {
                return 6;
            }

            var counts = new List<int>(droplet.Planes.Count);

            foreach ((var plane, var delta) in droplet.Planes)
            {
                var slice = new HashSet<Vector3>();

                for (float x = plane.Min((p) => p.X); x <= plane.Max((p) => p.X); x++)
                {
                    for (float y = plane.Min((p) => p.Y); y <= plane.Max((p) => p.Y); y++)
                    {
                        for (float z = plane.Min((p) => p.Z); z <= plane.Max((p) => p.Z); z++)
                        {
                            slice.Add(new(x, y, z));
                        }
                    }
                }

                float length = droplet.Length(delta);

                foreach (var point in slice)
                {
                    var line = droplet.Carve(point, delta);

                    if (line.Count > 0)
                    {
                        int count = 0;
                        bool lastWasEmpty = true;

                        for (int i = 0; i < length; i++)
                        {
                            var location = point + (delta * i);
                            bool found = line.Contains(location);

                            if (lastWasEmpty && found)
                            {
                                count++;
                            }

                            lastWasEmpty = !found && (!excludeInterior || !droplet.IsInternal(location));
                        }

                        counts.Add(count);
                    }
                }
            }

            return counts.Sum();
        }
    }

    /// <inheritdoc />
    protected override async Task<PuzzleResult> SolveCoreAsync(string[] args, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(args);

        var values = await ReadResourceAsLinesAsync();

        TotalDropletSurfaceArea = GetSurfaceArea(values, excludeInterior: false);
        ExternalDropletSurfaceArea = GetSurfaceArea(values, excludeInterior: true);

        if (Verbose)
        {
            Logger.WriteLine("The total surface area of the scanned lava droplet is {0}.", TotalDropletSurfaceArea);
            Logger.WriteLine("The external surface area of the scanned lava droplet is {0}.", ExternalDropletSurfaceArea);
        }

        return PuzzleResult.Create(TotalDropletSurfaceArea, ExternalDropletSurfaceArea);
    }

    private sealed class Droplet : HashSet<Vector3>
    {
        public Droplet(IEnumerable<Vector3> points)
            : base(points)
        {
            MinX = this.Min((p) => p.X);
            MaxX = this.Max((p) => p.X);
            MinY = this.Min((p) => p.Y);
            MaxY = this.Max((p) => p.Y);
            MinZ = this.Min((p) => p.Z);
            MaxZ = this.Max((p) => p.Z);

            Corners = new Vector3[]
            {
                new(MinX, MinY, MinZ),
                new(MinX, MinY, MaxZ),
                new(MinX, MaxY, MinZ),
                new(MinX, MaxY, MaxZ),
                new(MaxX, MinY, MinZ),
                new(MaxX, MinY, MaxZ),
                new(MaxX, MaxY, MinZ),
                new(MaxX, MaxY, MaxZ),
            };

            Planes = new (IReadOnlyList<Vector3> Plane, Vector3 Normal)[]
            {
                (new[] { Corners[0], Corners[1], Corners[2], Corners[3] }, Vector3.UnitX),
                (new[] { Corners[4], Corners[5], Corners[6], Corners[7] }, -Vector3.UnitX),
                (new[] { Corners[0], Corners[1], Corners[4], Corners[5] }, Vector3.UnitY),
                (new[] { Corners[2], Corners[3], Corners[6], Corners[7] }, -Vector3.UnitY),
                (new[] { Corners[0], Corners[2], Corners[4], Corners[6] }, Vector3.UnitZ),
                (new[] { Corners[1], Corners[3], Corners[5], Corners[7] }, -Vector3.UnitZ),
            };

            Bounds = new((int)(MaxX * MaxY * MaxZ));

            for (float x = MinX; x <= MaxX; x++)
            {
                for (float y = MinY; y <= MaxY; y++)
                {
                    for (float z = MinZ; z <= MaxZ; z++)
                    {
                        Bounds.Add(new(x, y, z));
                    }
                }
            }

            var negative = new HashSet<Vector3>(Bounds);
            negative.ExceptWith(this);

            if (negative.Count > 0)
            {
                var outside = new NegativeSpace(negative);
                var first = outside.First();

                var reachableFromFirst = new HashSet<Vector3>()
                {
                    first,
                };

                var notReachable = new HashSet<Vector3>();

                foreach (var other in outside)
                {
                    if (other == first || reachableFromFirst.Contains(other))
                    {
                        continue;
                    }

                    if (PathFinding.AStar(outside, first, other) != long.MaxValue)
                    {
                        reachableFromFirst.Add(other);
                    }
                    else
                    {
                        notReachable.Add(other);
                    }
                }

                int minimum = Math.Min(notReachable.Count, reachableFromFirst.Count);
                var smallest = minimum == reachableFromFirst.Count || minimum == 0 ? reachableFromFirst : notReachable;
                Pockets = new(smallest);
            }
            else
            {
                Pockets = new();
            }
        }

        public IReadOnlyList<Vector3> Corners { get; }

        public IReadOnlyList<(IReadOnlyList<Vector3> Plane, Vector3 Normal)> Planes { get; }

        public float MinX { get; }

        public float MaxX { get; }

        public float MinY { get; }

        public float MaxY { get; }

        public float MinZ { get; }

        public float MaxZ { get; }

        public float LengthX => Math.Abs(MaxX - MinX) + 1;

        public float LengthY => Math.Abs(MaxY - MinY) + 1;

        public float LengthZ => Math.Abs(MaxZ - MinZ) + 1;

        private HashSet<Vector3> Bounds { get; }

        private HashSet<Vector3> Pockets { get; }

        public float Length(Vector3 dimension)
        {
            dimension = Vector3.Abs(dimension);

            if (dimension == Vector3.UnitX)
            {
                return LengthX;
            }
            else if (dimension == Vector3.UnitY)
            {
                return LengthY;
            }
            else if (dimension == Vector3.UnitZ)
            {
                return LengthZ;
            }
            else
            {
                throw new ArgumentException($"The specified dimension {dimension} is not valid.", nameof(dimension));
            }
        }

        public HashSet<Vector3> Carve(Vector3 position, Vector3 direction)
        {
            var result = new HashSet<Vector3>();

            while (Bounds.Contains(position))
            {
                if (Contains(position))
                {
                    result.Add(position);
                }

                position += direction;
            }

            return result;
        }

        public bool IsInternal(Vector3 position)
            => Pockets.Contains(position);
    }

    private sealed class NegativeSpace : HashSet<Vector3>, IWeightedGraph<Vector3>
    {
        public NegativeSpace(IEnumerable<Vector3> points)
            : base(points)
        {
        }

        public long Cost(Vector3 a, Vector3 b) => (long)a.ManhattanDistance(b);

        public IEnumerable<Vector3> Neighbors(Vector3 id)
        {
            var next = id + Vector3.UnitX;

            if (Contains(next))
            {
                yield return next;
            }

            next = id + Vector3.UnitY;

            if (Contains(next))
            {
                yield return next;
            }

            next = id + Vector3.UnitZ;

            if (Contains(next))
            {
                yield return next;
            }

            next = id - Vector3.UnitX;

            if (Contains(next))
            {
                yield return next;
            }

            next = id - Vector3.UnitY;

            if (Contains(next))
            {
                yield return next;
            }

            next = id - Vector3.UnitZ;

            if (Contains(next))
            {
                yield return next;
            }
        }
    }
}
