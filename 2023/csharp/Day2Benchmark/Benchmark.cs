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

        [GlobalSetup]
        public void GlobalSetup()
        {
            _input = File.ReadAllLines("input.txt");
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

        [Benchmark]
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
    }
}
