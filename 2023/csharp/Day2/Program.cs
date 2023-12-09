namespace Test
{
    using BenchmarkDotNet.Running;
    using System;

    internal class Program
    {
        static void Main(string[] args)
        {
#if RELEASE
            BenchmarkRunner.Run<Benchmark>();
#else
            var b = new Benchmark();
            b.GlobalSetup();
            Console.WriteLine($"Day 1, part 1 answer: {b.Day2Part1()}");
            Console.WriteLine($"Day 2, part 1 optimized answer: {b.Day2Part1Optimized()}");
            Console.WriteLine($"Day 2, part 2 answer: {b.Day2Part2()}");
            Console.WriteLine($"Day 2, part 2 optimized answer: {b.Day2Part2Optimized()}");
            Console.WriteLine($"Day 2, part 2 viceroypenguin: {b.Day2ViceroyPenguin()}");
#endif
        }
    }
}
