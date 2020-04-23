class Sums
{
    private long[] impl;
    public int Length { get; }
    public Sums(int[] arr)
    {
        this.Length = arr.Length;
        impl = new long[arr.Length + 1];
        for (var i = 0; i < arr.Length; i++)
            impl[i + 1] = impl[i] + arr[i];
    }
    public long this[int toExclusive]
    {
        get { return impl[toExclusive]; }
    }
    public long this[int from, int toExclusive]
    {
        get { return impl[toExclusive] - impl[from]; }
    }
}
