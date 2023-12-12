namespace Test
{
    using BenchmarkDotNet.Attributes;
    using BenchmarkDotNet.Diagnosers;
    using Microsoft.CodeAnalysis.CSharp.Syntax;
    using System;
    using System.Collections.Concurrent;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Text;
    using System.Threading;
    using System.Threading.Tasks;

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
        public string Day11Part1()
        {
            var expanded = ExpandUniverse(_input);
            var positions = GetGalaxyPositions(expanded);
            var pairs = GetPairs(positions);
            var pathLengths = new List<long>();

            foreach (var pair in pairs)
            {
                pathLengths.Add(GetPathLength(pair.Item1, pair.Item2));
            }

            // Should probably not compare the same pair twice but for now just divide by 2.
            var answer = pathLengths.Sum() / 2;
            return answer.ToString();
        }

        [Benchmark]
        public string Day11Part2()
        {
            var expanded = ExpandUniversePart2(_input, 1_000_000);
            var positions = GetGalaxyPositionsPart2(expanded);
            var pairs = GetPairs(positions);
            var pathLengths = new List<long>();

            foreach (var pair in pairs)
            {
                pathLengths.Add(GetPathLength(pair.Item1, pair.Item2));
            }

            // Should probably not compare the same pair twice but for now just divide by 2.
            var answer = pathLengths.Sum() / 2;
            return answer.ToString();
        }

        long GetPathLength((int, int) A, (int, int) B)
        {
            var rowA = A.Item1;
            var colA = A.Item2;
            var rowB = B.Item1;
            var colB = B.Item2;

            var rowDiff = 0;
            var colDiff = 0;
            if (rowA > rowB)
            {
                rowDiff = rowA - rowB;
            }
            else
            {
                rowDiff = rowB - rowA;
            }

            if (colA > colB)
            {
                colDiff = colA - colB;
            }
            else
            {
                colDiff = colB - colA;
            }

            return rowDiff + colDiff;
        }

        IEnumerable<((int, int), (int, int))> GetPairs(List<(int, int)> input)
        {
            for (int i = 0; i < input.Count; i++)
            {
                for (int j = 0; j < input.Count; j++)
                {
                    if (i == j)
                    {
                        continue;
                    }

                    yield return ((input[i], input[j]));
                }
            }
        }

        List<(int, int)> GetGalaxyPositions(string[] input)
        {
            var result = new List<(int, int)>();

            for (int i = 0; i < input.Length; i++)
            {
                for (int j = 0; j < input[i].Length; j++)
                {
                    if (input[i][j] == '#')
                    {
                        result.Add((i, j));
                    }
                }
            }

            return result;
        }

        List<(int, int)> GetGalaxyPositionsPart2(string[] input)
        {
            var result = new ConcurrentBag<(int, int)>();

            Parallel.For(0, input.Length, x =>
            {
                if (input[x].StartsWith('!'))
                {
                    return;
                }

                var loc = input[x].IndexOf('#');

                if (loc > -1)
                {
                    for (int i = loc; i < input[x].Length; i++)
                    {
                        if (input[x][i] == '#')
                        {
                            result.Add((x, i));
                        }
                    }
                }
            });

            return result.ToList();
        }

        string[] ExpandUniverse(string[] input)
        {
            var tempStrings = new List<string>();

            // Expand columns
            var temp = Transpose(input);
            foreach (var line in temp)
            {
                tempStrings.Add(line);
                if (!line.Contains("#"))
                {
                    tempStrings.Add(line);
                }
            }

            // Expand rows
            var transposedBack = Transpose(tempStrings.ToArray());
            tempStrings.Clear();
            foreach (var line in transposedBack)
            {
                tempStrings.Add(line);
                if (!line.Contains("#"))
                {
                    tempStrings.Add(line);
                }
            }

            return tempStrings.ToArray();
        }

        string[] ExpandUniversePart2(string[] input, int factor)
        {
            var tempStrings = new List<string>();

            // Expand columns
            var temp = Transpose(input);
            var repstr = string.Empty;
            foreach (var line in temp)
            {
                tempStrings.Add(line);
                if (!line.Contains("#"))
                {
                    for (int i = 0; i < factor - 1; i++)
                    {
                        tempStrings.Add(line);
                    }
                }
            }

            // Expand rows
            var transposedBack = Transpose(tempStrings.ToArray());
            tempStrings.Clear();
            foreach (var line in transposedBack)
            {
                tempStrings.Add(line);
                if (!line.Contains("#"))
                {
                    if (repstr == string.Empty)
                    {
                        var arr = line.ToCharArray();
                        arr[0] = '!';
                        repstr = new string(arr);
                    }

                    for (int i = 0; i < factor - 1; i++)
                    {
                        tempStrings.Add(repstr);
                    }
                }
            }

            Console.WriteLine("Expanded!");

            return tempStrings.ToArray();
        }

        string[] Transpose(string[] input)
        {
            var res = new List<string>();
            var sb = new StringBuilder();

            for (int col = 0; col < input[0].Length; col++)
            {
                for (int i = 0; i < input.Length; i++)
                {
                    sb.Append(input[i][col]);
                }

                res.Add(sb.ToString());
                sb.Clear();
            }

            return res.ToArray();
        }

        void Print(string[] input)
        {
            foreach (var str in input)
            {
                Console.WriteLine(str);
            }
        }
    }
}