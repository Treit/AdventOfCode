using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;

// https://adventofcode.com/2020/day/1
string inputFile = args[0];

HashSet<int> expenses = new();
using StreamReader sr = new(inputFile);

string line;

while ((line = sr.ReadLine()) != null)
{
    expenses.Add(int.Parse(line));
}

var sw = Stopwatch.StartNew();

foreach (int val in expenses)
{
    int match = 2020 - val;
    if (expenses.Contains(match))
    {
        int answer = val * match;
        Console.WriteLine($"{val} * {match} == {answer}");
        break;
    }
}

int[] expenseArray = expenses.ToArray();
bool found = false;

for (int i = 0; i < expenseArray.Length; i++)
{
    if (found)
    {
        break;
    }

    for (int j = i + 1; j < expenseArray.Length; j++)
    {
        int a = expenseArray[i];
        int b = expenseArray[j];

        int match = 2020 - (a + b);

        if (expenses.Contains(match))
        {
            int answer = a * b * match;
            Console.WriteLine($"{a} * {b} * {match} == {answer}");
            found = true;
            break;
        }
    }
}

Console.WriteLine(sw.ElapsedMilliseconds);