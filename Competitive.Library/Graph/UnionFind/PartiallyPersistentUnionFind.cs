using System;
using System.Collections.Generic;
using AtCoder.Internal;

namespace Kzrnm.Competitive
{
    /// <summary>
    /// 部分永続UnionFind。t 回目の <see cref="Merge"/> を行った時点での情報を取得できる。
    /// </summary>
    public class PartiallyPersistentUnionFind
    {
        public int Version { private set; get; }
        internal readonly int _n;
        internal readonly int[] _parentOrRank;
        /// <summary>
        /// 親がいつ更新されたか
        /// </summary>
        internal readonly int[] _updatedVersion;
        /// <summary>
        /// (更新バージョン, 頂点数)
        /// </summary>
        internal readonly SimpleList<(int ver, int size)>[] _num;

        /// <summary>
        /// <see cref="PartiallyPersistentUnionFind"/> クラスの新しいインスタンスを、<paramref name="n"/> 頂点 0 辺のグラフとして初期化します。
        /// </summary>
        public PartiallyPersistentUnionFind(int n)
        {
            _n = n;
            _parentOrRank = new int[n];
            _updatedVersion = new int[n];
            _num = new SimpleList<(int ver, int size)>[n];
            for (int i = 0; i < n; ++i)
            {
                _parentOrRank[i] = -1;
                _num[i] = new SimpleList<(int ver, int size)> { (0, 1) };
            }
            _updatedVersion.AsSpan().Fill(int.MaxValue);
        }

        /// <summary>
        /// 頂点 <paramref name="a"/> と頂点 <paramref name="b"/> を結ぶ辺を追加します。既に追加済みならば false、新たに追加されたならば true を返します。
        /// </summary>
        /// <remarks>
        /// <para>制約: 0≤<paramref name="a"/>, <paramref name="b"/>&lt;n</para>
        /// <para>計算量: O(log(n))</para>
        /// </remarks>
        public bool Merge(int a, int b)
        {
            ++Version;
            a = Leader(a, Version);
            b = Leader(b, Version);
            if (a == b) return false;

            Contract.Assert(_parentOrRank[a] < 0);
            Contract.Assert(_parentOrRank[b] < 0);
            // aの方がランク(木の深さ)が小さい場合は入れ替える
            if (_parentOrRank[a] > _parentOrRank[b]) (a, b) = (b, a);
            else if (_parentOrRank[a] == _parentOrRank[b]) --_parentOrRank[a];

            _num[a].Add((Version, Size(a, Version - 1) + Size(b, Version - 1)));

            // a(ランクが大きい方)を親にする
            _parentOrRank[b] = a;
            _updatedVersion[b] = Version;
            return true;
        }

        /// <summary>
        /// <paramref name="t"/> 回目の <see cref="Merge"/> を行った後に頂点 <paramref name="a"/>, <paramref name="b"/> が連結かどうかを返します。
        /// </summary>
        /// <remarks>
        /// <para>制約: 0≤<paramref name="a"/>, <paramref name="b"/>&lt;n</para>
        /// <para>計算量: O(log(n))</para>
        /// </remarks>
        public bool Same(int a, int b, int t)
        {
            Contract.Assert(0 <= a && a < _n);
            Contract.Assert(0 <= b && b < _n);
            return Leader(a, t) == Leader(b, t);
        }

        /// <summary>
        /// <paramref name="t"/> 回目の <see cref="Merge"/> を行った後の頂点 <paramref name="a"/> の属する連結成分の代表元を返します。
        /// </summary>
        /// <remarks>
        /// <para>制約: 0≤<paramref name="a"/>&lt;n</para>
        /// <para>計算量: O(log(n))</para>
        /// </remarks>
        public int Leader(int a, int t)
        {
            if (t < _updatedVersion[a])
                return a;
            else
                return Leader(_parentOrRank[a], t);
        }

        /// <summary>
        /// <paramref name="t"/> 回目の <see cref="Merge"/> を行った後の頂点 <paramref name="a"/> の属する連結成分のサイズを返します。
        /// </summary>
        /// <remarks>
        /// <para>制約: 0≤<paramref name="a"/>&lt;n</para>
        /// <para>計算量: O(log(n))</para>
        /// </remarks>
        public int Size(int a, int t)
        {
            Contract.Assert(0 <= a && a < _n);
            a = Leader(a, t);

            var num = _num[a].AsSpan();
            int ok = 0, ng = num.Length;
            while (ng - ok > 1)
            {
                var m = (ok + ng) >> 1;
                if (num[m].ver <= t)
                    ok = m;
                else
                    ng = m;
            }
            return num[ok].size;
        }
    }
}
