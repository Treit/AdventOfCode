# Day 6 benchmark.

``` ini

BenchmarkDotNet=v0.13.3, OS=Windows 11 (10.0.26010.1000)
Intel Xeon W-2123 CPU 3.60GHz, 1 CPU, 8 logical and 4 physical cores
.NET SDK=8.0.100-rc.2.23502.2
  [Host]     : .NET 8.0.0 (8.0.23.47906), X64 RyuJIT AVX2
  DefaultJob : .NET 8.0.0 (8.0.23.47906), X64 RyuJIT AVX2


```
|    Method |          Mean |         Error |        StdDev |   Gen0 | Allocated |
|---------- |--------------:|--------------:|--------------:|-------:|----------:|
| Day6Part1 |      1.307 μs |     0.0179 μs |     0.0150 μs | 0.2861 |    1240 B |
| Day6Part2 | 53,897.431 μs | 1,052.8978 μs | 1,126.5889 μs |      - |         - |
