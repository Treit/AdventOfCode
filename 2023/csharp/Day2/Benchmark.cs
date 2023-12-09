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
        private string[] _input;
        private byte[] _bytes;

        [GlobalSetup]
        public void GlobalSetup()
        {
            _input = File.ReadAllLines("input.txt");
            _bytes = File.ReadAllBytes("input.txt");
        }

        [Benchmark]
        public int Day2Part1()
        {
            var maxRed = 12;
            var maxGreen = 13;
            var maxBlue = 14;
            var answer = 0;

            foreach (var line in _input)
            {
                var normalized = line.Replace(" ", "");
                var tokens = normalized.Split(":");
                var gameId = int.Parse(tokens[0].Replace("Game", ""));
                var results = tokens[1].Replace(";", ",");
                tokens = results.Split(",");
                var valid = true;

                foreach (var token in tokens)
                {
                    var match = Regex.Match(token, @"(\d+)(\w+)");
                    var num = int.Parse(match.Result("$1"));
                    var color = match.Result("$2");
                    if (color == "green" && num > maxGreen) valid = false;
                    if (color == "red" && num > maxRed) valid = false;
                    if (color == "blue" && num > maxBlue) valid = false;
                }

                if (valid)
                {
                    answer += gameId;
                }
            }

            return answer;
        }

        [Benchmark]
        public int Day2Part2()
        {
            var answer = 0;

            foreach (var line in _input)
            {
                var normalized = line.Replace(" ", "");
                var tokens = normalized.Split(":");
                var gameId = int.Parse(tokens[0].Replace("Game", ""));
                var results = tokens[1].Replace(";", ",");
                tokens = results.Split(",");
                var maxGreenSeen = 0;
                var maxRedSeen = 0;
                var maxBlueSeen = 0;

                foreach (var token in tokens)
                {
                    var match = Regex.Match(token, @"(\d+)(\w+)");
                    var num = int.Parse(match.Result("$1"));
                    var color = match.Result("$2");
                    if (color == "green" && num > maxGreenSeen) maxGreenSeen = num;
                    if (color == "red" && num > maxRedSeen) maxRedSeen = num;
                    if (color == "blue" && num > maxBlueSeen) maxBlueSeen = num;
                }

                var power = maxGreenSeen * maxRedSeen * maxBlueSeen;
                answer += power;
            }

            return answer;
        }

        [Benchmark(Baseline = true)]
        public int Day2Part1Optimized()
        {
            var answer = 0;
            var gameId = 1;

            foreach (var line in _input)
            {
                var maxGreenSeen = 0;
                var maxRedSeen = 0;
                var maxBlueSeen = 0;

                var span = line.AsSpan();
                span = span.Slice(span.IndexOf(':') + 2);
                var curr = 0;

                foreach (var c in span)
                {
                    if (char.IsAsciiDigit(c))
                    {
                        if (curr != 0)
                        {
                            curr *= 10;
                            curr += c - '0';
                        }
                        else
                        {
                            curr = c - '0';
                        }
                    }
                    else if (c == 'b')
                    {
                        if (curr > maxBlueSeen)
                        {
                            maxBlueSeen = curr;
                        }

                        curr = 0;
                    }
                    else if (c == 'r')
                    {
                        if (curr > maxRedSeen)
                        {
                            maxRedSeen = curr;
                        }

                        curr = 0;
                    }
                    else if (c == 'g')
                    {
                        if (curr > maxGreenSeen)
                        {
                            maxGreenSeen = curr;
                        }

                        curr = 0;
                    }
                }

                var valid = (maxRedSeen, maxGreenSeen, maxBlueSeen) switch
                {
                    (> 12, _, _) => false,
                    (_, > 13, _) => false,
                    (_, _, > 14) => false,
                    _ => true
                };

                if (valid)
                {
                    answer += gameId;
                }

                gameId++;
            }

            return answer;
        }

        [Benchmark]
        public int Day2Part2Optimized()
        {
            var answer = 0;

            foreach (var line in _input)
            {
                var maxGreenSeen = 0;
                var maxRedSeen = 0;
                var maxBlueSeen = 0;

                var span = line.AsSpan();
                span = span.Slice(span.IndexOf(':') + 2);
                var curr = 0;

                foreach (var c in span)
                {
                    if (char.IsAsciiDigit(c))
                    {
                        if (curr != 0)
                        {
                            curr *= 10;
                            curr += c - '0';
                        }
                        else
                        {
                            curr = c - '0';
                        }
                    }
                    else if (c == 'b')
                    {
                        if (curr > maxBlueSeen)
                        {
                            maxBlueSeen = curr;
                        }

                        curr = 0;
                    }
                    else if (c == 'r')
                    {
                        if (curr > maxRedSeen)
                        {
                            maxRedSeen = curr;
                        }

                        curr = 0;
                    }
                    else if (c == 'g')
                    {
                        if (curr > maxGreenSeen)
                        {
                            maxGreenSeen = curr;
                        }

                        curr = 0;
                    }
                }

                var power = maxGreenSeen * maxRedSeen * maxBlueSeen;
                answer += power;
            }

            return answer;
        }

        [Benchmark]
        public (string, string) Day2ViceroyPenguin()
        {
            var part1 = 0;
            var part2 = 0;
            var span = new ReadOnlySpan<byte>(_bytes);

            while (span.Length > 0)
            {
                var (id, n) = span.Slice(5).AtoI();
                span = span.Slice(n + 7);

                var maxRed = 0;
                var maxGreen = 0;
                var maxBlue = 0;

                while (true)
                {
                    (var num, n) = span.AtoI();
                    span = span.Slice(n + 1);

                    switch (span[0])
                    {
                        case (byte)'r':
                            {
                                maxRed = Math.Max(maxRed, num);
                                span = span.Slice(3);
                                break;
                            }
                        case (byte)'b':
                            {
                                maxBlue = Math.Max(maxBlue, num);
                                span = span.Slice(4);
                                break;
                            }
                        case (byte)'g':
                            {
                                maxGreen = Math.Max(maxGreen, num);
                                span = span.Slice(5);
                                break;
                            }
                    }

                    if (span[0] == (byte)'\n')
                        break;

                    span = span.Slice(2);
                }

                span = span.Slice(1);

                if (maxRed <= 12
                    && maxGreen <= 13
                    && maxBlue <= 14)
                {
                    part1 += id;
                }

                part2 += maxRed * maxGreen * maxBlue;
            }

            return (part1.ToString(), part2.ToString());
        }
    }
}