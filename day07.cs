Console.WriteLine("Part 1: {0}", Part1());
Console.WriteLine("Part 2: {0}", Part2());

long Part1()
{
    var map = ReadMap();
    return FindSplitters(map).Count();
}

long Part2()
{
    var map = ReadMap();

    List<Pos> splitters = FindSplitters(map).ToList();
    Dictionary<Pos, long> cache = new Dictionary<Pos, long>(splitters.Count);
    return CalcBeam(FindStart(map));

    long GetSplitterPaths(Pos pos)
    {
        if (cache.TryGetValue(pos, out long cachedValue))
            return cachedValue;

        var a = new Pos(pos.X - 1, pos.Y + 1);
        var b = new Pos(pos.X + 1, pos.Y + 1);

        var slitterResult = CalcBeam(a) + CalcBeam(b);
        cache[pos] = slitterResult;
        return slitterResult;
    }

    long CalcBeam(Pos pos)
    {
        if (pos.Y >= map.GetLength(1) - 1)
            return 1;

        if (cache.TryGetValue(pos, out long cachedValue))
            return cachedValue;

        var next = new Pos(pos.X, pos.Y + 1);

        var posResult = map[next.X, next.Y] == '^'
            ? GetSplitterPaths(next)
            : CalcBeam(next);

        cache[pos] = posResult;
        return posResult;
    }
}

static char[,] ReadMap()
{
    var lines = File.ReadLines("input/07.txt").ToList();
    var result = new char[lines[0].Length, lines.Count];
    for (int x = 0; x < result.GetLength(0); x++)
    {
        for (int y = 0; y < result.GetLength(1); y++)
        {
            result[x, y] = lines[y][x];
        }
    }
    return result;
}

IEnumerable<Pos> FindSplitters(char[,] map)
{
    HashSet<Pos> beamTrail = [];
    Queue<Pos> queue = [];

    queue.Enqueue(FindStart(map));

    while (queue.TryDequeue(out Pos next))
    {
        if (next.Y >= map.GetLength(1) - 1)
            continue;

        if (map[next.X, next.Y + 1] == '^')
        {
            yield return new Pos(next.X, next.Y + 1);
            var a = new Pos(next.X - 1, next.Y + 1);
            var b = new Pos(next.X + 1, next.Y + 1);
            if (beamTrail.Add(a))
                queue.Enqueue(a);
            if (beamTrail.Add(b))
                queue.Enqueue(b);
        }
        else
        {
            var a = new Pos(next.X, next.Y + 1);
            if (beamTrail.Add(a))
                queue.Enqueue(a);
        }
    }
}

static Pos FindStart(char[,] map)
{
    for (int x = 0; x < map.GetLength(0); x++)
    {
        if (map[x, 0] == 'S')
        {
            return new Pos(x, 0);
        }
    }
    throw new System.Diagnostics.UnreachableException();
}

record struct Pos(int X, int Y);