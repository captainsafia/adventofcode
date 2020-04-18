﻿// Copyright (c) Martin Costello, 2015. All rights reserved.
// Licensed under the Apache 2.0 license. See the LICENSE file in the project root for full license information.

namespace MartinCostello.AdventOfCode.Puzzles.Y2019
{
    using System.Collections.Generic;
    using System.Text;

    /// <summary>
    /// A class representing the puzzle for <c>https://adventofcode.com/2019/day/8</c>. This class cannot be inherited.
    /// </summary>
    public sealed class Day08 : Puzzle2019
    {
        /// <summary>
        /// Gets the checksum of the image.
        /// </summary>
        public int Checksum { get; private set; }

        /// <summary>
        /// Gets the checksum for the specified image.
        /// </summary>
        /// <param name="image">The image data to get the checksum for.</param>
        /// <param name="height">The height of the image.</param>
        /// <param name="width">The width of the image.</param>
        /// <param name="logger">The optional logger to use.</param>
        /// <returns>
        /// The checksum of the image data.
        /// </returns>
        public static int GetImageChecksum(string image, int height, int width, ILogger? logger = null)
        {
            var layers = new List<int[,]>();
            int[,] current = new int[0, 0];

            int pixelsPerLayer = height * width;

            int i = 0;
            int x = 0;
            int y = 0;

            for (; i < image.Length; i++)
            {
                if (i % pixelsPerLayer == 0)
                {
                    current = new int[width, height];
                    layers.Add(current);
                    y = 0;
                }

                int value = image[i] - '0';

                current[x, y] = value;

                if (x == width - 1)
                {
                    x = 0;
                    y++;
                }
                else
                {
                    x++;
                }
            }

            int countOfZeroesInLastLayer = int.MaxValue;
            int indexOfLayerWithLeastZeros = -1;

            for (i = 0; i < layers.Count; i++)
            {
                int countOfZeroes = GetCountOfDigit(layers[i], 0);

                if (countOfZeroes < countOfZeroesInLastLayer)
                {
                    countOfZeroesInLastLayer = countOfZeroes;
                    indexOfLayerWithLeastZeros = i;
                }
            }

            int[,] layerWithLeastZeroes = layers[indexOfLayerWithLeastZeros];
            int ones = GetCountOfDigit(layerWithLeastZeroes, 1);
            int twos = GetCountOfDigit(layerWithLeastZeroes, 2);

            int checksum = ones * twos;

            int[,] finalLayer = (int[,])layers[^1].Clone();

            for (i = layers.Count - 2; i >= 0; i--)
            {
                int[,] layer = layers[i];

                for (x = 0; x < width; x++)
                {
                    for (y = 0; y < height; y++)
                    {
                        int background = finalLayer[x, y];
                        int foreground = layer[x, y];

                        if (foreground != 2)
                        {
                            finalLayer[x, y] = foreground;
                        }
                    }
                }
            }

            WriteMessage(finalLayer, logger);

            return checksum;
        }

        /// <inheritdoc />
        protected override int SolveCore(string[] args)
        {
            string image = ReadResourceAsString().TrimEnd('\n');

            Checksum = GetImageChecksum(image, 6, 25, Logger);

            if (Verbose)
            {
                Logger.WriteLine("The checksum of the image data is {0}.", Checksum);
            }

            return 0;
        }

        /// <summary>
        /// Gets the number of occurences of the specified digit in the array.
        /// </summary>
        /// <param name="layer">The array representing the layer to count the digits in.</param>
        /// <param name="digit">The digit to count in the layer.</param>
        /// <returns>
        /// The number of occurences of the digit specified by <paramref name="digit"/> in the array.
        /// </returns>
        private static int GetCountOfDigit(int[,] layer, int digit)
        {
            int count = 0;
            int width = layer.GetLength(0);
            int height = layer.GetLength(1);

            for (int x = 0; x < width; x++)
            {
                for (int y = 0; y < height; y++)
                {
                    if (layer[x, y] == digit)
                    {
                        count++;
                    }
                }
            }

            return count;
        }

        /// <summary>
        /// Writes the specified message.
        /// </summary>
        /// <param name="message">The message to write.</param>
        /// <param name="logger">The logger to write the message to.</param>
        private static void WriteMessage(int[,] message, ILogger? logger)
        {
            if (logger == null)
            {
                return;
            }

            int width = message.GetLength(0);
            int height = message.GetLength(1);

            for (int y = 0; y < height; y++)
            {
                var builder = new StringBuilder();

                for (int x = 0; x < width; x++)
                {
                    builder.Append(message[x, y] == 1 ? 'x' : ' ');
                }

                logger.WriteLine(builder.ToString());
            }
        }
    }
}