namespace Test
{
    using BenchmarkDotNet.Attributes;
    using BenchmarkDotNet.Diagnosers;
    using SuperLinq;
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
        public string Day6Part1()
        {
            var answer = 0;
            var times = _input[0].Split(" ", StringSplitOptions.RemoveEmptyEntries).Skip(1).Select(x => int.Parse(x));
            var dists = _input[1].Split(" ", StringSplitOptions.RemoveEmptyEntries).Skip(1).Select(x => int.Parse(x));
            var pairs = times.Zip(dists);

            var waysToWin = new List<int>();

            foreach (var pair in pairs)
            {
                waysToWin.Add(WaysToWin(pair.First, pair.Second));
            }

            answer = waysToWin.Aggregate((a, x) => a * x);

            return answer.ToString();
        }

        [Benchmark]
        public string Day6Part2()
        {
            var answer = 0L;
            var times = _input[0].Replace(" ", "").Replace("Time:", "");
            var dists = _input[1].Replace(" ", "").Replace("Distance:", "");

            answer = WaysToWin(long.Parse(times), long.Parse(dists));

            return answer.ToString();
        }

        int WaysToWin(int time, int dist)
        {
            int ans = 0;

            for (int i = 1; i < time; i++)
            {
                var trav = i * (time - i);

                if (trav > dist)
                {
                    ans++;
                }
            }

            return ans;
        }

        long WaysToWin(long time, long dist)
        {
            long ans = 0;

            for (long i = 1; i < time; i++)
            {
                var trav = i * (time - i);

                if (trav > dist)
                {
                    ans++;
                }
            }

            return ans;
        }
    }
}