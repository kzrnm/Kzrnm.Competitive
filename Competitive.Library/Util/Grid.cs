using Kzrnm.Competitive.IO;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;

namespace Kzrnm.Competitive
{
    using static MethodImplOptions;
    public static class Grid
    {
        public static Grid<char> GridString(this PropertyConsoleReader cr, int H) => Create(cr.Repeat(H).Ascii);
        public static Grid<char> GridString(this PropertyConsoleReader cr, int H, char defaultValue) => Create(cr.Repeat(H).Ascii, defaultValue);
        public static Grid<int> GridInt(this PropertyConsoleReader cr, int H, int W) => Create(cr.Repeat(H).Select(cr => cr.Repeat(W).Int));
        public static Grid<int> GridInt(this PropertyConsoleReader cr, int H, int W, int defaultValue) => Create(cr.Repeat(H).Select(cr => cr.Repeat(W).Int), defaultValue);
        public static Grid<char> Create(string[] data) => new Grid<char>(data.Flatten(), data.Length, data[0].Length, default(char));
        public static Grid<char> Create(string[] data, char defaultValue) => new Grid<char>(data.Flatten(), data.Length, data[0].Length, defaultValue);
        public static Grid<T> Create<T>(T[][] data) => new Grid<T>(data.Flatten(), data.Length, data[0].Length, default(T));
        public static Grid<T> Create<T>(T[][] data, T defaultValue) => new Grid<T>(data.Flatten(), data.Length, data[0].Length, defaultValue);
        public static Grid<T> Create<T>(Span<T[]> data) => new Grid<T>(data.Flatten(), data.Length, data[0].Length, default(T));
        public static Grid<T> Create<T>(Span<T[]> data, T defaultValue) => new Grid<T>(data.Flatten(), data.Length, data[0].Length, defaultValue);
        public static Grid<T> Create<T>(ReadOnlySpan<T[]> data) => new Grid<T>(data.Flatten(), data.Length, data[0].Length, default(T));
        public static Grid<T> Create<T>(ReadOnlySpan<T[]> data, T defaultValue) => new Grid<T>(data.Flatten(), data.Length, data[0].Length, defaultValue);
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
    [DebuggerTypeProxy(typeof(Grid<>.DebugView))]
    public class Grid<T>
    {
        public int Size => data.Length;
        public readonly int H;
        public readonly int W;
        internal readonly T[] data;
        private readonly T defaultValue;
        public Grid(int H, int W, T defaultValue = default(T)) : this(new T[H * W].Fill(defaultValue), H, W, defaultValue) { }
        internal Grid(T[] data, int H, int W, T defaultValue)
        {
            this.H = H;
            this.W = W;
            this.data = data;
            this.defaultValue = defaultValue;
        }
        [MethodImpl(AggressiveInlining)]
        public int Index(int h, int w) =>
            (uint)h < (uint)H && (uint)w < (uint)W
            ? h * W + w
            : -1;
        [MethodImpl(AggressiveInlining)]
        public (int h, int w) FromIndex(int ix)
        {
            var h = ix / W;
            return (h, ix - h * W);
        }

        private T defaultReference;
        [MethodImpl(AggressiveInlining)]
        private ref T DefaultValueReference()
        {
            defaultReference = defaultValue;
            return ref defaultReference;
        }
        public ref T this[int h, int w]
        {
            [MethodImpl(AggressiveInlining)]
            get => ref this[Index(h, w)];
        }
        public ref T this[int index]
        {
            [MethodImpl(AggressiveInlining)]
            get
            {
                if ((uint)index < (uint)data.Length)
                    return ref data[index];
                return ref DefaultValueReference();
            }
        }
        [MethodImpl(AggressiveInlining)]
        public MoveEnumerator Moves(int h, int w) => new MoveEnumerator(this, h, w);
        public struct MoveEnumerator : IEnumerator<(int h, int w)>, IEnumerable<(int h, int w)>
        {
            private enum Status
            {
                None,
                Left,
                Up,
                Right,
                Down,
            }
            private readonly Grid<T> grid;
            private readonly int origH, origW;
            private Status status;
            [MethodImpl(AggressiveInlining)]
            public MoveEnumerator(Grid<T> grid, int h, int w)
            {
                this.grid = grid;
                this.origH = h;
                this.origW = w;
                this.status = Status.None;
            }
            public (int h, int w) Current
            {
                [MethodImpl(AggressiveInlining)]
                get
                {
                    int dh = 0;
                    int dw = 0;
                    switch (status)
                    {
                        case Status.Up:
                            dh = -1;
                            break;
                        case Status.Right:
                            dw = 1;
                            break;
                        case Status.Left:
                            dw = -1;
                            break;
                        case Status.Down:
                            dh = 1;
                            break;
                    }
                    return (origH + dh, origW + dw);
                }
            }

            object IEnumerator.Current => this.Current;

            [MethodImpl(AggressiveInlining)]
            public bool MoveNext()
            {
                switch (status)
                {
                    case Status.None:
                        if (origW > 0)
                            status = Status.Left;
                        else
                            goto case Status.Left;
                        return true;
                    case Status.Left:
                        if (origH > 0)
                            status = Status.Up;
                        else
                            goto case Status.Up;
                        return true;
                    case Status.Up:
                        if (origW + 1 < grid.W)
                            status = Status.Right;
                        else
                            goto case Status.Right;
                        return true;
                    case Status.Right:
                        if (origH + 1 < grid.H)
                            status = Status.Down;
                        else
                            goto default;
                        return true;
                    default:
                        status = Status.None;
                        return false;
                }
            }

            [MethodImpl(AggressiveInlining)]
            public MoveEnumerator GetEnumerator() => this;
            IEnumerator<(int h, int w)> IEnumerable<(int h, int w)>.GetEnumerator() => this;
            IEnumerator IEnumerable.GetEnumerator() => this;
            public void Reset() => status = Status.None;
            public void Dispose() { }
        }
        private class DebugLine
        {
            [DebuggerBrowsable(DebuggerBrowsableState.RootHidden)]
            private T[] line;
            public DebugLine(T[] line)
            {
                this.line = line;
            }
            public override string ToString()
            {
                if (line is char[] chrs)
                    return new string(chrs);
                return string.Join(", ", line);
            }
        }
        private class DebugView
        {
            private readonly Grid<T> grid;
            public DebugView(Grid<T> grid)
            {
                this.grid = grid;
            }
            [DebuggerBrowsable(DebuggerBrowsableState.RootHidden)]
            public DebugLine[] Items
            {
                get
                {
                    var items = new DebugLine[grid.H];
                    for (int h = 0; h < grid.H; h++)
                    {
                        var line = new T[grid.W];
                        for (int w = 0; w < grid.W; w++)
                            line[w] = grid[h, w];
                        items[h] = new DebugLine(line);
                    }
                    return items;
                }
            }
        }
    }
}
