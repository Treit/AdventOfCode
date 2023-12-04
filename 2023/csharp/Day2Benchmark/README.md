# Day 2 benchmark.



``` ini

BenchmarkDotNet=v0.13.3, OS=Windows 11 (10.0.26002.1010)
Intel Xeon W-2123 CPU 3.60GHz, 1 CPU, 8 logical and 4 physical cores
.NET SDK=8.0.100-rc.2.23502.2
  [Host]     : .NET 8.0.0 (8.0.23.47906), X64 RyuJIT AVX2
  DefaultJob : .NET 8.0.0 (8.0.23.47906), X64 RyuJIT AVX2


```
|             Method |        Mean |     Error |    StdDev |     Gen0 | Allocated |
|------------------- |------------:|----------:|----------:|---------:|----------:|
|          Day2Part1 | 1,118.78 μs | 17.126 μs | 15.181 μs | 228.5156 |  989705 B |
|          Day2Part2 | 1,106.69 μs | 13.336 μs | 11.822 μs | 228.5156 |  989705 B |
| Day2Part1Optimized |    21.37 μs |  0.395 μs |  0.566 μs |        - |         - |
| Day2Part2Optimized |    19.37 μs |  0.232 μs |  0.205 μs |        - |         - |