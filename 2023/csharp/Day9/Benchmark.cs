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
        public string Day9Part1()
        {
            var answer = 0L;

            foreach (var line in _input)
            {
                answer += ProcessLine(line, false);
            }

            return answer.ToString();
        }

        [Benchmark]
        public string Day9Part2()
        {
            var answer = 0L;

            foreach (var line in _input)
            {
                answer += ProcessLine(line, true);
            }

            return answer.ToString();
        }

        long ProcessLine(string line, bool reverse)
        {
            var nums = line.Split(" ").Select(x => long.Parse(x)).ToArray();
            var sequence = reverse ? GenerateNextSequence(nums.Reverse().ToArray()) : GenerateNextSequence(nums.ToArray());
            var answer = sequence[sequence.Length - 1];
            sequence = reverse ? sequence.Reverse().ToArray() : sequence.ToArray();
            Console.WriteLine(string.Join(',', sequence));
            return answer;
        }

        long[] GenerateNextSequence(long[] input)
        {
            if (input.All(x => x == 0))
            {
                return input;
            }

            long[] result = new long[input.Length + 1];
            Array.Copy(input, result, input.Length);

            long[] tempResult = new long[input.Length - 1];
            for (int i = 0; i < input.Length - 1; i++)
            {
                var x = input[i];
                var y = input[i + 1];
                tempResult[i] = y - x;
            }

            var next = GenerateNextSequence(tempResult);
            result[result.Length - 1] = input[input.Length - 1] + next[next.Length - 1];

            return result;
        }
    }
}