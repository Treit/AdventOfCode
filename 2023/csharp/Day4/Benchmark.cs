using System.Numerics;
using System.Runtime.CompilerServices;
using System;

namespace Test
{
    using BenchmarkDotNet.Attributes;
    using BenchmarkDotNet.Diagnosers;
    using System.IO;
    using System;
    using System.Linq;
    using System.Collections.Generic;

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
        public string Day4Part1()
        {
            var answer = 0;

            foreach (var line in _input)
            {
                var cardpoints = 0;
                var loc = line.IndexOf(":");
                var slice = line.AsSpan().Slice(loc + 2).Trim();
                var normalized = slice.ToString().Replace(" | ", "|");
                var parts = normalized.Split("|");
                var winners = parts[0].Split(' ').ToHashSet();
                var ours = parts[1].Split(' ', StringSplitOptions.RemoveEmptyEntries);
                foreach (var num in ours)
                {
                    if (winners.Contains(num))
                    {
                        cardpoints = cardpoints == 0 ? 1 : cardpoints * 2;
                    }
                }

                answer += cardpoints;
            }

            return answer.ToString();
        }

        [Benchmark]
        public string Day4Part2()
        {
            var answer = 0;
            var cardindex = -1;
            var cardList = new List<ScratchCard>();
            var cardQueue = new Queue<ScratchCard>();

            foreach (var line in _input)
            {
                cardindex++;
                var loc = line.IndexOf(":");
                var slice = line.AsSpan().Slice(loc + 2).Trim();
                var normalized = slice.ToString().Replace(" | ", "|");
                var parts = normalized.Split("|");
                var winners = parts[0].Split(' ').ToHashSet();
                var ours = parts[1].Split(' ', StringSplitOptions.RemoveEmptyEntries);
                var wins = 0;
                for (int i = 0; i < ours.Length; i++)
                {
                    if (winners.Contains(ours[i]))
                    {
                        wins++;
                    }
                }

                var card = new ScratchCard(cardindex, wins);
                cardList.Add(card);
                cardQueue.Enqueue(card);
            }

            while (cardQueue.Count > 0)
            {
                answer++;
                var currentCard = cardQueue.Dequeue();
                var winCount = currentCard.WinCount;
                for (int i = currentCard.CardIndex + 1; i < currentCard.CardIndex + winCount + 1; i++)
                {
                    cardQueue.Enqueue(cardList[i]);
                }
            }

            return answer.ToString();
        }

        record ScratchCard(int CardIndex, int WinCount);
   }
}