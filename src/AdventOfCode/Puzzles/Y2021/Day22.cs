﻿// Copyright (c) Martin Costello, 2015. All rights reserved.
// Licensed under the Apache 2.0 license. See the LICENSE file in the project root for full license information.

using System.Numerics;

namespace MartinCostello.AdventOfCode.Puzzles.Y2021;

/// <summary>
/// A class representing the puzzle for <c>https://adventofcode.com/2021/day/22</c>. This class cannot be inherited.
/// </summary>
[Puzzle(2021, 22, "Reactor Reboot", RequiresData = true)]
public sealed class Day22 : Puzzle
{
    /// <summary>
    /// Gets the number of cubes that are on once the reactor has been initialized.
    /// </summary>
    public long InitializedCubeCount { get; private set; }

    /// <summary>
    /// Gets the number of cubes that are on once the reactor has been rebooted.
    /// </summary>
    public long RebootedCubeCount { get; private set; }

    /// <summary>
    /// Reboots the reactor.
    /// </summary>
    /// <param name="instructions">The instructions to follow to reboot the reactor.</param>
    /// <param name="initialize">Whether to only initialize the reactor, rather than fully reboot it.</param>
    /// <returns>
    /// The number of cubes that are on once the reactor has been rebooted.
    /// </returns>
    public static long Reboot(IList<string> instructions, bool initialize)
    {
        var cuboids = new List<(Cuboid Cuboid, bool TurnOn)>(instructions.Count);

        foreach (string instruction in instructions)
        {
            var parsed = Parse(instruction);

            if (parsed.TurnOn || cuboids.Count > 0)
            {
                cuboids.Add(Parse(instruction));
            }
        }

        return initialize ? Initialize(cuboids) : Reboot(cuboids);

        static long Initialize(List<(Cuboid Cuboid, bool TurnOn)> cuboids)
        {
            var bounds = new Cuboid(new(-50, -50, -50), new(100, 100, 100));
            var reactor = new HashSet<Vector3>();

            foreach ((Cuboid cuboid, bool turnOn) in cuboids)
            {
                var points = cuboid.IntersectWith(bounds);

                if (turnOn)
                {
                    reactor.UnionWith(points);
                }
                else
                {
                    reactor.ExceptWith(points);
                }
            }

            return reactor.Count;
        }

        static long Reboot(List<(Cuboid Cuboid, bool TurnOn)> cuboids)
        {
            var reactor = new Dictionary<Cuboid, long>(cuboids.Count * 2);

            foreach ((Cuboid cuboid, bool turnOn) in cuboids)
            {
                var newCuboids = new Dictionary<Cuboid, long>();

                // Remove the new cuboid from each existing cuboid.
                // This will effectively perform the operation to
                // turn it off if that is the required action.
                foreach ((Cuboid existing, long count) in reactor)
                {
                    Cuboid? abjunction = cuboid.Abjunction(existing);

                    if (abjunction is { } value)
                    {
                        newCuboids.AddOrDecrement(value, -count, count);
                    }
                }

                if (turnOn)
                {
                    newCuboids.AddOrIncrement(cuboid, 1);
                }

                if (newCuboids.Count > 0)
                {
                    reactor.EnsureCapacity(reactor.Count + (newCuboids.Count * 2));

                    foreach ((Cuboid newCuboid, long count) in newCuboids)
                    {
                        reactor.AddOrIncrement(newCuboid, count, count);
                    }
                }
            }

            return reactor.Sum((p) => p.Key.Volume() * p.Value);
        }

        static (Cuboid Cuboid, bool TurnOn) Parse(string instruction)
        {
            (string state, string coordinates) = instruction.Bifurcate(' ');
            (string rangeX, string rangeY, string rangeZ) = coordinates.Trifurcate(',');

            const string Range = "..";
            string[] valuesX = rangeX[2..].Split(Range);
            string[] valuesY = rangeY[2..].Split(Range);
            string[] valuesZ = rangeZ[2..].Split(Range);

            int minX = Parse<int>(valuesX[0]);
            int maxX = Parse<int>(valuesX[1]);

            int minY = Parse<int>(valuesY[0]);
            int maxY = Parse<int>(valuesY[1]);

            int minZ = Parse<int>(valuesZ[0]);
            int maxZ = Parse<int>(valuesZ[1]);

            int lengthX = maxX - minX + 1;
            int lengthY = maxY - minY + 1;
            int lengthZ = maxZ - minZ + 1;

            var cuboid = new Cuboid(new(minX, minY, minZ), new(lengthX, lengthY, lengthZ));
            bool turnOn = string.Equals(state, "on", StringComparison.Ordinal);

            return (cuboid, turnOn);
        }
    }

