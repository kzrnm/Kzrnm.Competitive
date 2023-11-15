using AtCoder.Internal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Runtime.CompilerServices;
using 凾 = System.Runtime.CompilerServices.MethodImplAttribute;

namespace Kzrnm.Competitive.Internal
{
    /// <summary>
    /// 行列式とかガウスの消去法とか
    /// </summary>
    public static class ArrayMatrixLogic<T>
        where T : INumberBase<T>
    {
        /// <summary>
        /// <paramref name="r"/> より下で <paramref name="x"/> 列が 0 ではない行を探して、<paramref name="r"/> 行に置く。
        /// </summary>
        /// <returns>0 ではない行が見つかったかどうか</returns>
        [凾(256)]
        public static bool SearchNonZero(T[][] mat, int r, int x)
        {
            for (int y = r; y < mat.Length; y++)
                if (!EqualityComparer<T>.Default.Equals(mat[y][x], default))
                {
                    (mat[r], mat[y]) = (mat[y], mat[r]);
                    return true;
                }
            return false;
        }

        [凾(256)]
        public static T DeterminantImpl(T[][] arr)
        {
            if (arr.Length == 0) return T.MultiplicativeIdentity;
            Contract.Assert(arr.Length == arr[0].Length);
            bool swap = false;
            //上三角行列
            for (int i = 0; i < arr.Length; i++)
            {
                if (EqualityComparer<T>.Default.Equals(arr[i][i], default))
                {
                    if (!SearchNonZero(arr, i, i))
                        return default;
                    swap = !swap;
                }
                var t = arr[i][i];
                for (int j = i + 1; j < arr.Length; j++)
                {
                    var c = arr[j][i] / t;
                    for (int k = 0; k < arr[i].Length; k++)
                        arr[j][k] -= c * arr[i][k];
                }
            }

            T det = T.MultiplicativeIdentity;
            if (swap)
                det = -det;
            for (int i = 0; i < arr.Length; i++)
                det *= arr[i][i];
            return det;
        }

        /// <summary>
        /// 大きさ <paramref name="s"/> の正方行列の <c>(<paramref name="i"/>, <paramref name="j"/>)</c> 余因子を求めます。
        /// </summary>
        [凾(256)]
        public static T Cofactor(ReadOnlySpan<T> v, int s, int i, int j)
        {
            Contract.Assert(v.Length == s * s);
            var arr = new T[s - 1][];
            for (int k = 0; k < arr.Length; k++)
            {
                var p = k;
                if (k >= i) ++p;
                var row = v.Slice(p * s, s);
                arr[k] = new T[s - 1];
                row[..j].CopyTo(arr[k]);
                row[(j + 1)..].CopyTo(arr[k].AsSpan(j));
            }
            var rt = DeterminantImpl(arr);
            if (((i ^ j) & 1) != 0)
                rt = -rt;
            return rt;
        }


        /// <summary>
        /// 転置行列を取得します。
        /// </summary>
        public static T[] Transpose(ReadOnlySpan<T> v, int h, int w)
        {
            var rt = new T[h * w];
            if (rt.Length == 0) return rt;

            ref var rp = ref rt[0];
            for (int hh = 0; hh < h; hh++)
            {
                var f = hh * w;
                var s = v.Slice(f, w);
                for (int ww = 0; ww < s.Length; ww++)
                {
                    Unsafe.Add(ref rp, ww * h + hh) = s[ww];
                }
            }
            return rt;
        }

        /// <summary>
        /// 大きさ <paramref name="s"/> の正方行列の逆行列を求めます。
        /// </summary>
        [凾(256)]
        public static T[][] Inv(ReadOnlySpan<T> v, int s)
        {
            Contract.Assert(v.Length == s * s);
            var len1 = s * 2;
            var arr = new T[s][];
            for (int i = 0; i < arr.Length; i++)
            {
                arr[i] = new T[len1];
                v.Slice(i * s, s).CopyTo(arr[i]);
                arr[i][s + i] = T.MultiplicativeIdentity;
            }
            GaussianEliminationImpl(arr, true);

            var ix = arr.Length - 1;
            if (EqualityComparer<T>.Default.Equals(arr[ix][ix], default))
                return null;

            var res = new T[arr.Length][];
            for (int i = 0; i < res.Length; i++)
                res[i] = arr[i][res.Length..];
            return res;
        }

        /// <summary>
        /// <para>ガウスの消去法(掃き出し法)</para>
        /// </summary>
        /// <param name="arr">対象の行列</param>
        /// <param name="isReduced">行標準形にするかどうか。false ならば上三角行列</param>
        /// <returns>(行列式, 0ではない列のインデックス)</returns>
        [凾(256)]
        public static List<int> GaussianEliminationImpl(T[][] arr, bool isReduced)
        {
            var idx = new List<int>(arr.Length);
            int r = 0;
            int width = arr[0].Length;
            for (int x = 0; x < width && r < arr.Length; x++)
            {
                if (!SearchNonZero(arr, r, x))
                    continue;
                var arrR = arr[r];
                {
                    var inv = T.MultiplicativeIdentity / arrR[x];
                    arrR[x] = T.MultiplicativeIdentity;
                    for (int i = x + 1; i < arrR.Length; i++)
                        arrR[i] *= inv;
                }
                for (int y = isReduced ? 0 : r + 1; y < arr.Length; y++)
                {
                    var arrY = arr[y];
                    if (y == r || EqualityComparer<T>.Default.Equals(arrY[x], default))
                        continue;
                    var freq = arrY[x];
                    for (int k = x + 1; k < arrY.Length; k++)
                        arrY[k] -= freq * arrR[k];
                    arrY[x] = default;
                }
                ++r;
                idx.Add(x);
            }
            return idx;
        }

        /// <summary>
        /// 連立一次方程式 <see langword="this"/>・X=<paramref name="vector"/> を満たす ベクトル X を求めます。
        /// </summary>
        /// <returns>
        /// <list type="bullet">
        /// <item><description>最初の配列: 特殊解</description></item>
        /// <item><description>2番目以降の配列: 解空間の基底ベクトル</description></item>
        /// <item><description>ただし解無しのときは空配列を返す</description></item>
        /// </list>
        /// </returns>
        [凾(256)]
        public static T[][] LinearSystem(ReadOnlySpan<T> _v, int h, int w, T[] vector)
        {
            Contract.Assert(h == vector.Length);
            var impl = new T[h][];
            for (int i = 0; i < impl.Length; i++)
            {
                impl[i] = new T[w + 1];
                _v.Slice(i * w, w).CopyTo(impl[i]);
                impl[i][^1] = vector[i];
            }
            var idxs = GaussianEliminationImpl(impl, false).AsSpan();
            var r = idxs.Length;

            // 解があるかチェック
            // a×0+b×0+c×0..+z×0≠0 になっていたら解無し
            for (int i = r; i < impl.Length; i++)
            {
                if (!EqualityComparer<T>.Default.Equals(impl[i][^1], default))
                    return Array.Empty<T[]>();
            }
            if (idxs.IsEmpty)
            {
                var eres = new T[w + 1][];
                eres[0] = Enumerable.Repeat(T.AdditiveIdentity, w).ToArray();
                for (int i = 1; i < eres.Length; i++)
                {
                    eres[i] = Enumerable.Repeat(T.AdditiveIdentity, w).ToArray();
                    eres[i][i - 1] = T.MultiplicativeIdentity;
                }
                return eres;
            }
            if (idxs[^1] == w)
                return Array.Empty<T[]>();

            var used = new HashSet<int>(Enumerable.Range(0, w));
            var lst = new List<T[]>(w);
            {
                var v = new T[w];
                for (int y = idxs.Length - 1; y >= 0; y--)
                {
                    int f = idxs[y];
                    used.Remove(f);
                    v[f] = impl[y][^1];
                    for (int x = f + 1; x < w; x++)
                        v[f] -= impl[y][x] * v[x];
                    v[f] /= impl[y][f];
                }
                lst.Add(v);
            }

            foreach (var s in used)
            {
                var v = new T[w];
                v[s] = T.MultiplicativeIdentity;
                for (int y = idxs.Length - 1; y >= 0; y--)
                {
                    int f = idxs[y];
                    for (int x = f + 1; x < w; x++)
                        v[f] -= impl[y][x] * v[x];
                    v[f] /= impl[y][f];
                }
                lst.Add(v);
            }
            return lst.ToArray();
        }

    }
}