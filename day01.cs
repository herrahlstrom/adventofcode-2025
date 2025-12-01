Console.WriteLine("Part 1: {0}", await Part1());
Console.WriteLine("Part 2: {0}", await Part2());

async Task<int> Part1()
{
    int dial = 50;
    int result = 0;
    await foreach (var value in ReadValues())
    {
        dial = (dial + value) % 100;
        if (dial == 0)
            result++;
    }
    return result;
}

async Task<int> Part2()
{
    int dial = 50;
    int result = 0;
    await foreach (var value in ReadValues())
    {
        bool wasZero = dial.Equals(0);
        dial += value;

        if (value > 0)
        {
            while (dial > 99)
            {
                dial -= 100;
                result++;
            }
        }
        else if (value < 0)
        {
            if (dial < 0)
            {
                // We start at 0 and it has been counted once...
                if (wasZero)
                    result--;

                while (dial < 0)
                {
                    dial += 100;
                    result++;
                }
            }

            if (dial == 0)
                result++;
        }
    }
    return result;
}

async IAsyncEnumerable<int> ReadValues()
{
    await foreach (var line in File.ReadLinesAsync("input/day01.txt"))
    {
        var span = line.AsSpan();
        yield return span[0] switch
        {
            'L' => -int.Parse(span[1..]),
            'R' => +int.Parse(span[1..]),
            _ => 0
        };
    }
}