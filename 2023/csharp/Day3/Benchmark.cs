using System.Numerics;
using System.Runtime.CompilerServices;
using System;

namespace Test
{
    using BenchmarkDotNet.Attributes;
    using BenchmarkDotNet.Diagnosers;
    using System.Text.RegularExpressions;
    using System.IO;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Collections.Immutable;

    [MemoryDiagnoser]
    public class Benchmark
    {
        string[] _input;

        public Benchmark()
        {
        }

        public Benchmark(string inputFile)
        {
            _input = File.ReadAllLines(inputFile);
        }

        [GlobalSetup]
        public void GlobalSetup()
        {
            _input = File.ReadAllLines("input.txt");
        }

        [Benchmark]
        public string Day3Part1()
        {
            var answer = 0;
            var x = 0;
            var coloredPoints = new HashSet<(int X, int Y)>();

            // First pass, color all neighbors of symbols.
            foreach (var line in _input)
            {
                int y = 0;
                foreach (var c in line)
                {
                    var coords = (x, y);

                    if (IsSymbol(c))
                    {
                        ColorPoints(coords, coloredPoints);
                    }

                    y++;
                }
                x++;
            }

            // Second pass, determine each number that is colored.
            x = 0;
            var currBuffer = new char[8].AsSpan();
            var currLength = 0;
            bool mark = false;
            var numsToAdd = new List<int>();

            foreach (var line in _input)
            {
                int y = 0;
                foreach (var c in line)
                {
                    if (char.IsAsciiDigit(c))
                    {
                        currBuffer[currLength] = c;
                        currLength++;
                    }
                    else
                    {
                        if (currLength > 0)
                        {
                            var num = int.Parse(currBuffer.Slice(0, currLength));
                            for (int i = y - currLength; i < y; i++)
                            {
                                var toCheck = (x, i);
                                if (coloredPoints.Contains(toCheck))
                                {
                                    mark = true;
                                    numsToAdd.Add(num);
                                    break;
                                }
                            }

                            currLength = 0;
                        }
                    }

                    var coords = (x, y);
                    if (coloredPoints.Contains(coords))
                    {
                        Console.BackgroundColor = ConsoleColor.DarkBlue;
                        Console.Write(c);
                    }
                    else
                    {
                        Console.ResetColor();
                        Console.Write(c);
                    }

                    if (mark)
                    {
                        var pos = Console.GetCursorPosition();
                        Console.SetCursorPosition(pos.Left - 1, pos.Top);
                        Console.BackgroundColor = ConsoleColor.Red;
                        Console.Write(c);
                        Console.ResetColor();
                        mark = false;
                    }

                    y++;
                }

                currLength = 0;

                Console.WriteLine();
                x++;
            }

            Console.WriteLine(numsToAdd.Max());

            answer = numsToAdd.Sum();
            return answer.ToString();
        }

        [Benchmark]
        public string Day3Part2()
        {
            var answer = 0;
            return answer.ToString();
        }

        private bool IsSymbol(char c)
        {
            return !char.IsAsciiDigit(c) && c != '.';
        }

        private void ColorPoints((int X, int Y) symbolLocation, HashSet<(int X, int Y)> coloredPoints)
        {
            coloredPoints.Add((symbolLocation.X, symbolLocation.Y + 1)); // Right
            coloredPoints.Add((symbolLocation.X - 1, symbolLocation.Y + 1)); // Up right
            coloredPoints.Add((symbolLocation.X - 1, symbolLocation.Y)); // Up
            coloredPoints.Add((symbolLocation.X - 1, symbolLocation.Y - 1)); // Up left
            coloredPoints.Add((symbolLocation.X, symbolLocation.Y - 1)); // Left
            coloredPoints.Add((symbolLocation.X + 1, symbolLocation.Y - 1)); // Down left
            coloredPoints.Add((symbolLocation.X + 1, symbolLocation.Y)); // Down
            coloredPoints.Add((symbolLocation.X + 1, symbolLocation.Y + 1)); // Down right
        }
   }
}