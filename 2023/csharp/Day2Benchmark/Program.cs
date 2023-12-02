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
            Console.WriteLine(b.Day2Part2());
#endif
        }
    }
}
