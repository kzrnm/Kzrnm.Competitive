using AtCoder.Internal;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using 凾 = System.Runtime.CompilerServices.MethodImplAttribute;

namespace Kzrnm.Competitive
{
    // https://nyaannyaan.github.io/library/data-structure/rollback-union-find.hpp
    /// <summary>
    /// ロールバック可能なUnionFind
    /// </summary>
    [DebuggerDisplay("Count = {" + nameof(_ps) + "." + nameof(Array.Length) + "}")]
    public class RollbackUnionFind
    {
        /// <summary>
        /// Parent or size. A negative value indicates size, a positive value indicates parent.
        /// </summary>
        internal readonly int[] _ps;
        internal readonly SimpleList<(int, int)> history = new();
        internal int snapVersion;

        /// <summary>
        /// マージされた回数
        /// </summary>
        public int Version => history.Count >> 1;

        /// <summary>
        /// <see cref="RollbackUnionFind"/> クラスの新しいインスタンスを、<paramref name="n"/> 頂点 0 辺のグラフとして初期化します。
        /// </summary>
        /// <remarks>
        /// <para>制約: 0≤<paramref name="n"/>≤10^8</para>
        /// <para>計算量: O(<paramref name="n"/>)</para>
        /// </remarks>
        public RollbackUnionFind(int n)
        {
            _ps = new int[n];
            _ps.AsSpan().Fill(-1);
        }

        /// <summary>
        /// 頂点 <paramref name="a"/> と頂点 <paramref name="b"/> を結ぶ辺を追加します。既に追加済みならば false、新たに追加されたならば true を返します。
        /// </summary>
        /// <remarks>
        /// <para>制約: 0≤<paramref name="a"/>, <paramref name="b"/>&lt;n</para>
        /// <para>計算量: O(log n)</para>
        /// </remarks>
        [凾(256)]
        public bool Merge(int a, int b)
        {
            Contract.Assert(0 <= a && a < _ps.Length);
            Contract.Assert(0 <= b && b < _ps.Length);
            int x = Leader(a), y = Leader(b);
            history.Add((x, _ps[x]));
            history.Add((y, _ps[y]));
            if (x == y) return false;
            if (_ps[x] > _ps[y]) (x, y) = (y, x);
            _ps[x] += _ps[y];
            _ps[y] = x;
            return true;
        }

        /// <summary>
        /// 頂点 <paramref name="a"/>, <paramref name="b"/> が連結かどうかを返します。
        /// </summary>
        /// <remarks>
        /// <para>制約: 0≤<paramref name="a"/>, <paramref name="b"/>&lt;n</para>
        /// <para>計算量: O(log n)</para>
        /// </remarks>
        [凾(256)]
        public bool Same(int a, int b)
        {
            Contract.Assert(0 <= a && a < _ps.Length);
            Contract.Assert(0 <= b && b < _ps.Length);
            return Leader(a) == Leader(b);
        }

        /// <summary>
        /// 頂点 <paramref name="a"/> の属する連結成分の代表元を返します。
        /// </summary>
        /// <remarks>
        /// <para>制約: 0≤<paramref name="a"/>&lt;n</para>
        /// <para>計算量: O(log n)</para>
        /// </remarks>
        [凾(256)]
        public int Leader(int a)
        {
            if (_ps[a] < 0) return a;
            return Leader(_ps[a]);
        }


        /// <summary>
        /// 頂点 <paramref name="a"/> の属する連結成分のサイズを返します。
        /// </summary>
        /// <remarks>
        /// <para>制約: 0≤<paramref name="a"/>&lt;n</para>
        /// <para>計算量: O(log n)</para>
        /// </remarks>
        [凾(256)]
        public int Size(int a)
        {
            Contract.Assert(0 <= a && a < _ps.Length);
            return -_ps[Leader(a)];
        }

        /// <summary>
        /// 直前のマージを取り消します。
        /// </summary>
        /// <remarks>
        /// <para>計算量: O(1)</para>
        /// </remarks>
        [凾(256)]
        public void Undo()
        {
            var (x, px) = history[^1];
            var (y, py) = history[^2];
            history.RemoveLast(2);
            _ps[x] = px;
            _ps[y] = py;
        }

        /// <summary>
        /// ロールバック先のスナップショットを取得します。
        /// </summary>
        /// <remarks>
        /// <para>計算量: O(1)</para>
        /// </remarks>
        [凾(256)]
        public void Snapshot()
        {
            snapVersion = Version;
        }

        /// <summary>
        /// <paramref name="version"/> までロールバックします。<paramref name="version"/> が負のときはスナップショットまでロールバックします。
        /// </summary>
        /// <remarks>
        /// <para>計算量: O(1)</para>
        /// </remarks>
        [凾(256)]
        public void Rollback(int version = -1)
        {
            if (version < 0) version = snapVersion;
            version <<= 1;
            Contract.Assert(version <= history.Count);
            while (version < history.Count)
                Undo();
        }

        /// <summary>
        /// グラフを連結成分に分け、その情報を返します。
        /// </summary>
        /// <para>計算量: O(n log n)</para>
        /// <returns>「一つの連結成分の頂点番号のリスト」のリスト。</returns>
        [凾(256)]
        public int[][] Groups() => GroupsAndIds().Groups;

        /// <summary>
        /// グラフを連結成分に分け、そのIDを返します。
        /// </summary>
        /// <para>計算量: O(n log n)</para>
        /// <returns>頂点番号に対応する連結成分のID。</returns>
        [凾(256)]
        public int[] GroupIds() => GroupsAndIds().GroupIds;

        /// <summary>
        /// グラフを連結成分に分け、その情報を返します。
        /// </summary>
        /// <para>計算量: O(n log n)</para>
        /// <returns>「一つの連結成分の頂点番号のリスト」のリスト, 頂点番号に対応する連結成分のID。</returns>
        public (int[][] Groups, int[] GroupIds) GroupsAndIds()
        {
            var leaderBuf = new int[_ps.Length];
            var id = new int[_ps.Length];
            var gr = new int[_ps.Length];
            var resultList = new List<int[]>(_ps.Length);
            for (int i = 0; i < leaderBuf.Length; i++)
            {
                leaderBuf[i] = Leader(i);
                if (i == leaderBuf[i])
                {
                    id[i] = resultList.Count;
                    resultList.Add(new int[-_ps[i]]);
                }
            }
            var result = resultList.ToArray();
            var ind = new int[result.Length];
            for (int i = 0; i < leaderBuf.Length; i++)
            {
                var leaderID = id[leaderBuf[i]];
                gr[i] = leaderID;
                result[leaderID][ind[leaderID]] = i;
                ind[leaderID]++;
            }
            return (result, gr);
        }
    }
}