    /// <inheritdoc />
    protected override async Task<PuzzleResult> SolveCoreAsync(string[] args, CancellationToken cancellationToken)
    {
        IList<string> instructions = await ReadResourceAsLinesAsync();

        InitializedCubeCount = Reboot(instructions, initialize: true);
        RebootedCubeCount = Reboot(instructions, initialize: false);

        if (Verbose)
        {
            Logger.WriteLine("{0:N0} cubes in the reactor are on after initialization.", InitializedCubeCount);
            Logger.WriteLine("{0:N0} cubes in the reactor are on after reboot.", RebootedCubeCount);
        }

        return PuzzleResult.Create(InitializedCubeCount, RebootedCubeCount);
    }

    [System.Diagnostics.DebuggerDisplay("({Origin.X}, {Origin.Y}, {Origin.Z}), ({Length.X}, {Length.Y}, {Length.Z})")]
    private readonly struct Cuboid
    {
        public readonly Vector3 Origin;

        public readonly Vector3 Length;

        public Cuboid(Vector3 origin, Vector3 length)
        {
            Origin = origin;
            Length = length;
        }

        public override readonly int GetHashCode()
            => HashCode.Combine(Origin.GetHashCode(), Length.GetHashCode());

        public readonly bool Contains(in Vector3 point)
            => Origin.X <= point.X &&
               point.X <= Origin.X + Length.X &&
               Origin.Y <= point.Y &&
               point.Y <= Origin.Y + Length.Y &&
               Origin.Z <= point.Z &&
               point.Z <= Origin.Z + Length.Z;

        public readonly Cuboid? Abjunction(in Cuboid other)
        {
            float minX = Math.Max(Origin.X, other.Origin.X);
            float maxX = Math.Min(Origin.X + Length.X, other.Origin.X + other.Length.X);
            float minY = Math.Max(Origin.Y, other.Origin.Y);
            float maxY = Math.Min(Origin.Y + Length.Y, other.Origin.Y + other.Length.Y);
            float minZ = Math.Max(Origin.Z, other.Origin.Z);
            float maxZ = Math.Min(Origin.Z + Length.Z, other.Origin.Z + other.Length.Z);

            if (minX <= maxX && minY <= maxY && minZ <= maxZ)
            {
                var origin = new Vector3(minX, minY, minZ);
                var length = new Vector3(maxX - minX, maxY - minY, maxZ - minZ);

                return new(origin, length);
            }

            return null;
        }

        public readonly bool IntersectsWith(in Cuboid other)
        {
            long volumeThis = Volume();
            long volumeOther = other.Volume();

            bool otherIsLarger = volumeOther > volumeThis;
            Cuboid largest = otherIsLarger ? other : this;
            Cuboid smallest = !otherIsLarger ? other : this;

            foreach (Vector3 vertex in smallest.Verticies())
            {
                if (largest.Contains(vertex))
                {
                    return true;
                }
            }

            return false;
        }

        public readonly HashSet<Vector3> IntersectWith(in Cuboid other)
        {
            var result = new HashSet<Vector3>();

            if (!IntersectsWith(other))
            {
                return result;
            }

            long volumeThis = Volume();
            long volumeOther = other.Volume();

            bool otherIsLarger = volumeOther > volumeThis;
            Cuboid largest = otherIsLarger ? other : this;
            Cuboid smallest = !otherIsLarger ? other : this;

            float lengthX = smallest.Origin.X + smallest.Length.X;
            float lengthY = smallest.Origin.Y + smallest.Length.Y;
            float lengthZ = smallest.Origin.Z + smallest.Length.Z;

            for (float z = Origin.Z; z < lengthZ; z++)
            {
                for (float y = Origin.Y; y < lengthY; y++)
                {
                    for (float x = Origin.X; x < lengthX; x++)
                    {
                        var point = new Vector3(x, y, z);

                        if (largest.Contains(point))
                        {
                            result.Add(point);
                        }
                    }
                }
            }

            return result;
        }

        public readonly long Volume() => (long)(Length.X * Length.Y * Length.Z);

        private readonly IEnumerable<Vector3> Verticies()
        {
            yield return Origin;
            yield return Origin + new Vector3(Length.X, 0, 0);
            yield return Origin + new Vector3(0, Length.Y, 0);
            yield return Origin + new Vector3(0, 0, Length.Z);
            yield return Origin + new Vector3(0, Length.Y, Length.Z);
            yield return Origin + new Vector3(Length.X, 0, Length.Z);
            yield return Origin + new Vector3(Length.X, Length.Y, 0);
            yield return Origin + new Vector3(Length.X, Length.Y, Length.Z);
        }
    }
}
