using System;
using System.Linq;
using System.Numerics;
using 凾 = System.Runtime.CompilerServices.MethodImplAttribute;

namespace Kzrnm.Competitive
{
    public static class Knapsack
    {
        /// <summary>
        /// 重さ <paramref name="wv"/>[i].W, 価値 <paramref name="wv"/>[i].V の品物を、重さの和が <paramref name="W"/> 以下になるように0個または1個選んだときの価値の和の最大値を返します。
        /// </summary>
        /// <returns>r[i] が「重さの和が i となるときの価値の和の最大値」である配列 r</returns>
        /// <remarks>
        /// <para>計算量: O(NW)</para>
        /// <para>制約: <paramref name="wv"/>.W は非負である</para>
        /// </remarks>
        public static T[] SmallWeight<T>((int W, T V)[] wv, int W) where T : INumber<T>, IMinMaxValue<T>
        {
            var rt = new T[W + 1];
            rt.AsSpan(1).Fill(T.MinValue / (T.One + T.One));
            foreach (var (w, v) in wv)
            {
                for (int i = rt.Length - 1 - w; i >= 0; i--)
                {
                    ref var tv = ref rt[i + w];
                    var tx = rt[i] + v;
                    if (tv < tx) tv = tx;
                }
            }
            return rt;
        }

        /// <summary>
        /// 重さ <paramref name="wv"/>[i].W, 価値 <paramref name="wv"/>[i].V の品物を、重さの和が <paramref name="W"/> 以下になるように0個以上選んだときの価値の和の最大値を返します。
        /// </summary>
        /// <returns>r[i] が「重さの和が i となるときの価値の和の最大値」である配列 r</returns>
        /// <remarks>
        /// <para>計算量: O(NW)</para>
        /// <para>制約: <paramref name="wv"/>.W は非負である</para>
        /// </remarks>
        public static T[] SmallWeightUnlimited<T>((int W, T V)[] wv, int W) where T : INumber<T>, IMinMaxValue<T>
        {
            var rt = new T[W + 1];
            rt.AsSpan(1).Fill(T.MinValue / (T.One + T.One));
            foreach (var (w, v) in wv)
            {
                for (int i = 0; i + w < rt.Length; i++)
                {
                    ref var tv = ref rt[i + w];
                    var tx = rt[i] + v;
                    if (tv < tx) tv = tx;
                }
            }
            return rt;
        }

        /// <summary>
        /// 重さ <paramref name="wv"/>[i].W, 価値 <paramref name="wv"/>[i].V の品物を、0個または1個選んだときの重さの和の最小値を返します。
        /// </summary>
        /// <returns>r[i] が「価値の和が i となるときの重さの和の最小値」である配列 r</returns>
        /// <remarks>
        /// <para>計算量: O(N Sum(V))</para>
        /// <para>制約: <paramref name="wv"/>.W は非負である</para>
        /// </remarks>
        [凾(256)]
        public static T[] SmallValue<T>((T W, int V)[] wv) where T : INumber<T>, IMinMaxValue<T>
            => SmallValue(wv, T.MaxValue / (T.One + T.One));

        /// <summary>
        /// 重さ <paramref name="wv"/>[i].W, 価値 <paramref name="wv"/>[i].V の品物を、0個または1個選んだときの重さの和の最小値を返します。
        /// </summary>
        /// <returns>r[i] が「価値の和が i となるときの重さの和の最小値」である配列 r</returns>
        /// <remarks>
        /// <para>計算量: O(N Sum(V))</para>
        /// <para>制約: <paramref name="wv"/>.W は非負である</para>
        /// </remarks>
        public static T[] SmallValue<T>((T W, int V)[] wv, T INF) where T : INumber<T>
        {
            var rt = new T[wv.Sum(t => t.V) + 1];
            rt.AsSpan(1).Fill(INF);
            foreach (var (w, v) in wv)
            {
                for (int i = rt.Length - 1 - v; i >= 0; i--)
                {
                    ref var tv = ref rt[i + v];
                    var tx = rt[i] + w;
                    if (tx < tv) tv = tx;
                }
            }
            return rt;
        }
    }
}
