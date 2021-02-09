using Kzrnm.Competitive.IO;
using System;
using System.Runtime.CompilerServices;

namespace AtCoder
{
    public static class Grid
    {
        public static Grid<char> GridString(this PropertyConsoleReader cr, int H) => Create(cr.Repeat(H).Ascii);
        public static Grid<char> GridString(this PropertyConsoleReader cr, int H, char defaultValue) => Create(cr.Repeat(H).Ascii, defaultValue);
        public static Grid<int> GridInt(this PropertyConsoleReader cr, int H, int W) => Create(cr.Repeat(H).Select(cr => cr.Repeat(W).Int));
        public static Grid<int> GridInt(this PropertyConsoleReader cr, int H, int W, int defaultValue) => Create(cr.Repeat(H).Select(cr => cr.Repeat(W).Int), defaultValue);
        public static Grid<char> Create(string[] data) => new Grid<char>(CollectionUtil.Flatten(data), data.Length, data[0].Length);
        public static Grid<char> Create(string[] data, char defaultValue) => new Grid<char>(CollectionUtil.Flatten(data), data.Length, data[0].Length, defaultValue);
        public static Grid<T> Create<T>(T[][] data) => new Grid<T>(CollectionUtil.Flatten(data), data.Length, data[0].Length);
        public static Grid<T> Create<T>(T[][] data, T defaultValue) => new Grid<T>(CollectionUtil.Flatten(data), data.Length, data[0].Length, defaultValue);
        public static Grid<T> Create<T>(Span<T[]> data) => new Grid<T>(CollectionUtil.Flatten(data), data.Length, data[0].Length);
        public static Grid<T> Create<T>(Span<T[]> data, T defaultValue) => new Grid<T>(CollectionUtil.Flatten(data), data.Length, data[0].Length, defaultValue);
        public static Grid<T> Create<T>(ReadOnlySpan<T[]> data) => new Grid<T>(CollectionUtil.Flatten(data), data.Length, data[0].Length);
        public static Grid<T> Create<T>(ReadOnlySpan<T[]> data, T defaultValue) => new Grid<T>(CollectionUtil.Flatten(data), data.Length, data[0].Length, defaultValue);
        public static void WriteGrid(this Grid<char> grid, ConsoleWriter cw)
        {
            for (int i = 0; i < grid.H; i++)
                cw.StreamWriter.WriteLine(grid.data.AsSpan(i * grid.W, grid.W));
        }
        public static void WriteGrid<T>(this Grid<T> grid, ConsoleWriter cw)
        {
            for (int i = 0; i < grid.H; i++)
                cw.WriteLineJoin(grid.data.AsSpan(i * grid.W, grid.W));
        }
    }
    public class Grid<T>
    {
        public readonly int H;
        public readonly int W;
        internal readonly T[] data;
        private readonly T defaultValue;
        public Grid(int H, int W, T defaultValue = default(T)) : this(new T[H * W].Fill(defaultValue), H, W, defaultValue) { }
        internal Grid(T[] data, int H, int W, T defaultValue = default(T))
        {
            this.H = H;
            this.W = W;
            this.data = data;
            this.defaultValue = defaultValue;
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public int Index(int h, int w) => h * W + w;

        private T defaultReference;
        private ref T DefaultValueReference()
        {
            defaultReference = defaultValue;
            return ref defaultReference;
        }
        public ref T this[int h, int w]
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get
            {
                if ((uint)h < (uint)H && (uint)w < (uint)W)
                    return ref data[Index(h, w)];
                return ref DefaultValueReference();
            }
        }
    }
}
