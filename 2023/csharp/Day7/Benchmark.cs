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
        public string Day7Part1()
        {
            var hands = new List<(Hand, int)>();

            foreach (var line in _input)
            {
                var tokens = line.Split(" ");
                var hand = new Hand(tokens[0]);
                var bid = int.Parse(tokens[1]);
                hands.Add((hand, bid));
            }

            var rank = 0;
            var totalWinnings = 0;

            foreach (var hand in hands.OrderBy(x => x.Item1))
            {
                rank++;
                Console.WriteLine($"{hand.Item1.Cards} -> {hand.Item1.Strength}");
                totalWinnings += rank * hand.Item2;
            }

            var answer = totalWinnings;
            return answer.ToString();
        }

        [Benchmark]
        public string Day7Part2()
        {
            var hands = new List<(WildcardHand, long)>();

            foreach (var line in _input)
            {
                var tokens = line.Split(" ");
                var hand = new WildcardHand(tokens[0]);
                var bid = long.Parse(tokens[1]);

                hands.Add((hand, bid));
            }

            var rank = 0L;
            var totalWinnings = 0L;

            foreach (var hand in hands.OrderBy(x => x.Item1))
            {
                rank++;
                Console.WriteLine($"({rank}) {hand.Item1.Cards} ({hand.Item1.EffectiveCards}) -> {hand.Item1.Strength} -> {totalWinnings} += {rank} * {hand.Item2} == {totalWinnings + rank * hand.Item2}");
                totalWinnings += rank * hand.Item2;
            }

            var answer = totalWinnings;
            return answer.ToString();
        }
   }
}