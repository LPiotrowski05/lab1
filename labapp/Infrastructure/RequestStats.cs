using System.Collections.Concurrent;

namespace labapp.Infrastructure;

public class RequestStats
{
    private long _total;

    public long Total => Interlocked.Read(ref _total);
    public long Hit() => Interlocked.Increment(ref _total);

    public ConcurrentDictionary<string, long> Paths { get; } = new(StringComparer.OrdinalIgnoreCase);
    public void HitPath(string path) => Paths.AddOrUpdate(path, 1, (_, v) => v + 1);
}