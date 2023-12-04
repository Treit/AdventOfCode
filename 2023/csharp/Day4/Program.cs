namespace Test
{
    using BenchmarkDotNet.Running;
    using System;
    using System.Linq;

    internal class Program
    {
        static void Main(string[] args)
        {
#if RELEASE
            BenchmarkRunner.Run<Benchmark>();
#else
            var inputFile = args.Any(arg => arg.Contains("test")) ? "testinput.txt" : "input.txt";

            var b = new Benchmark(inputFile);
            Console.WriteLine($"Day 4, part 1 answer: {b.Day4Part1()}");
            Console.WriteLine($"Day 4, part 2 answer: {b.Day4Part2()}");
#endif
        }
    }
}
