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

            return answer.ToString();
        }

        [Benchmark]
        public string Day10Part2()
        {
            using var sw = new StreamWriter(@"input2.txt");
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

                    sw.Write(Pipes.SymbolMap[c]);
                    Console.Write(Pipes.SymbolMap[c]);
                    Console.ResetColor();
                }

                sw.WriteLine();
                Console.WriteLine();
            }

            //var answer = Pipes.Walk(startPos, _input);
            var positions = Pipes.GetAllPipeLocations(startPos, _input);
            foreach (var pos in positions)
            {
                Console.WriteLine(pos);
            }

            var answer = 0;
            return answer.ToString();
        }
   }
}