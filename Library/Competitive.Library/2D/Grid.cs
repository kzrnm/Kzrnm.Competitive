using Kzrnm.Competitive.IO;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using 凾 = System.Runtime.CompilerServices.MethodImplAttribute;

namespace Kzrnm.Competitive
{
    public static class Grid
    {
        [凾(256)] public static Grid<IO.Ascii> GridAscii(this ConsoleReader cr, int H, char defaultValue = default) => Create(cr.Repeat(H).Ascii(), defaultValue);
        [凾(256)] public static Grid<int> GridInt(this ConsoleReader cr, int H, int W, int defaultValue = default) => Create(cr.Grid<int>(H, W), defaultValue);
        [凾(256)] public static Grid<long> GridLong(this ConsoleReader cr, int H, int W, long defaultValue = default) => Create(cr.Grid<long>(H, W), defaultValue);
        [凾(256)] public static Grid<ulong> GridULong(this ConsoleReader cr, int H, int W, ulong defaultValue = default) => Create(cr.Grid<ulong>(H, W), defaultValue);
        [凾(256)] public static Grid<char> Create(string[] d, char defaultValue = default) => new Grid<char>(d.Flatten(), d.Length, d[0].Length, defaultValue);
        [凾(256)] public static Grid<IO.Ascii> Create(Asciis[] d, char defaultValue = default) => new Grid<IO.Ascii>(d.Flatten(), d.Length, d[0].Length, defaultValue);
        [凾(256)] public static Grid<T> Create<T>(T[][] d, T defaultValue = default) => new Grid<T>(d.Flatten(), d.Length, d[0].Length, defaultValue);

        [凾(256)]
        public static void WriteGrid(this Utf8ConsoleWriter cw, Grid<char> grid)
        {
            for (int i = 0; i < grid.H; i++)
                cw.WriteLine(grid.RowSpan(i));
        }

        [凾(256)]
        public static void WriteGrid<T>(this Utf8ConsoleWriter cw, Grid<T> grid)
        {
            for (int i = 0; i < grid.H; i++)
                cw.WriteLineJoin(grid.RowSpan(i));
        }
    }

