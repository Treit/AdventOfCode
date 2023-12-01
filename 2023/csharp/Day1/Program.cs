var inputfile = args.Any(arg => arg.Contains("test")) ? "testinput.txt" : "input.txt";
var input = File.ReadAllLines(inputfile);

if (args.Any(arg => arg.Contains("2")))
{
    Part2(input);
    return;
}

Part1(input);

void Part1(string[] input)
{
    // Part 1
    var sum = 0;
    foreach (var line in input)
    {
        var first = "";
        var last = "";

        foreach (char c in line)
        {
            if (!char.IsDigit(c))
            {
                continue;
            }

            if (first is null)
            {
                first = c.ToString();
            }

            last = c.ToString();
        }

        sum += int.Parse(first + last);
    }

    Console.WriteLine(sum);
}

void Part2(string[] input)
{
    var sum = 0;

    foreach (var tmp in input)
    {
        var first = "";
        var last = "";

        for (int i = 0; i < tmp.Length; i++)
        {
            var digit = DigitAtCurrent(tmp, i);

            if (digit is null)
            {
                continue;
            }

            if (first == "")
            {
                first = digit;
            }

            last = digit;
        }

        sum += int.Parse(first + last);

    }

    Console.WriteLine(sum);
}

string? DigitAtCurrent(string input, int pos)
{
    if (char.IsDigit(input[pos]))
    {
        return input[pos].ToString();
    }

    if (input.Substring(pos, Math.Min("one".Length, input.Length - pos)) == "one") return "1";
    if (input.Substring(pos, Math.Min("two".Length, input.Length - pos)) == "two") return "2";
    if (input.Substring(pos, Math.Min("three".Length, input.Length - pos)) == "three") return "3";
    if (input.Substring(pos, Math.Min("four".Length, input.Length - pos)) == "four") return "4";
    if (input.Substring(pos, Math.Min("five".Length, input.Length - pos)) == "five") return "5";
    if (input.Substring(pos, Math.Min("six".Length, input.Length - pos)) == "six") return "6";
    if (input.Substring(pos, Math.Min("seven".Length, input.Length - pos)) == "seven") return "7";
    if (input.Substring(pos, Math.Min("eight".Length, input.Length - pos)) == "eight") return "8";
    if (input.Substring(pos, Math.Min("nine".Length, input.Length - pos)) == "nine") return "9";

    return null;
}

