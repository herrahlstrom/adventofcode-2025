Console.WriteLine("Part 1: {0}", Part1());
Console.WriteLine("Part 2: {0}", Part2());

long Part1()
{
    long result = 0;
    foreach (var range in ReadRanges())
    {
        for (long id = range.From; id <= range.To; id++)
        {
            if (IsSillyPattern(id.ToString()))
                result += id;
        }
    }
    return result;

    static bool IsSillyPattern(string id)
    {
        if (id.Length % 2 != 0)
            return false;

        var p = id.Length / 2;
        for (int i = 0; i < p; i++)
        {
            if (id[i] != id[i + p])
                return false;
        }
        return true;
    }
}

long Part2()
{
    long result = 0;
    foreach (var range in ReadRanges())
    {
        for (long id = range.From; id <= range.To; id++)
        {
            if (IsSillyPattern(id.ToString()))
            {
                result += id;
            }
        }
    }
    return result;

    static bool IsSillyPattern(string id)
    {
        for (int chunks = 2; chunks <= id.Length; chunks++)
        {
            if (id.Length % chunks != 0)
                continue;

            bool canBeSilly = true;

            var p = id.Length / chunks;
            for (int i = 0; i < p; i++)
            {
                for (int j = 1; j < chunks; j++)
                {
                    if (id[i] != id[i + p * j])
                    {
                        canBeSilly = false;
                        break;
                    }
                }
                if (!canBeSilly)
                    break;
            }

            if (canBeSilly)
                return true;
        }
        return false;
    }
}


static IEnumerable<Range> ReadRanges()
{
    var ranges = File.ReadAllText("input/day02.txt").Split(',');
    foreach (var range in ranges)
    {
        var arr = range.Split('-');
        yield return new Range(long.Parse(arr[0]), long.Parse(arr[1]));
    }
}

record struct Range(long From, long To);
