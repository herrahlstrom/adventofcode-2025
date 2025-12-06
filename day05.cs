Console.WriteLine("Part 1: {0}", Part1());
Console.WriteLine("Part 2: {0}", Part2());

long Part1()
{
    var data = ReadData();
    int result = 0;

    foreach (var value in data.Values)
    {
        if (data.Range.Any(r => r.Contains(value)))
            result += 1;
    }

    return result;
}

long Part2()
{
    var data = ReadData();
    List<Range> ranges = [data.Range.First()];

    foreach (var range in data.Range.Skip(1))
    {
        ranges.AddRange(GetRanges(range));
    }

    return ranges.Sum(r => r.End - r.Start + 1);

    IEnumerable<Range> GetRanges(Range range)
    {
        while (true)
        {
            TrimRange(ref range);
            if (range.IsEmpty)
            {
                break;
            }

            var firstRangeWithOverlap = ranges.Where(x => x.Start < range.End && x.Start > range.Start).OrderBy(range => range.Start).FirstOrDefault();
            if (firstRangeWithOverlap is null)
            {
                yield return range;
                break;
            }
            else if (firstRangeWithOverlap.End >= range.End)
            {
                yield return new Range(range.Start, firstRangeWithOverlap.Start - 1);
                break;
            }
            else
            {
                yield return new Range(range.Start, firstRangeWithOverlap.Start - 1);
                range = new Range(firstRangeWithOverlap.End + 1, range.End);
            }
        }
    }

    void TrimRange(ref Range range)
    {
        bool doneWithTrim = false;
        while (!doneWithTrim)
        {
            doneWithTrim = true;
            for (var i = 0; i < ranges.Count; i++)
            {
                if (ranges[i].Contains(range.Start))
                {
                    if (ranges[i].End >= range.End)
                    {
                        range = Range.Empty;
                        return;
                    }

                    range = new Range(ranges[i].End + 1, range.End);

                    doneWithTrim = false;
                    break;
                }

                if (ranges[i].Contains(range.End))
                {
                    range = new Range(range.Start, ranges[i].Start - 1);
                    doneWithTrim = false;
                    break;
                }
                if (range.Start > range.End)
                {
                    range = new Range(default, default);
                    return;
                }
            }
        }
    }
}

static Data ReadData()
{
    var enumerator = File.ReadLines("input/05.txt").GetEnumerator();

    List<Range> ranges = [];
    List<long> values = [];

    while (enumerator.MoveNext())
    {
        if (string.IsNullOrWhiteSpace(enumerator.Current))
            break;

        var span = enumerator.Current.AsSpan();
        var index = span.IndexOf('-');
        var start = long.Parse(span.Slice(0, index));
        var end = long.Parse(span.Slice(index + 1));

        ranges.Add(new Range(start, end));
    }

    while (enumerator.MoveNext())
    {
        if (string.IsNullOrWhiteSpace(enumerator.Current))
            break;

        values.Add(long.Parse(enumerator.Current));
    }

    return new Data(ranges, values);
}

record Data(List<Range> Range, List<long> Values);
record Range(long Start, long End)
{
    public bool Contains(long value) => value >= Start && value <= End;
    public bool IsEmpty => Start == 0 && End == 0;
    public static Range Empty { get; } = new Range(0, 0);
}
