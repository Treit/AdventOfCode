namespace Test
{
    using BenchmarkDotNet.Attributes;
    using BenchmarkDotNet.Diagnosers;
    using System;
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
            var nums = line.Split(" ").Select(x => long.Parse(x));
            var sequence = reverse
                ? GenerateNextSequence(nums.Reverse().ToArray())
                : GenerateNextSequence(nums.ToArray());

            var answer = sequence[sequence.Length - 1];
            return answer;
        }

        long[] GenerateNextSequence(long[] input)
        {
            if (input.All(x => x == 0))
            {
                return input;
            }

            var tempResult = new long[input.Length - 1];

            Array.Resize(ref input, input.Length + 1);

            for (var i = 0; i < input.Length - 2; i++)
            {
                var x = input[i];
                var y = input[i + 1];
                tempResult[i] = y - x;
            }

            var next = GenerateNextSequence(tempResult);
            input[^1] = input[^2] + next[^1];

            return input;
        }
    }
}