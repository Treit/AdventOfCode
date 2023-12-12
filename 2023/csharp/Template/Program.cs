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

            var runDay1 = args switch
            {
                [] => true,
                ["test"] => true,
                ["1", ..] => true,
                [_, "1", ..] => true,
                _ => false
            };

            var runDay2 = args switch
            {
                [] => true,
                ["test"] => true,
                ["2", ..] => true,
                [_, "2", ..] => true,
                _ => false
            };

            if (runDay1)
            {
                Console.WriteLine($"Day ___, part 1 answer: {b.Day___Part1()}");
            }

            if (runDay2)
            {
                Console.WriteLine($"Day ___, part 2 answer: {b.Day___Part2()}");
            }
#endif
        }
    }
}
