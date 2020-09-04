using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Configs;

[Config(typeof(ShortBenchmarkConfig))]
[GroupBenchmarksBy(BenchmarkLogicalGroupRule.ByCategory)]
[CategoriesColumn]
public class PriorityQueueBench
{
    const int N = 1000000;
    static readonly int[] arr = Util.MakeIntArray(N);
    private int[] pqBuffer = new int[N];

    PriorityQueue<int> pq = new PriorityQueue<int>(N);

    [Benchmark(Baseline = true)]
    [BenchmarkCategory("Add")]
    public void Add()
    {
        pq.Clear();
        foreach (var item in arr) pq.Add(item);
    }

    [Benchmark]
    [BenchmarkCategory("Add")]
    public void AddRef()
    {
        var pq = new PriorityQueueRef<int>(pqBuffer);
        pq.Clear();
        foreach (var item in arr) pq.Add(item);
    }

    [Benchmark(Baseline = true)]
    [BenchmarkCategory("Dequeue")]
    public void AddAndDequeue()
    {
        pq.Clear();
        foreach (var item in arr) pq.Add(item);
        while (pq.Count > 0) pq.Dequeue();
    }

    [Benchmark]
    [BenchmarkCategory("Dequeue")]
    public void AddAndDequeueRef()
    {
        var pq = new PriorityQueueRef<int>(pqBuffer);
        pq.Clear();
        foreach (var item in arr) pq.Add(item);
        while (pq.Count > 0) pq.Dequeue();
    }
}
