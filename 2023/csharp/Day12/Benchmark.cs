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
        public string Day12Part1()
        {
            var answer = 0;

            foreach (var line in _input)
            {
                var checker = new SpringChecker(line);
                answer += checker.DoCheck();
            }

            return answer.ToString();
        }

        [Benchmark]
        public string Day12Part2()
        {
            var answer = 0;
            return answer.ToString();
        }
   }
}