    [DebuggerDisplay("""{ToStringSplit().Replace("\r\n","\n"),raw}""")]
    [DebuggerTypeProxy(typeof(Grid<>.DebugView))]
    public class Grid<T>
    {
        public int Size => data.Length;
        public readonly int H;
        public readonly int W;
        public readonly T[] data;
        internal readonly T defaultValue;
        public Grid(int H, int W, T defaultValue = default) : this(new T[H * W].Fill(defaultValue), H, W, defaultValue) { }
        public Grid(ReadOnlySpan<T> data, int H, int W, T defaultValue = default) : this(data.ToArray(), H, W, defaultValue)
        {
            AtCoder.Internal.Contract.Assert(H * W == data.Length);
        }
        public Grid(Grid<T> other) : this((T[])other.data.Clone(), other.H, other.W, other.defaultValue)
        {
            AtCoder.Internal.Contract.Assert(other.H * other.W == other.data.Length);
        }
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
                defaultReference = defaultValue;
                return ref defaultReference;
            }
        }
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        T defaultReference;

        public Span<T> RowSpan(int h) => (uint)h < (uint)H ? data.AsSpan(h * W, W) : default;
        public Grid<T> Clone() => new Grid<T>(this);

        static string ToStringNoSplit(Grid<T> grid)
        {
            var H = grid.H;
            var W = grid.W;
            var data = grid.data;
            var sb = new StringBuilder();
            for (int h = 0; h + 1 < H; h++)
            {
                Ap(sb, data.AsSpan(h * W, W));
                sb.AppendLine();
            }
            Ap(sb, data.AsSpan((H - 1) * W, W));
            return sb.ToString();
            void Ap(StringBuilder sb, Span<T> s)
            {
                foreach (var v in s)
                    if (typeof(T) == typeof(char))
                        sb.Append((char)(object)v);
                    else
                        sb.Append((char)(IO.Ascii)(object)v);
            }
        }
        string ToStringSplit()
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
            if (typeof(T) == typeof(char) || typeof(T) == typeof(IO.Ascii))
                return ToStringNoSplit(this);
            return ToStringSplit();
        }


        /// <summary>
        /// 時計回りに 90° 回転した <see cref="Grid{T}"/> を返します。
        /// </summary>
        public Grid<T> Rotate90()
        {
            int H = this.H;
            int W = this.W;
            var g = new Grid<T>(W, H, defaultValue);
            for (int h = 0; h < H; h++)
                for (int w = 0; w < W; w++)
                    g[w, H - h - 1] = this[h, w];
            return g;
        }

        /// <summary>
        /// 180° 回転した <see cref="Grid{T}"/> を返します。
        /// </summary>
        public Grid<T> Rotate180()
        {
            var g = new Grid<T>(this);
            g.data.AsSpan().Reverse();
            return g;
        }

        /// <summary>
        /// 時計回りに 270° (反時計回りに 90°)回転した <see cref="Grid{T}"/> を返します。
        /// </summary>
        public Grid<T> Rotate270()
        {
            int H = this.H;
            int W = this.W;
            var g = new Grid<T>(W, H, defaultValue);
            for (int h = 0; h < H; h++)
                for (int w = 0; w < W; w++)
                    g[W - w - 1, h] = this[h, w];
            return g;
        }

        /// <summary>
        /// 縦横を入れ替えた <see cref="Grid{T}"/> を返します。
        /// </summary>
        public Grid<T> Transpose()
        {
            int H = this.H;
            int W = this.W;
            var g = new Grid<T>(W, H, defaultValue);
            for (int h = 0; h < H; h++)
                for (int w = 0; w < W; w++)
                    g[w, h] = this[h, w];
            return g;
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
        public readonly record struct Position(int Index, int Width)
        {
            [凾(256)]
            public void Deconstruct(out int h, out int w)
            {
                h = Math.DivRem(Index, Width, out w);
            }

            [凾(256)]
            public static implicit operator int(Position p) => p.Index;
        }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Style", "IDE0251:メンバーを 'readonly' にする", Justification = "いらん")]
        public record struct MoveEnumerator(
            Grid<T> grid,
            int origH,
            int origW) : IEnumerator<Position>, IEnumerable<Position>
        {
            enum Status
            {
                None,
                Left,
                Up,
                Right,
                Down,
            }
            Status status = Status.None;
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

        public Enumerator GetEnumerator() => new Enumerator(this);
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Style", "IDE0251:メンバーを 'readonly' にする", Justification = "いらん")]
        public struct Enumerator
        {
            readonly Grid<T> g;
            int index;
            internal Enumerator(Grid<T> grid)
            {
                g = grid;
                index = -1;
            }
            public bool MoveNext() => ++index < g.data.Length;
            public (T value, int h, int w) Current
            {
                get
                {
                    var (h, w) = g.FromIndex(index);
                    return (g[index], h, w);
                }
            }
        }

        [SourceExpander.NotEmbeddingSource]
        internal object __ToDebugView() => new DebugView(this);

        [SourceExpander.NotEmbeddingSource]
        readonly record struct DebugLine(
            [property: DebuggerBrowsable(DebuggerBrowsableState.RootHidden)] T[] line)
        {
            public override string ToString()
            {
                if (line is char[] chrs)
                    return new string(chrs);
                if (line is IO.Ascii[] asciis)
                    return new Asciis(Unsafe.As<byte[]>(asciis)).ToString();
                return string.Join(", ", line);
            }
        }
        [SourceExpander.NotEmbeddingSource]
        readonly record struct DebugView(
            [property: DebuggerBrowsable(DebuggerBrowsableState.Never)] Grid<T> grid)
        {
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
