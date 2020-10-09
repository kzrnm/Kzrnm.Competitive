using System.Diagnostics;
using static AtCoder.Global;

namespace AtCoder.Util
{
    public static class Matrix
    {
        public static T[][] Pow<T, TOp>(T[][] mat, int y)
            where T : struct
            where TOp : struct, INumOperator<T>
        {
            Debug.Assert(mat.Length == mat[0].Length);
            var K = mat.Length;
            T[][] res = NewArray(K, K, default(T));
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
            Debug.Assert(l[0].Length == r.Length);
            T[][] res = NewArray(l.Length, r[0].Length, default(T));
            for (var i = 0; i < res.Length; i++)
                for (var j = 0; j < res[i].Length; j++)
                    for (var k = 0; k < r.Length; k++)
                        res[i][j] = op.Add(res[i][j], op.Multiply(l[i][k], r[k][j]));
            return res;
        }
    }

}
