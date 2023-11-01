using System.Diagnostics.Contracts;
using System.Numerics;
using 凾 = System.Runtime.CompilerServices.MethodImplAttribute;

namespace Kzrnm.Competitive
{
    // https://nyaannyaan.github.io/library/matrix/characteristric-polynomial.hpp
    public static class __Matrix_CharacteristricPolynomial
    {
        /// <summary>
        /// <paramref name="mt"/> の特性多項式の係数を返します。
        /// </summary>
        /// <param name="mt">行列</param>
        /// <param name="normalize">false で <paramref name="mt"/> の次数が奇数なら正負が反転したまま返します。</param>
        [凾(256)]
        public static T[] CharacteristricPolynomial<T>(this ArrayMatrix<T> mt, bool normalize = true)
            where T : INumberBase<T>
        {
            Contract.Assert(mt.Height == mt.Width);
            var a = mt.ToArray();
            int n = mt.Height;


            /*
             * 表記を楽にするため符号を逆にしている。λ - a00 を a00 - λ とする
             * 
             * 
             * 対角成分 akk は λ が入るので掛け算で使いたくない
             * 
             * a00 - λ, a01    , ..., a0n
             *     a10, a11 - λ, ..., a1n
             *     .................
             *     .................
             *     an0, ............, ann - λ
             * 
             */

            // 上三角行列っぽいものを作る
            for (int j = 0; j + 2 < n; j++)
            {
                // a10 に 0 以外の数を入れたい
                if (T.IsZero(a[j + 1][j]))
                    for (int i = j + 2; i < a.Length; i++)
                    {
                        if (!T.IsZero(a[i][j]))
                        {
                            /*
                             * 行を入れ替える
                             * 対角成分以外に λ が現れる
                             * a00 - λ,     a01, ................., a0n
                             *     ai0,     ai1, ..., aii - λ, ..., ain
                             *          ..................... 
                             *     a10, a11 - λ, ................., a1n
                             *          ..................... 
                             *     an0, .........................., ann - λ
                             * 
                             */
                            (a[j + 1], a[i]) = (a[i], a[j + 1]);


                            /*
                             * λ が現れるのが対角成分になるように列を入れ替える
                             * λ が対角成分だけになる
                             * a00 - λ,     a0i,   ...         ..., a0n
                             *     ai0, aii - λ,   ...         ..., ain
                             *          ..........................
                             *     a10,     a1i, ..., a11 - λ, ..., a1n
                             *          .......................... 
                             *     an0, .........................., ann - λ
                             * 
                             */
                            for (int k = 0; k < a.Length; k++)
                                (a[k][j + 1], a[k][i]) = (a[k][i], a[k][j + 1]);
                            break;
                        }
                    }

                /*
                 * 対角成分の一つ下で調整
                 * a00 - λ,     a01, ..., a0n
                 *     a10, a11 - λ, ..., a1n
                 *      ................... 
                 *      ................... 
                 *     an0, ............, ann - λ
                 * 
                 */
                if (!T.IsZero(a[j + 1][j]))
                {
                    var inv = T.One / a[j + 1][j];
                    for (int i = j + 2; i < a.Length; i++)
                    {
                        if (T.IsZero(a[i][j])) continue;
                        var c = inv * a[i][j];


                        /*
                         * 対角成分の一つ下で調整
                         * a00 - λ,     a01,     a02, ..., a0n
                         *     a10, a11 - λ,     a12, ..., a1n
                         *     a20,     a21, a22 - λ, ..., a1n
                         * ............ 
                         * an0, ............, ann - λ
                         * 
                         * ↓
                         * 
                         *         a00 - λ,                   a01,                   a02, ..., a0n
                         *             a10,               a11 - λ,                   a12, ..., a1n
                         * a20-a10*a20/a10, a21-(a11 - λ)*a20/a10, a22 - λ - a12*a20/a10, ..., a1n - a1n*a20/a10
                         * ............ 
                         * an0-a10*an0/a10, an1-(a11 - λ)*an0/a10, an2     - a12*an0/a10, ..., ann - λ - a1n*an0/a10
                         * 
                         * ↓
                         * 
                         *         a00 - λ,               a01    ,                   a02, ..., a0n
                         *             a10,               a11 - λ,                   a12, ..., a1n
                         *               0, a21-(a11 - λ)*a20/a10, a22 - λ - a12*a20/a10, ..., a1n - a1n*a20/a10
                         * ............ 
                         *               0, an1-(a11 - λ)*an0/a10, an2     - a12*an0/a10, ..., ann - λ - a1n*an0/a10
                         * 
                         */
                        a[i][j] = T.Zero;
                        for (int l = j + 1; l < n; l++)
                            a[i][l] -= c * a[j + 1][l];


                        /*
                         * 上記のように j+1 列に λ が出てきているので対角成分を足して打ち消す
                         * 
                         *  j+1 列: an1-(a11 - λ)*an0/a10
                         * 対角成分: ann - λ - a1n*an0/a10
                         *      c = an0/a10
                         *  an1-(a11 - λ)*c  +  (ann - λ - a1n*an0/a10)*c
                         *  ↓
                         *  an1 + (ann - a1n*an0/a10 - a11)*c
                         */
                        for (int k = 0; k < a.Length; k++)
                            a[k][j + 1] += c * a[k][i];
                    }
                }
            }
            /*
             * 上三角行列っぽくなった
             * 
             * a00 - λ, a01    , a02    , ..., a0n
             *     a10, a11 - λ, a12    , ..., a1n
             *       0,       0, a22 - λ, ..., a1n
             *     .................
             *     .................
             *     　0, ............, ann - λ
             * 
             */

            var p = new T[n + 1][];
            p[0] = new T[1] { T.One };
            for (int i = 1; i < p.Length; i++)
            {
                var pp = p[i] = new T[i + 1];
                for (int j = 0; j < i; j++)
                {
                    pp[j + 1] -= p[i - 1][j];
                    pp[j] += p[i - 1][j] * a[i - 1][i - 1];
                }
                var x = T.One;
                for (int m = 1; m < i; m++)
                {
                    x *= -a[i - m][i - m - 1];
                    var c = x * a[i - m - 1][i - 1];
                    for (int j = 0; j < i - m; j++)
                        pp[j] += c * p[i - m - 1][j];
                }
            }

            var s = p[^1];

            if (normalize && (n & 1) != 0)
                for (int i = 0; i < s.Length; i++)
                    s[i] = -s[i];

            return s;
        }
    }
}
