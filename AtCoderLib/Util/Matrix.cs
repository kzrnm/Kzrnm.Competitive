using static AtCoderProject.Global;


class Matrix
{
    public long[][] Pow(long[][] mat, int y)
    {
        var K = mat.Length;
        long[][] res = NewArray(K, K, 0L);
        for (var i = 0; i < res.Length; i++)
            res[i][i] = 1;
        for (; y > 0; y >>= 1)
        {
            if ((y & 1) == 1) res = Mul(res, mat);
            mat = Mul(mat, mat);
        }
        return res;
    }
    public long[][] Mul(long[][] l, long[][] r)
    {
        var K = l[0].Length;
        long[][] res = NewArray(K, K, 0L);
        for (var i = 0; i < res.Length; i++)
            for (var j = 0; j < res.Length; j++)
                for (var k = 0; k < res.Length; k++)
                    res[i][j] += l[i][k] * r[k][j];
        return res;
    }
}
