﻿using System;

public static class Util
{
    public static int[] MakeIntArray(int size)
    {
        var rnd = new Xorshift(42);
        var res = new int[size];
        for (int i = 0; i < res.Length; i++) res[i] = rnd.Next();
        return res;
    }
    public static long[] MakeLongArray(int size)
    {
        var rnd = new Xorshift(42);
        var res = new long[size];
        for (int i = 0; i < res.Length; i++) res[i] = rnd.NextLong();
        return res;
    }
}

class Xorshift
{
    private uint x = 123456789;
    private uint y = 362436069;
    private uint z = 521288629;
    private uint w;
    public Xorshift() : this((uint)DateTime.Now.Millisecond) { }
    public Xorshift(uint seed) { w = seed; }
    public uint NextUInt32()
    {
        uint t = x ^ (x << 11);
        x = y; y = z; z = w;
        return w = (w ^ (w >> 19)) ^ (t ^ (t >> 8));
    }
    public int Next() => (int)NextUInt32();
    public long NextLong() => ((long)NextUInt32() << 32) | NextUInt32();
}