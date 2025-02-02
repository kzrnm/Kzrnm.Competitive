using System;
using System.Collections.Generic;
using System.Diagnostics;
using 凾 = System.Runtime.CompilerServices.MethodImplAttribute;

namespace Kzrnm.Competitive
{
    /*
     * Y
     * ↑
     * │ +
     * │
     * │       +
     * │
     * │        +  +
     * │    +           +  +
     * │           +  +
     * │                     +
     * └───────────────────────→ X
     * 
     * 上記のような点の集合があるとする。
     * 
     * Y
     * ↑
     * │ *
     * │
     * │       *
     * │
     * │        $  *
     * │    +           $  *
     * │           +  +
     * │                     *
     * └───────────────────────→ X
     * * は採用される。
     * $ は strict じゃないときは採用される。
     * 
     */

    /// <summary>
    /// 単調減少で全域をカバーする点の集合。コメント参照
    /// </summary>
    public static class DecreasingPoints
    {
        public readonly record struct Result<T, U>(T X, U Y, int Index)
        {
            public Result((T X, U Y) p, int i) : this(p.X, p.Y, i) { }

            [凾(256)]
            public void Deconstruct(out T x, out U y)
            {
                x = X;
                y = Y;
            }
        }

        [凾(256)]
        public static Result<T, U>[] Points<T, U>((T X, U Y)[] s, bool strict = true) where T : IComparable<T> where U : IComparable<U>
            => Points(s, strict, new V<T, U>());
        [凾(256)]
        public static Result<T, U>[] Points<T, U, TCmp, UCmp>((T X, U Y)[] s, TCmp tCmp, UCmp uCmp, bool strict = true)
            where TCmp : IComparer<T>
            where UCmp : IComparer<U>
            => Points(s, strict, new C<T, U, TCmp, UCmp> { tc = tCmp, uc = uCmp });

        interface ICp<T, U>
        {
            void Sort(Span<T> s, Span<int> idx);
            int CmpY(U py, U y);
        }
#pragma warning disable IDE0251 // メンバーを 'readonly' にする
        /// <summary>
        /// <see cref="IComparable{T}"/>, <see cref="IComparable{U}"/> による比較を与える
        /// </summary>
        struct V<T, U> : ICp<T, U> where T : IComparable<T> where U : IComparable<U>
        {
            [凾(256)] public void Sort(Span<T> s, Span<int> idx) => s.Sort(idx);
            [凾(256)] public int CmpY(U py, U y) => py.CompareTo(y);
        }

        /// <summary>
        /// <see cref="IComparer{T}"/>, <see cref="IComparer{U}"/> による比較を与える
        /// </summary>
        struct C<T, U, TCmp, UCmp> : ICp<T, U>
            where TCmp : IComparer<T> where UCmp : IComparer<U>
        {
            public IComparer<T> tc;
            public IComparer<U> uc;
            [凾(256)] public void Sort(Span<T> s, Span<int> idx) => s.Sort(idx, tc);
            [凾(256)] public int CmpY(U py, U y) => uc.Compare(py, y);
        }
#pragma warning restore IDE0251 // メンバーを 'readonly' にする

        [凾(256)]
        static Result<T, U>[] Points<T, U, Cp>((T X, U Y)[] s, bool strict, Cp op = default) where Cp : ICp<T, U>
        {
            if (s.Length == 0)
#if NET8_0_OR_GREATER
                return [];
#else
                return Array.Empty<Result<T, U>>();
#endif
            Span<T> xs = new T[s.Length];
            Span<U> ys = new U[s.Length];
            Span<int> idx = new int[s.Length];

            for (int i = 0; i < s.Length; i++)
            {
                xs[i] = s[i].X;
                idx[i] = i;
            }

            op.Sort(xs, idx);
            for (int i = 0; i < idx.Length; i++)
                ys[i] = s[idx[i]].Y;


            var ls = new List<U> { ys[0] };
            var li = new List<int> { idx[0] };
            for (int i = 1; i < xs.Length; i++)
            {
                Debug.Assert(ls.Count > 0);
                Debug.Assert(ls.Count == li.Count);
                var y = ys[i];

                while (ls.Count > 0)
                {
                    var pi = li[^1];
                    if (EqualityComparer<T>.Default.Equals(xs[i], s[pi].X))
                    {
                        if (op.CmpY(ls[^1], y) < 0)
                        {
                            ls.RemoveAt(ls.Count - 1);
                            li.RemoveAt(li.Count - 1);
                        }
                        else goto FIN;
                    }
                    else if (op.CmpY(ls[^1], y) is var cmp
                            && (cmp < 0 || cmp == 0 && strict))
                    {
                        ls.RemoveAt(ls.Count - 1);
                        li.RemoveAt(li.Count - 1);
                    }
                    else break;
                }
                ls.Add(y);
                li.Add(idx[i]);
            FIN:;
            }

            var rx = li.AsSpan();
            var rt = new Result<T, U>[rx.Length];
            for (int i = rt.Length - 1; i >= 0; i--)
            {
                rt[i] = new(s[rx[i]], rx[i]);
            }
            return rt;
        }
    }
}
