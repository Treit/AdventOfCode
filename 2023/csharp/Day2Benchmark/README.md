# Day 2 benchmark.


``` ini

BenchmarkDotNet=v0.13.3, OS=Windows 11 (10.0.26002.1010)
Intel Xeon W-2123 CPU 3.60GHz, 1 CPU, 8 logical and 4 physical cores
.NET SDK=8.0.100-rc.2.23502.2
  [Host]     : .NET 7.0.14 (7.0.1423.51910), X64 RyuJIT AVX2
  DefaultJob : .NET 7.0.14 (7.0.1423.51910), X64 RyuJIT AVX2


```
|    Method |     Mean |    Error |   StdDev |     Gen0 |  Allocated |
|---------- |---------:|---------:|---------:|---------:|-----------:|
| Day2Part1 | 959.8 μs | 17.98 μs | 16.82 μs | 237.3047 | 1003.48 KB |
| Day2Part2 | 965.2 μs | 18.58 μs | 17.38 μs | 237.3047 | 1003.48 KB |


