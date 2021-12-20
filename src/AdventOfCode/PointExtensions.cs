﻿// Copyright (c) Martin Costello, 2015. All rights reserved.
// Licensed under the Apache 2.0 license. See the LICENSE file in the project root for full license information.

namespace System.Drawing;

/// <summary>
/// A class containing extension methods for the <see cref="Point"/> structure. This class cannot be inherited.
/// </summary>
internal static class PointExtensions
{
    /// <summary>
    /// Returns the absolute Manhattan Distance of a point.
    /// </summary>
    /// <param name="value">The value.</param>
    /// <returns>
    /// The Manhattan Distance of <paramref name="value"/>.
    /// </returns>
    public static int ManhattanDistance(this Point value) => value.ManhattanDistance(Point.Empty);

    /// <summary>
    /// Returns the Manhattan Distance between two points.
    /// </summary>
    /// <param name="value">The origin value.</param>
    /// <param name="other">The value to calculate the distance to.</param>
    /// <returns>
    /// The Manhattan Distance between <paramref name="value"/> and <paramref name="other"/>.
    /// </returns>
    public static int ManhattanDistance(this Point value, Point other)
        => Math.Abs(value.X - other.X) + Math.Abs(value.Y - other.Y);

    /// <summary>
    /// Returns the neighbors in a 2D grid of the specified point from top-left to bottom-right.
    /// </summary>
    /// <param name="value">The value to get the neighbors of.</param>
    /// <param name="includeSelf">Whether to return the value itself.</param>
    /// <returns>
    /// A sequence which returns the neighbors of the specified point.
    /// </returns>
    public static IEnumerable<Point> Neighbors(this Point value, bool includeSelf = false)
    {
        yield return new(value.X - 1, value.Y - 1);
        yield return new(value.X, value.Y - 1);
        yield return new(value.X + 1, value.Y - 1);
        yield return new(value.X - 1, value.Y);

        if (includeSelf)
        {
            yield return value;
        }

        yield return new(value.X + 1, value.Y);
        yield return new(value.X - 1, value.Y + 1);
        yield return new(value.X, value.Y + 1);
        yield return new(value.X + 1, value.Y + 1);
    }
}
