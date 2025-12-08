Console.WriteLine("Part 1: {0}", Part1());
Console.WriteLine("Part 2: {0}", Part2());

long Part1()
{
    List<Pos> positions = ReadPositions().ToList();
    List<Connection> shortestConnections = GetShortestConnections(positions, 1000);
    List<List<Pos>> circuits = ConnectCircuts(shortestConnections);

    circuits.Sort((a, b) => -a.Count.CompareTo(b.Count));
    return circuits.Select(x => x.Count).Take(3).Aggregate(1, (a, b) => a * b);
}

long Part2()
{
    List<Pos> positions = ReadPositions().ToList();
    List<Connection> shortestConnections = GetConnections(positions);

    long result = 0;
    List<List<Pos>> circuits = ConnectCircuts(shortestConnections, (a, b) => { result = (long)a.X * (long)b.X; });

    return result;
}

static List<List<Pos>> ConnectCircuts(IEnumerable<Connection> connections, Action<Pos, Pos>? onMerge = null)
{
    List<List<Pos>> circuts = [];

    foreach (var connection in connections)
    {
        var circuitWithA = circuts.FirstOrDefault(x => x.Contains(connection.A));
        var circuitWithB = circuts.FirstOrDefault(x => x.Contains(connection.B));

        if (circuitWithA == default && circuitWithB == default)
        {
            circuts.Add([connection.A, connection.B]);
        }
        else if (circuitWithA == default)
        {
            circuitWithB.Add(connection.A);
        }
        else if (circuitWithB == default)
        {
            circuitWithA.Add(connection.B);
        }
        else if (circuitWithA != circuitWithB)
        {
            circuts.Add([.. circuitWithA, .. circuitWithB]);
            circuts.Remove(circuitWithA);
            circuts.Remove(circuitWithB);

            onMerge?.Invoke(connection.A, connection.B);
        }
    }
    return circuts;
}

static List<Connection> GetConnections(IList<Pos> positions)
{
    List<Connection> result = new List<Connection>();

    for (int i = 0; i < positions.Count; i++)
    {
        for (int j = i + 1; j < positions.Count; j++)
        {
            var distance = GetDistance(positions[i], positions[j]);
            result.Add(new Connection(positions[i], positions[j], distance));
        }
    }

    result.Sort((a, b) => a.Distance.CompareTo(b.Distance));
    return result;
}

static List<Connection> GetShortestConnections(IList<Pos> positions, int maxConnections)
{
    List<Connection> result = new List<Connection>(maxConnections + 1);
    double threshold = double.MaxValue;

    for (int i = 0; i < positions.Count; i++)
    {
        for (int j = i + 1; j < positions.Count; j++)
        {
            var distance = GetDistance(positions[i], positions[j]);

            if (distance > threshold)
                continue;

            result.Add(new Connection(positions[i], positions[j], distance));
            if (result.Count > maxConnections)
            {
                result.Sort((a, b) => a.Distance.CompareTo(b.Distance));
                result.RemoveRange(maxConnections, result.Count - maxConnections);
                threshold = result.Last().Distance;
            }
        }
    }
    return result;
}

static IEnumerable<Pos> ReadPositions()
{
    foreach (var line in File.ReadLines("input/08.txt"))
    {
        var arr = line.Split(',');
        yield return new Pos(int.Parse(arr[0]), int.Parse(arr[1]), int.Parse(arr[2]));
    }
}

static double GetDistance(Pos a, Pos b)
{
    return Math.Sqrt(
        Math.Pow(a.X - b.X, 2) +
        Math.Pow(a.Y - b.Y, 2) +
        Math.Pow(a.Z - b.Z, 2));
}

record struct Pos(int X, int Y, int Z)
{
    public override string ToString() => $"{{{X},{Y},{Z}}}";
}
record struct Connection(Pos A, Pos B, double Distance);
