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
        bool _debugMode;

        public Benchmark()
        {
        }

        public Benchmark(string inputFile, bool debugMode = false)
        {
            _input = File.ReadAllLines(inputFile);
            _debugMode = debugMode;
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
            var numsToAdd = new List<long>();

            foreach (var line in _input)
            {
                var col = 0;
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
                            DoColorCheck(
                                row,
                                coloredPoints,
                                currBuffer,
                                currLength,
                                numsToAdd,
                                col);
                        }

                        currLength = 0;
                    }

#if DEBUG
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
#endif
                    col++;
                }

                if (currLength > 0)
                {
                    DoColorCheck(
                        row,
                        coloredPoints,
                        currBuffer,
                        currLength,
                        numsToAdd,
                        col);
                }

                currLength = 0;
#if DEBUG
                Console.WriteLine();
#endif

                row++;
            }

            answer = numsToAdd.Sum();
            return answer.ToString();
        }

        [Benchmark]
        public string Day3Part2()
        {
            var answer = 0L;
            var row = 0;
            var coloredPoints = new Dictionary<(int row, int col), (int row, int col)>();
            var reverseLookup = new Dictionary<(int row, int col), List<int>>();

            // First pass, color all neighbors of symbols.
            foreach (var line in _input)
            {
                int col = 0;
                foreach (var c in line)
                {
                    var coords = (row, col);

                    if (IsPossibleGear(c))
                    {
                        ColorGearPoints(coords, coloredPoints);
                    }

                    col++;
                }
                row++;
            }

            // Second pass, determine each number that is colored.
            row = 0;
            var currBuffer = new char[8].AsSpan();
            var currLength = 0;

            foreach (var line in _input)
            {
                var col = 0;
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
                            DoGearColorCheck(
                                row,
                                coloredPoints,
                                reverseLookup,
                                currBuffer,
                                currLength,
                                col);
                        }

                        currLength = 0;
                    }

#if DEBUG
                    var coords = (row, col);
                    if (coloredPoints.ContainsKey(coords))
                    {
                        Console.BackgroundColor = ConsoleColor.DarkBlue;
                    }
                    else
                    {
                        Console.ResetColor();
                    }

                    Console.Write(c);
#endif
                    col++;
                }

                if (currLength > 0)
                {
                    DoGearColorCheck(
                        row,
                        coloredPoints,
                        reverseLookup,
                        currBuffer,
                        currLength,
                        col);
                }

                currLength = 0;
#if DEBUG
                Console.WriteLine();
#endif

                row++;
            }

            foreach (var item in reverseLookup)
            {
                if (item.Value.Count == 2)
                {
                    var gearRatio = 1;

                    foreach (var num in item.Value)
                    {
                        gearRatio *= num;
                    }

                    answer += gearRatio;
                }
            }

            return answer.ToString();
        }

        private bool IsSymbol(char c)
        {
            return !char.IsAsciiDigit(c) && c != '.';
        }

        private bool IsPossibleGear(char c)
        {
            return c == '*';
        }

        private static void DoColorCheck(
            int row,
            HashSet<(int row, int col)> coloredPoints,
            Span<char> currBuffer,
            int currLength,
            List<long> numsToAdd,
            int col)
        {
            var num = int.Parse(currBuffer.Slice(0, currLength));
            for (int i = col - currLength; i < col; i++)
            {
                var toCheck = (row, i);
                if (coloredPoints.Contains(toCheck))
                {
                    numsToAdd.Add(num);
                    break;
                }
            }
        }

        private static void DoGearColorCheck(
            int row,
            Dictionary<(int row, int col), (int row, int col)> coloredPoints,
            Dictionary<(int row, int col), List<int>> reverseLookup,
            Span<char> currBuffer,
            int currLength,
            int col)
        {
            var num = int.Parse(currBuffer.Slice(0, currLength));
            for (int i = col - currLength; i < col; i++)
            {
                var toCheck = (row, i);
                if (coloredPoints.TryGetValue(toCheck, out var item))
                {
                    if (reverseLookup.TryGetValue(item, out var list))
                    {
                        list.Add(num);
                    }
                    else
                    {
                        reverseLookup.Add(item, new List<int>());
                        reverseLookup[item].Add(num);
                    }

                    break;
                }
            }
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

        private void ColorGearPoints((int Row, int Col) symbolLocation, Dictionary<(int Row, int Col), (int row, int col)> coloredPoints)
        {
            coloredPoints.TryAdd((symbolLocation.Row, symbolLocation.Col + 1), symbolLocation); // Right
            coloredPoints.TryAdd((symbolLocation.Row - 1, symbolLocation.Col + 1), symbolLocation); // Up right
            coloredPoints.TryAdd((symbolLocation.Row - 1, symbolLocation.Col), symbolLocation); // Up
            coloredPoints.TryAdd((symbolLocation.Row - 1, symbolLocation.Col - 1), symbolLocation); // Up left
            coloredPoints.TryAdd((symbolLocation.Row, symbolLocation.Col - 1), symbolLocation); // Left
            coloredPoints.TryAdd((symbolLocation.Row + 1, symbolLocation.Col - 1), symbolLocation); // Down left
            coloredPoints.TryAdd((symbolLocation.Row + 1, symbolLocation.Col), symbolLocation); // Down
            coloredPoints.TryAdd((symbolLocation.Row + 1, symbolLocation.Col + 1), symbolLocation); // Down right
        }
    }
}