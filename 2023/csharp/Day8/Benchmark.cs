using System.Numerics;
using System.Runtime.CompilerServices;
using System;

namespace Test
{
    using BenchmarkDotNet.Attributes;
    using BenchmarkDotNet.Diagnosers;
    using System.Text.RegularExpressions;
    using System.IO;
    using System;
    using System.Collections.Generic;
    using Microsoft.Diagnostics.Tracing.Parsers.MicrosoftAntimalwareEngine;
    using System.Linq;
    using System.Data;
    using Microsoft.Diagnostics.Runtime;
    using SuperLinq;

    class Node(string label)
    {
        public string Label { get; private set; } = label;
        public Node Left { get; set; }
        public Node Right { get; set; }
    }

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
        public string Day8Part1()
        {
            var start = "AAA";
            var target = "ZZZ";

            var directions = _input[0];
            var nodeMap = new Dictionary<string, Node>();

            for (int i = 2; i < _input.Length; i++)
            {
                var nodeLabel = _input[i].Substring(0, 3);
                nodeMap.Add(nodeLabel, new Node(nodeLabel));
            }

            var re = new Regex(@"\((\w{3}), (\w{3})");
            for (int i = 2; i < _input.Length; i++)
            {
                var nodeLabel = _input[i].Substring(0, 3);
                var m = re.Match(_input[i]);
                if (!m.Success)
                {
                    throw new InvalidOperationException(@"Unexpected");
                }

                nodeMap[nodeLabel].Left = nodeMap[m.Result("$1")];
                nodeMap[nodeLabel].Right = nodeMap[m.Result("$2")];
            }

            var currentNode = nodeMap[start];
            var currentDir = -1;
            var steps = 0;

            while (true)
            {
                steps++;
                currentDir++;
                if (currentDir == directions.Length)
                {
                    currentDir = 0;
                }

                var dir = directions[currentDir];

                currentNode = dir == 'L' ? currentNode.Left : currentNode = currentNode.Right;

                if (currentNode.Label == target)
                {
                    break;
                }
            }

            var answer = steps;
            return answer.ToString();
        }

        [Benchmark]
        public string Day8Part2()
        {
            var start = "A";
            var target = "Z";
            var answer = 0L;

            var directions = _input[0];
            var nodeMap = new Dictionary<string, Node>();
            var startNodes = new List<Node>();
            var endNodes = new List<Node>();

            for (int i = 2; i < _input.Length; i++)
            {
                var nodeLabel = _input[i].Substring(0, 3);
                var node = new Node(nodeLabel);
                if (nodeLabel.EndsWith(start))
                {
                    startNodes.Add(node);
                }
                else if (nodeLabel.EndsWith(target))
                {
                    endNodes.Add(node);
                }

                nodeMap.Add(nodeLabel, node);
            }

            var re = new Regex(@"\((\w{3}), (\w{3})");
            for (int i = 2; i < _input.Length; i++)
            {
                var nodeLabel = _input[i].Substring(0, 3);
                var m = re.Match(_input[i]);
                if (!m.Success)
                {
                    throw new InvalidOperationException(@"Unexpected");
                }

                nodeMap[nodeLabel].Left = nodeMap[m.Result("$1")];
                nodeMap[nodeLabel].Right = nodeMap[m.Result("$2")];
            }

            var answers = new List<(Node, Node, long)>();

            for (int i = 0; i < startNodes.Count; i++)
            {
                var startNode = startNodes[i];
                var currentNode = startNode;
                var currentDir = -1;
                var steps = 0L;

                while (true)
                {
                    steps++;
                    currentDir++;
                    if (currentDir == directions.Length)
                    {
                        currentDir = 0;
                    }

                    var dir = directions[currentDir];

                    currentNode = dir == 'L' ? currentNode.Left : currentNode = currentNode.Right;

                    if (currentNode.Label.EndsWith(target))
                    {
                        break;
                    }
                }
                answers.Add((startNode, currentNode, steps));
            }

            foreach (var tuple in answers)
            {
                Console.WriteLine($"Start: {tuple.Item1.Label}, End: {tuple.Item2.Label}, Steps: {tuple.Item3}");
            }

            var allAnswers = answers.Select(x => x.Item3).ToList();
            var lcm = LCM(allAnswers[0], allAnswers[1]);
            foreach (var num in allAnswers.Skip(2))
            {
                lcm = LCM(num, lcm);
            }

            answer = lcm;
            return answer.ToString();
        }

        static long GCF(long a, long b)
        {
            while (b != 0)
            {
                long temp = b;
                b = a % b;
                a = temp;
            }

            return a;
        }

        static long LCM(long a, long b)
        {
            return (a / GCF(a, b)) * b;
        }
    }
}