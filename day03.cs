Console.WriteLine("Part 1: {0}", Part1());
Console.WriteLine("Part 2: {0}", Part2());

long Part1()
{
    long result = 0;
    foreach (var line in ReadLines())
    {
        FindLargestNumber(line, 0, line.Length - 2, out var a, out var aPos);
        FindLargestNumber(line, aPos + 1, line.Length - 1, out var b, out _);

        result += a * 10 + b;
    }
    return result;
}

long Part2()
{
    const int totalLength = 12;
    long result = 0;

    foreach (var line in ReadLines())
    {
        var lineResult = new System.Text.StringBuilder(totalLength);
        int number;
        int pos = 0;
        for (int i = 0; i < 12; i++)
        {
            FindLargestNumber(line, pos, line.Length - totalLength + lineResult.Length, out number, out pos);
            lineResult.Append(number);

            pos += 1;
        }

        result += long.Parse(lineResult.ToString());
    }
    return result;
}

void FindLargestNumber(string line, int start, int end, out int number, out int position)
{
    number = -1;
    position = -1;
    for (int i = start; i <= end; i++)
    {
        var n = line[i] - 48;
        if (n > number)
        {
            number = n;
            position = i;
        }
    }
}

static IEnumerable<string> ReadLines()
{
    return File.ReadLines("input/03.txt");
}
