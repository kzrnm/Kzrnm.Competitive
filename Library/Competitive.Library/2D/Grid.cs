using Kzrnm.Competitive.IO;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using 凾 = System.Runtime.CompilerServices.MethodImplAttribute;

namespace Kzrnm.Competitive
{
    public static class Grid
    {
        [凾(256)] public static Grid<char> GridString(this ConsoleReader cr, int H, char defaultValue = default) => Create(cr.Repeat(H).Ascii(), defaultValue);
        [凾(256)] public static Grid<int> GridInt(this ConsoleReader cr, int H, int W, int defaultValue = default) => Create(cr.Grid<int>(H, W), defaultValue);
        [凾(256)] public static Grid<long> GridLong(this ConsoleReader cr, int H, int W, long defaultValue = default) => Create(cr.Grid<long>(H, W), defaultValue);
        [凾(256)] public static Grid<ulong> GridULong(this ConsoleReader cr, int H, int W, ulong defaultValue = default) => Create(cr.Grid<ulong>(H, W), defaultValue);
        [凾(256)] public static Grid<char> Create(string[] data, char defaultValue = default) => new Grid<char>(data.Flatten(), data.Length, data[0].Length, defaultValue);
        [凾(256)] public static Grid<T> Create<T>(T[][] data, T defaultValue = default) => new Grid<T>(data.Flatten(), data.Length, data[0].Length, defaultValue);

        [凾(256)]
        public static void WriteGrid(this Utf8ConsoleWriter cw, Grid<char> grid)
        {
            for (int i = 0; i < grid.H; i++)
                cw.WriteLine(grid.data.AsSpan(i * grid.W, grid.W));
        }

        [凾(256)]
        public static void WriteGrid<T>(this Utf8ConsoleWriter cw, Grid<T> grid)
        {
            for (int i = 0; i < grid.H; i++)
                cw.WriteLineJoin(grid.data.AsSpan(i * grid.W, grid.W));
        }
    }

    [DebuggerDisplay("{ToStringSplit(),raw}")]
    [DebuggerTypeProxy(typeof(Grid<>.DebugView))]
    public class Grid<T>
    {
        public int Size => data.Length;
        public readonly int H;
        public readonly int W;
        public readonly T[] data;
        internal readonly T defaultValue;
        public Grid(int H, int W, T defaultValue = default) : this(new T[H * W].Fill(defaultValue), H, W, defaultValue) { }
        public Grid(Grid<T> other) : this((T[])other.data.Clone(), other.H, other.W, other.defaultValue) { }
        internal Grid(T[] data, int H, int W, T defaultValue)
        {
            this.H = H;
            this.W = W;
            this.data = data;
            this.defaultValue = defaultValue;
        }
        [凾(256)]
        public int Index(int h, int w) =>
            (uint)h < (uint)H && (uint)w < (uint)W
            ? h * W + w
            : -1;
        [凾(256)]
        public (int h, int w) FromIndex(int ix)
        {
            var h = ix / W;
            return (h, ix - h * W);
        }

        private T defaultReference;
        [凾(256)]
        private ref T DefaultValueReference()
        {
            defaultReference = defaultValue;
            return ref defaultReference;
        }
        public ref T this[int h, int w]
        {
            [凾(256)]
            get => ref this[Index(h, w)];
        }
        public ref T this[int index]
        {
            [凾(256)]
            get
            {
                if ((uint)index < (uint)data.Length)
                    return ref data[index];
                return ref DefaultValueReference();
            }
        }
        public Span<T> RowSpan(int h) => (uint)h < (uint)H ? data.AsSpan(h * W, W) : default;
        public Grid<T> Clone() => new Grid<T>(this);

        private static string ToStringNoSplit(Grid<char> grid)
        {
            var H = grid.H;
            var W = grid.W;
            var data = grid.data;
            var sb = new StringBuilder();
            for (int h = 0; h + 1 < H; h++)
            {
                sb.Append(data.AsSpan(h * W, W));
                sb.AppendLine();
            }
            sb.Append(data.AsSpan((H - 1) * W, W));
            return sb.ToString();
        }
        private string ToStringSplit()
        {
            var sb = new StringBuilder();
            for (int h = 0; h < H; h++)
            {
                sb.Append(this[h, 0]);
                for (int w = 1; w < W; w++)
                {
                    sb.Append(' ');
                    sb.Append(this[h, w]);
                }
                if (h + 1 < H)
                    sb.AppendLine();
            }
            return sb.ToString();
        }
        public override string ToString()
        {
            if (W == 0) return "";
            if (typeof(T) == typeof(char))
                return ToStringNoSplit((Grid<char>)(object)this);
            return ToStringSplit();
        }

        /// <summary>
        /// 対象の上下左右の座標を返します。
        /// </summary>
        [凾(256)]
        public MoveEnumerator Moves(int index)
        {
            var (h, w) = FromIndex(index);
            return new MoveEnumerator(this, h, w);
        }

        /// <summary>
        /// 対象の上下左右の座標を返します。
        /// </summary>
        [凾(256)]
        public MoveEnumerator Moves(int h, int w) => new MoveEnumerator(this, h, w);
#if NET7_0_OR_GREATER
        public readonly record struct Position(int Index, int Width)
        {
#else
        public readonly struct Position : IEquatable<Position>
        {
            public int Index { get; }
            public int Width { get; }
            public Position(int index, int width) { Index = index; Width = width; }
            public bool Equals(Position other) => Index == other.Index && Width == other.Width;
#endif
            [凾(256)]
            public void Deconstruct(out int h, out int w)
            {
                h = Math.DivRem(Index, Width, out w);
            }

            [凾(256)]
            public static implicit operator int(Position p) => p.Index;
        }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Style", "IDE0251:メンバーを 'readonly' にする", Justification = "いらん")]
        public struct MoveEnumerator : IEnumerator<Position>, IEnumerable<Position>
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
            [凾(256)]
            public MoveEnumerator(Grid<T> grid, int h, int w)
            {
                this.grid = grid;
                origH = h;
                origW = w;
                status = Status.None;
            }
            public Position Current
            {
                [凾(256)]
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
                    return new Position(grid.Index(origH + dh, origW + dw), grid.W);
                }
            }

            object IEnumerator.Current => Current;

            [凾(256)]
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

            [凾(256)]
            public MoveEnumerator GetEnumerator() => this;
            IEnumerator<Position> IEnumerable<Position>.GetEnumerator() => this;
            IEnumerator IEnumerable.GetEnumerator() => this;
            public void Reset() => status = Status.None;
            public void Dispose() { }
        }
#if !LIBRARY
        [SourceExpander.NotEmbeddingSource]
#endif
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
#if !LIBRARY
        [SourceExpander.NotEmbeddingSource]
#endif
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
