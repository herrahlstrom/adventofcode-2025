Console.WriteLine("Part 1: {0}", Part1());
Console.WriteLine("Part 2: {0}", Part2());

long Part1()
{
    long result = 0;
    foreach (var data in ReadData())
    {
        var values = data.Values.Select(x => long.Parse(x));
        result += data.Operator switch
        {
            '+' => values.Aggregate(0L, (a, n) => a + n),
            '*' => values.Aggregate(1L, (a, n) => a * n),
            _ => throw new System.Diagnostics.UnreachableException()
        };
    }
    return result;
}

long Part2()
{
    long result = 0;
    foreach (var data in ReadData())
    {
        var values = PivotNumbers(data.Values[0].Length, data.Values);
        result += data.Operator switch
        {
            '+' => values.Aggregate(0L, (a, n) => a + n),
            '*' => values.Aggregate(1L, (a, n) => a * n),
            _ => throw new System.Diagnostics.UnreachableException()
        };
    }
    return result;

    IEnumerable<long> PivotNumbers(int length, IList<string> values)
    {
        System.Text.StringBuilder num = new();
        for (int i = length - 1; i >= 0; i--)
        {
            num.Clear();
            foreach (var value in values)
            {
                num.Append(value[i]);
            }
            yield return long.Parse(num.ToString());
        }
    }
}

static IEnumerable<Data> ReadData()
{
    var lines = File.ReadLines("input/06.txt").ToList();
    var operatorLine = lines.Last();
    lines.RemoveAt(lines.Count - 1);

    int start = 0;
    char op = operatorLine[0];
    for (int i = 1; i < operatorLine.Length; i++)
    {
        if (operatorLine[i] is '+' or '*')
        {
            yield return new Data(op, lines.Select(x => x.Substring(start, i - start - 1)).ToList());

            start = i;
            op = operatorLine[i];
        }
    }

    yield return new Data(op, lines.Select(x => x.Substring(start)).ToList());
}

record Data(char Operator, List<string> Values);
