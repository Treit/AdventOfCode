using System.Text;

namespace AdventOfCode.Puzzles._2024;

[Puzzle(2024, 09, CodeType.Original)]
public class Day_09_Original : IPuzzle
{
    public (string, string) Solve(PuzzleInput input)
    {
        var part1 = Part1(input.Text);
        var part2 = Part2(input.Text);
        return (part1, part2);
    }

    public static string Part1(string input)
    {
        //input = "2333133121414131402";
        var (indexMap, file) = Expand(input);
        var compacted = Compact(indexMap, file);
        return CheckSum(compacted).ToString();
    }

    public static string Part2(string input)
    {
        //input = "2333133121414131402";
        var (indexMap, file) = Expand(input);
        var compacted = CompactWholeFiles(indexMap, file);
        return CheckSum(compacted).ToString();
    }

    private static ulong CheckSum(int[] fileData)
    {
        var result = 0UL;
        var fileId = 0UL;

        for (var i = 0; i < fileData.Length; i++)
        {
            if (fileData[i] != -1)
            {
                result += (ulong)fileData[i] * fileId;
            }

            fileId++;
        }

        return result;
    }

    private static int[] Compact(Dictionary<int, int> indexMap, string fileData)
    {
        var compressedData = new int[fileData.Length];
        var data = fileData.ToCharArray();

        for (var i = 0; i < data.Length; i++)
        {
            compressedData[i] = data[i] == 'X' ? indexMap[i] : -1;
        }

        for (var i = compressedData.Length - 1; i >= 0; i--)
        {
            for (var j = 0; j < compressedData.Length; j++)
            {
                if (i <= j)
                {
                    goto Done;
                }

                if (compressedData[j] == -1)
                {
                    compressedData[j] = compressedData[i];
                    compressedData[i] = -1;
                    break;
                }
            }
        }

Done:
        return compressedData;
    }

    private static int[] CompactWholeFiles(Dictionary<int, int> indexMap, string fileData)
    {
        var compressedData = new int[fileData.Length];
        var data = fileData.ToCharArray();

        for (var i = 0; i < data.Length; i++)
        {
            compressedData[i] = data[i] == 'X' ? indexMap[i] : -1;
        }

        var endPtr = compressedData.Length - 1;
        List<int> nextFile = [];

        while (endPtr > 0)
        {
            (endPtr, nextFile) = GetNextFile(compressedData, endPtr);
            AddToFreeSpaceIfPossible(nextFile, compressedData, endPtr + 1);
        }

        return compressedData;
    }

    private static void AddToFreeSpaceIfPossible(List<int> file, int[] data, int endPtr)
    {
        var requiredLength = file.Count;
        var startOfRun = -1;
        var runCount = 0;
        var foundBlock = false;

        for (var i = 0; i < data.Length; i++)
        {
            if (data[i] == -1)
            {
                if (startOfRun == -1)
                {
                    startOfRun = i;
                }

                runCount++;
            }
            else
            {
                if (startOfRun != -1 && runCount >= requiredLength)
                {
                    foundBlock = true;
                    break;
                }

                startOfRun = -1;
                runCount = 0;
            }
        }

        // Only move files to free space to the left
        if (foundBlock && startOfRun < endPtr)
        {
            for (var i = 0; i < requiredLength; i++)
            {
                data[startOfRun + i] = file[i];
                data[endPtr++] = -1;
            }
        }
    }

    private static (int, List<int>) GetNextFile(int[] data, int endPtr)
    {
        var current = data[endPtr];
        var result = new List<int>();

        // Skip free space
        while (endPtr >= 0 && current == -1)
        {
            endPtr--;
            current = data[endPtr];
        }

        while (endPtr >= 0 && data[endPtr] == current)
        {
            result.Add(current);
            endPtr--;
        }

        return (endPtr, result);
    }

    private static (Dictionary<int, int>, string) Expand(string input)
    {
        var fileId = -1;
        var sb = new StringBuilder();
        var inFile = true;
        var indexMap = new Dictionary<int, int>();
        var currIdx = -1;

        foreach (var d in input.Select(char.GetNumericValue))
        {
            if (inFile)
            {
                fileId++;

                // Lay down the blocks for the current file
                for (var i = 0; i < d; i++)
                {
                    currIdx++;
                    indexMap[currIdx] = fileId;
                    sb.Append('X');
                }
            }
            else
            {
                // Lay down the free space
                for (var i = 0; i < d; i++)
                {
                    currIdx++;
                    sb.Append('.');
                }
            }

            // Toggle
            inFile = !inFile;
        }

        return (indexMap, sb.ToString());
    }
}
