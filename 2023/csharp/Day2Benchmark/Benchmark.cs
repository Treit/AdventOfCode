namespace Test
{
    using BenchmarkDotNet.Attributes;
    using BenchmarkDotNet.Diagnosers;
    using System.Text.RegularExpressions;
    using System.IO;

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

    }
}
