namespace Test
{
    using BenchmarkDotNet.Attributes;
    using BenchmarkDotNet.Diagnosers;
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;

    record struct Seed(long Id);
    record struct Map(long DestRangeStart, long SourceRangeStart, long Length);

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
        public string Day5Part1()
        {
            var seeds = GetSeeds(_input);
            var maps = GetMaps(_input);
            var minLocation = long.MaxValue;

            foreach (var seed in seeds)
            {
                var loc = WalkMaps(seed.Id, maps);
                minLocation = Math.Min(minLocation, loc);
            }

            var answer = minLocation;
            return answer.ToString();
        }

        [Benchmark]
        public string Day5Part2()
        {
            var seeds = GetSeeds(_input);
            var maps = GetMaps(_input);
            var minLocation = long.MaxValue;

            foreach (var seedPair in seeds.Chunk(2))
            {
                var seedStart = seedPair[0].Id;
                var length = seedPair[1].Id;

                Parallel.For(seedStart, seedStart + length, x =>
                {
                    var loc = WalkMaps(x, maps);
                    var newValue = Math.Min(minLocation, loc);

                    long currentValue = minLocation;
                    while (currentValue != Interlocked.CompareExchange(ref minLocation, newValue, currentValue))
                    {
                        currentValue = minLocation;
                        newValue = Math.Min(currentValue, newValue);
                    }

                });
            }

            var answer = minLocation;
            return answer.ToString();
        }

        long WalkMaps(long seed, Dictionary<string, List<Map>> maps)
        {
            var nextLoc = DoMapping(seed, maps["seed-to-soil"]);
            nextLoc = DoMapping(nextLoc, maps["soil-to-fertilizer"]);
            nextLoc = DoMapping(nextLoc, maps["fertilizer-to-water"]);
            nextLoc = DoMapping(nextLoc, maps["water-to-light"]);
            nextLoc = DoMapping(nextLoc, maps["light-to-temperature"]);
            nextLoc = DoMapping(nextLoc, maps["temperature-to-humidity"]);
            nextLoc = DoMapping(nextLoc, maps["humidity-to-location"]);

            return nextLoc;
        }

        long DoMapping(long source, List<Map> maps)
        {
            var result = 0L;

            foreach (var map in maps)
            {
                var sourceStart = map.SourceRangeStart;
                var sourceEnd = map.SourceRangeStart + map.Length;

                if (source < sourceStart || source > sourceEnd)
                {
                    result = source;
                    continue;
                }

                var offset = source - sourceStart;

                var destStart = map.DestRangeStart;
                result = destStart + offset;
                break;
            }

            return result;
        }

        List<Seed> GetSeeds(string[] input)
        {
            var seedLine = input[0];
            var tokens = seedLine.Split(" ");
            var seeds = new List<Seed>();

            for (int i = 1; i < tokens.Length; i++)
            {
                seeds.Add(new Seed(long.Parse(tokens[i])));
            }

            return seeds;
        }

        Dictionary<string, List<Map>> GetMaps(string[] input)
        {
            var result = new Dictionary<string, List<Map>>();
            var seedToSoilMap = new List<Map>();
            var soilToFertilizerMap = new List<Map>();
            var fertilizerToWaterMap = new List<Map>();
            var waterToLightMap = new List<Map>();
            var lightToTemperatureMap = new List<Map>();
            var temperateToHumidityMap = new List<Map>();
            var humidityToLocationMap = new List<Map>();

            var current = seedToSoilMap;
            var currentKey = "seed-to-soil";

            for (int i = 2; i < input.Length; i++)
            {
                var line = input[i];

                var (key, tmp) = line switch
                {
                    "seed-to-soil map:" => ("seed-to-soil", seedToSoilMap),
                    "soil-to-fertilizer map:" => ("soil-to-fertilizer", soilToFertilizerMap),
                    "fertilizer-to-water map:" => ("fertilizer-to-water", fertilizerToWaterMap),
                    "water-to-light map:" => ("water-to-light", waterToLightMap),
                    "light-to-temperature map:" => ("light-to-temperature", lightToTemperatureMap),
                    "temperature-to-humidity map:" => ("temperature-to-humidity", temperateToHumidityMap),
                    "humidity-to-location map:" => ("humidity-to-location", humidityToLocationMap),
                    _ => (null, null)
                };

                if (string.IsNullOrWhiteSpace(line) || i + 1 == input.Length)
                {
                    result[currentKey] = current;
                }

                if (key is null && !string.IsNullOrWhiteSpace(line))
                {
                    var vals = line.Split(" ");
                    var target = long.Parse(vals[0]);
                    var source = long.Parse(vals[1]);
                    var length = long.Parse(vals[2]);
                    var map = new Map(target, source, length);
                    current.Add(map);
                }
                else
                {
                    currentKey = key;
                    current = tmp;
                }
            }

            return result;
        }
    }
}