using System;
using System.Diagnostics;
using System.IO;

// https://adventofcode.com/2020/day/2
string inputFile = args[0];

using StreamReader sr = new(inputFile);

string line;
int goodPartOne = 0;
int goodPartTwo = 0;

var sw = Stopwatch.StartNew();

while ((line = sr.ReadLine()) != null)
{
    ReadOnlySpan<char> span = line.AsSpan();
    int loc = span.IndexOf(' ');
    ReadOnlySpan<char> password = span.Slice(loc + 4);
    ReadOnlySpan<char> countRange = span.Slice(0, loc);
    char requiredChar = span.Slice(loc + 1, 1)[0];
    loc = countRange.IndexOf('-');
    ReadOnlySpan<char> valAStr = countRange.Slice(0, loc);
    ReadOnlySpan<char> valBStr = countRange.Slice(loc + 1);
    int valA = int.Parse(valAStr);
    int valB = int.Parse(valBStr);

    if (MatchesFirstPolicy(valA, valB, requiredChar, password))
    {
        goodPartOne++;
    }

    if (MatchesSecondPolicy(valA, valB, requiredChar, password))
    {
        goodPartTwo++;
    }
}

Console.WriteLine($"Determined that {goodPartOne} passwords comply with the first policy in {sw.ElapsedMilliseconds} ms.");
Console.WriteLine($"Determined that {goodPartTwo} passwords comply with the second policy in {sw.ElapsedMilliseconds} ms.");

static bool MatchesFirstPolicy(int min, int max, char requiredChar, ReadOnlySpan<char> pwd)
{
    int count = CountChars(pwd, requiredChar);
    return count >= min && count <= max;
}

static bool MatchesSecondPolicy(int posA, int posB, char requiredChar, ReadOnlySpan<char> pwd)
{
    bool posAMatched = false;
    bool posBMatched = false;
    int currPos = 0;

    for (int i = 0; i < pwd.Length; i++)
    {
        currPos = i + 1; // One-based indexing.
        bool charMatches = pwd[i] == requiredChar;

        if (!posAMatched)
        {
            posAMatched = currPos == posA && charMatches;
        }

        if (!posBMatched)
        {
            posBMatched = currPos == posB && charMatches;
        }
    }

    bool bothMatched = posAMatched && posBMatched;
    bool result = (posAMatched || posBMatched) && !bothMatched;

    return result;
}

static int CountChars(ReadOnlySpan<char> str, char target)
{
    int count = 0;

    foreach (char c in str)
    {
        if (c == target)
        {
            count++;
        }
    }

    return count;
}
