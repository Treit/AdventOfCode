using System.Security.Cryptography;
using System.Text;

namespace AdventOfCode.Puzzles._2015;

[Puzzle(2015, 04, CodeType.Original)]
public class Day_04_Original : IPuzzle
{
    public (string, string) Solve(PuzzleInput input)
    {
        var part1 = Part1(input.Text);
        var part2 = Part2(input.Text);
        return (part1, part2);
    }

    public static string Part1(string input)
    {
        var key = input.TrimEnd();

        var answer = 0UL;

        for (var i = 0UL; i < uint.MaxValue; i++)
        {
            var tmp = key + i.ToString();
            var md5 = MD5.HashData(ASCIIEncoding.ASCII.GetBytes(tmp));
            var hexStr = Convert.ToHexString(md5);

            if (hexStr.StartsWith("00000"))
            {
                answer = i;
                break;
            }
        }

        return answer.ToString();
    }

    public static string Part2(string input)
    {
        var key = input.TrimEnd();

        var answer = 0UL;

        for (var i = 0UL; i < uint.MaxValue; i++)
        {
            var tmp = key + i.ToString();
            var md5 = MD5.HashData(ASCIIEncoding.ASCII.GetBytes(tmp));
            var hexStr = Convert.ToHexString(md5);

            if (hexStr.StartsWith("000000"))
            {
                answer = i;
                break;
            }
        }

        return answer.ToString();
    }
}
