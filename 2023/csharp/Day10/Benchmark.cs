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
    using System.Text;
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
        public string Day10Part1()
        {
            var startPos = (0, 0);
            var row = -1;
            var col = -1;

            foreach (var line in _input)
            {
                row++;
                col = -1;
                foreach (var c in line)
                {
                    col++;
                    if (c == 'S')
                    {
                        startPos = (row, col);
                        Console.BackgroundColor = ConsoleColor.Green;
                    }

                    Console.Write(Pipes.SymbolMap[c]);
                    Console.ResetColor();
                }

                Console.WriteLine();
            }

            var answer = Pipes.WalkPartOne(startPos, _input);
            var answer2 = Pipes.WalkBackToStart(startPos, _input);

            Console.WriteLine(answer);
            Console.WriteLine(answer2);
            Console.ReadKey();

            return answer.ToString();
        }

        [Benchmark]
        public string Day10Part2()
        {
            var startPos = (0, 0);
            var row = -1;
            var col = -1;

            foreach (var line in _input)
            {
                row++;
                col = -1;
                foreach (var c in line)
                {
                    col++;
                    if (c == 'S')
                    {
                        startPos = (row, col);
                        Console.BackgroundColor = ConsoleColor.Green;
                    }

                    //Console.Write(Pipes.SymbolMap[c]);
                    Console.ResetColor();
                }

                //Console.WriteLine();
            }

            //var answer = Pipes.Walk(startPos, _input);
            var positions = Pipes.GetAllPipeLocations(startPos, _input);
            var grid = Pipes.GetGrid(_input);
            var scaledGrid = Pipes.RescaleGrid(grid, 2);

            var sb = new StringBuilder();

            for (int i = 0; i < grid.Length; i++)
            {
                for (int j = 0; j < grid[i].Length; j++)
                {
                    if (positions.Contains((i, j)))
                    {
                        scaledGrid[i * 2][j * 2] = (Pipes.SymbolMap[grid[i][j]]);
                    }
                }
            }

            Pipes.WalkAndFill((startPos.Item1 * 2, startPos.Item2 * 2), scaledGrid);
            Pipes.FloodFill(scaledGrid, '.', '~');

            for (int i = 0; i < scaledGrid.Length; i++)
            {
                for (int j = 0; j < scaledGrid[i].Length; j++)
                {
                    sb.Append(scaledGrid[i][j]);
                }

                sb.Append("\n");
            }

            var str = sb.ToString();
            Console.WriteLine(str);

            var scaledGrid2 = Pipes.RescaleGrid(scaledGrid, -2);

            sb.Clear();

            for (int i = 0; i < scaledGrid2.Length; i++)
            {
                for (int j = 0; j < scaledGrid2[i].Length; j++)
                {
                    sb.Append(scaledGrid[i * 2][j * 2]);
                }

                sb.Append("\n");
            }

            str = sb.ToString();
            Console.WriteLine(str);

            var answer = str.Count(x => x == '.');
            return answer.ToString();
        }
   }
}