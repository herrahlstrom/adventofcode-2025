Console.WriteLine("Part 1: {0}", Part1());
Console.WriteLine("Part 2: {0}", Part2());

long Part1()
{
    var map = ReadMap();

    long result = 0;

    for (int y = 0; y < map.GetLength(1); y++)
    {
        for (int x = 0; x < map.GetLength(0); x++)
        {
            if (map[x, y] == '@' && CanUseForklift(map, new Pos(x, y)))
                result += 1;
        }
    }

    return result;
}

long Part2()
{
    var map = ReadMap();

    long result = 0;

    List<Pos> session = [];
    do
    {
        session.Clear();
        for (int y = 0; y < map.GetLength(1); y++)
        {
            for (int x = 0; x < map.GetLength(0); x++)
            {
                var pos = new Pos(x, y);
                if (map[x, y] == '@' && CanUseForklift(map, pos))
                    session.Add(pos);
            }
        }

        result += session.Count;
        foreach (var pos in session)
        {
            map[pos.X, pos.Y] = 'x';
        }
    } while (session.Any());
    return result;
}

static bool CanUseForklift(char[,] map, Pos pos)
{
    var adjacentRollsOfPaper = GetAdjacentPositions(pos)
        .Where(pos => InMap(map, pos))
        .Where(pos => map[pos.X, pos.Y] == '@')
        .Count();
    return adjacentRollsOfPaper < 4;
}

static IEnumerable<Pos> GetAdjacentPositions(Pos pos)
{
    yield return new Pos(pos.X - 1, pos.Y - 1);
    yield return new Pos(pos.X, pos.Y - 1);
    yield return new Pos(pos.X + 1, pos.Y - 1);

    yield return new Pos(pos.X - 1, pos.Y);
    yield return new Pos(pos.X + 1, pos.Y);

    yield return new Pos(pos.X - 1, pos.Y + 1);
    yield return new Pos(pos.X, pos.Y + 1);
    yield return new Pos(pos.X + 1, pos.Y + 1);
}

static bool InMap(char[,] map, Pos pos)
{
    return pos.X >= 0 && pos.Y >= 0 && pos.X < map.GetLength(0) && pos.Y < map.GetLength(1);
}

static char[,] ReadMap()
{
    var lines = File.ReadLines("input/04.txt").ToList();
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

record struct Pos(int X, int Y);