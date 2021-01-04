using AtCoder.Internal;
using System.Collections.Generic;

namespace AtCoder
{
    public static class Matrix
    {
        public static T[][] Pow<T, TOp>(T[][] mat, long y)
            where T : struct
            where TOp : struct, IArithmeticOperator<T>
        {
            DebugUtil.Assert(mat.Length == mat[0].Length);
            var K = mat.Length;
            T[][] res = Global.NewArray(K, K, default(T));
            var one = default(TOp).Increment(default);
            for (var i = 0; i < res.Length; i++)
                res[i][i] = one;
            for (; y > 0; y >>= 1)
            {
                if ((y & 1) == 1) res = Mul<T, TOp>(res, mat);
                mat = Mul<T, TOp>(mat, mat);
            }
            return res;
        }
        public static T[][] Mul<T, TOp>(T[][] l, T[][] r)
            where T : struct
            where TOp : struct, IArithmeticOperator<T>
        {
            var op = default(TOp);
            DebugUtil.Assert(l[0].Length == r.Length);
            T[][] res = Global.NewArray(l.Length, r[0].Length, default(T));
            for (var i = 0; i < res.Length; i++)
                for (var j = 0; j < res[i].Length; j++)
                    for (var k = 0; k < r.Length; k++)
                        res[i][j] = op.Add(res[i][j], op.Multiply(l[i][k], r[k][j]));
            return res;
        }

        /// <summary>
        /// ガウスの消去法(掃き出し法)で一次方程式を解く。<paramref name="mat"/>に破壊的変更を加える。
        /// </summary>
        public static T[][] GaussianElimination<T, TOp>(T[][] mat)
            where T : struct
            where TOp : struct, IArithmeticOperator<T>
        {
            static bool SearchNonZero(T[][] mat, int i)
            {
                for (int j = i + 1; j < mat.Length; j++)
                    if (!EqualityComparer<T>.Default.Equals(mat[j][i], default))
                    {
                        (mat[i], mat[j]) = (mat[j], mat[i]);
                        return true;
                    }
                return false;
            }
            var op = default(TOp);
            DebugUtil.Assert(mat[0].Length == mat.Length + 1);
            for (int i = 0; i < mat.Length; i++)
            {
                if (EqualityComparer<T>.Default.Equals(mat[i][i], default))
                {
                    if (!SearchNonZero(mat, i))
                        continue;
                }
                var inv = op.Divide(op.Increment(default), mat[i][i]);

                for (int k = i; k < mat[i].Length; k++)
                    mat[i][k] = op.Multiply(mat[i][k], inv);

                for (int j = i + 1; j < mat.Length; j++)
                {
                    for (int k = i + 1; k < mat[j].Length; k++)
                        mat[j][k] = op.Subtract(mat[j][k], op.Multiply(mat[i][k], mat[j][i]));
                    mat[j][i] = default;
                }
            }
            return mat;
        }
    }

}
