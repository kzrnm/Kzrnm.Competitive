using System;
using System.Collections.Generic;
using System.Numerics;
using System.Runtime.CompilerServices;
using System.Text;

namespace Kzrnm.Competitive
{
    /// <summary>
    /// mod Nで循環しているなどのときに 前計算 O(N) で K 個先の遷移を O(1) で求められるデータ構造
    /// </summary>
    public class PathLoop
    {
        /// <summary>
        /// <para><paramref name="to"/>: 頂点 i からの遷移先。負数は遷移先なしとする。</para>
        /// <para><paramref name="start"/>: どの頂点からのパスを見るか</para>
        /// <para>制約: <paramref name="to"/>[i] &lt; |<paramref name="to"/>|, 0 ≦ <paramref name="start"/> &lt; |<paramref name="to"/>|</para>
        /// <para>計算量: O(N)</para>
        /// </summary>
        /// <param name="to">数値 i からの遷移先</param>
        /// <param name="start">どの頂点からのパスを見るか</param>
        public PathLoop(int[] to, int start)
        {
            var used = new int[to.Length];
            var list = new SList<int>(to.Length);
            int cur = start;
            while (used[cur] == 0)
            {
                list.Add(cur);
                used[cur] = list.Count;
                cur = to[cur];
                if ((uint)cur >= (uint)to.Length)
                {
                    _straight = list.ToArray();
                    _loop = null;
                    return;
                }
            }
            var ix = used[cur] - 1;
            _straight = list.AsSpan()[..ix].ToArray();
            _loop = list.AsSpan()[ix..].ToArray();
        }
        internal readonly int[] _straight;
        internal readonly int[] _loop;
        public int StraightSize => _straight.Length;
        public int LoopSize => _loop.Length;

        /// <summary>
        /// <para><paramref name="moveNum"/>: 移動回数</para>
        /// </summary>
        /// <param name="moveNum">: 移動回数</param>
        /// <returns></returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public int Move(long moveNum) => Move((ulong)moveNum);
        /// <summary>
        /// <para><paramref name="moveNum"/>: 移動回数</para>
        /// </summary>
        /// <param name="moveNum">: 移動回数</param>
        /// <returns></returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public int Move(ulong moveNum)
        {
            if (moveNum < (ulong)_straight.Length)
                return _straight[(int)moveNum];
            if (_loop == null) return -1;
            moveNum -= (ulong)_straight.Length;
            return _loop[(int)(moveNum % (ulong)_loop.Length)];
        }
        /// <summary>
        /// <para><paramref name="moveNum"/>: 移動回数</para>
        /// </summary>
        /// <param name="moveNum">: 移動回数</param>
        /// <returns></returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public int Move(BigInteger moveNum)
        {
            if (moveNum < _straight.Length)
                return _straight[(int)moveNum];
            if (_loop == null) return -1;
            moveNum -= _straight.Length;
            return _loop[(int)(moveNum % _loop.Length)];
        }
    }
}
