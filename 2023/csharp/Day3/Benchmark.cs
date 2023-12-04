namespace Test
{
    using BenchmarkDotNet.Attributes;
    using BenchmarkDotNet.Diagnosers;
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;

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
            var answer = 0L;
            var row = 0;
            var coloredPoints = new HashSet<(int row, int col)>();

            // First pass, color all neighbors of symbols.
            foreach (var line in _input)
            {
                int col = 0;
                foreach (var c in line)
                {
                    var coords = (row, col);

                    if (IsSymbol(c))
                    {
                        ColorPoints(coords, coloredPoints);
                    }

                    col++;
                }
                row++;
            }

            // Second pass, determine each number that is colored.
            row = 0;
            var currBuffer = new char[8].AsSpan();
            var currLength = 0;
            var mark = false;
            var numsToAdd = new List<long>();

            foreach (var line in _input)
            {
                var col = 0;
                foreach (var c in line)
                {
                    if (currLength > 0 && (!char.IsAsciiDigit(c) || col + 1 == line.Length))
                    {
                        var num = int.Parse(currBuffer.Slice(0, currLength));
                        for (int i = col - currLength; i < col; i++)
                        {
                            var toCheck = (row, i);
                            if (coloredPoints.Contains(toCheck))
                            {
                                mark = true;
                                numsToAdd.Add(num);
                                break;
                            }
                        }

                        currLength = 0;
                    }
                    else if (char.IsAsciiDigit(c))
                    {
                        currBuffer[currLength] = c;
                        currLength++;
                    }

                    var coords = (row, col);
                    if (coloredPoints.Contains(coords))
                    {
                        Console.BackgroundColor = ConsoleColor.DarkBlue;
                    }
                    else
                    {
                        Console.ResetColor();
                    }

                    Console.Write(c);

                    if (mark)
                    {
                        var pos = Console.GetCursorPosition();
                        Console.SetCursorPosition(pos.Left - 1, pos.Top);
                        Console.BackgroundColor = ConsoleColor.Red;
                        Console.Write(c);
                        Console.ResetColor();
                        mark = false;
                    }

                    col++;
                }

                currLength = 0;
                Console.WriteLine();

                row++;
            }

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

        private void ColorPoints((int Row, int Col) symbolLocation, HashSet<(int Row, int Col)> coloredPoints)
        {
            coloredPoints.Add((symbolLocation.Row, symbolLocation.Col + 1)); // Right
            coloredPoints.Add((symbolLocation.Row - 1, symbolLocation.Col + 1)); // Up right
            coloredPoints.Add((symbolLocation.Row - 1, symbolLocation.Col)); // Up
            coloredPoints.Add((symbolLocation.Row - 1, symbolLocation.Col - 1)); // Up left
            coloredPoints.Add((symbolLocation.Row, symbolLocation.Col - 1)); // Left
            coloredPoints.Add((symbolLocation.Row + 1, symbolLocation.Col - 1)); // Down left
            coloredPoints.Add((symbolLocation.Row + 1, symbolLocation.Col)); // Down
            coloredPoints.Add((symbolLocation.Row + 1, symbolLocation.Col + 1)); // Down right
        }
    }
}