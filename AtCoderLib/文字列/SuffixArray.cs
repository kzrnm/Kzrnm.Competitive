using System;
using System.Linq;

class SuffixArray
{
#pragma warning disable IDE0059,IDE0060
    int[] S;
    int N;
    int[] SA;
    int[] rank;
    SparseTableRMQ rmq;
    public SuffixArray(string str)
    {
        N = str.Length;
        S = new int[N + 1];
        for (int i = 0; i < N; i++)
            S[i] = str[i];
        SA = Sais(S, S.Max());
        rank = new int[N + 1];
        for (int i = 0; i <= N; i++) rank[SA[i]] = i;
    }

    void CreateBeginBucket(int[] v, int[] b)
    {
        for (int i = 0; i < b.Length; i++) b[i] = 0;
        for (int i = 0; i < v.Length; i++) b[v[i]]++;
        int sum = 0;
        for (int i = 0; i < b.Length; i++) { b[i] += sum; var tmp = b[i]; b[i] = sum; sum = tmp; }
    }
    void CreateEndBucket(int[] v, int[] b)
    {
        for (int i = 0; i < b.Length; i++) b[i] = 0;
        for (int i = 0; i < v.Length; i++) b[v[i]]++;
        for (int i = 1; i < b.Length; i++) b[i] += b[i - 1];
    }
    void InducedSort(int[] v, int[] sa, int mv, int[] b, bool[] isl)
    {
        CreateBeginBucket(v, b);
        for (int i = 0; i < v.Length; i++) if (sa[i] > 0 && isl[sa[i] - 1]) sa[b[v[sa[i] - 1]]++] = sa[i] - 1;
    }
    void InvertInducedSort(int[] v, int[] sa, int mv, int[] b, bool[] isl)
    {
        CreateEndBucket(v, b);
        for (int i = v.Length - 1; i >= 0; i--)
            if (sa[i] > 0 && !isl[sa[i] - 1]) sa[--b[v[sa[i] - 1]]] = sa[i] - 1;
    }
    int[] Sais(int[] v, int mv)
    {
        if (v.Length == 1) return new int[] { 0 };
        var isl = new bool[v.Length];
        var b = new int[mv + 1];
        var sa = new int[v.Length];
        for (int i = 0; i < v.Length; i++)
            sa[i] = -1;
        for (int i = v.Length - 2; i >= 0; i--)
            isl[i] = v[i] > v[i + 1] || (v[i] == v[i + 1] && isl[i + 1]);
        CreateEndBucket(v, b);
        for (int i = 0; i < v.Length; i++) if (IsLMS(i, isl)) sa[--b[v[i]]] = i;
        InducedSort(v, sa, mv, b, isl);
        InvertInducedSort(v, sa, mv, b, isl);


        var cur = 0;
        var ord = new int[v.Length];
        for (int i = 0; i < v.Length; i++) if (IsLMS(i, isl)) ord[i] = cur++;
        var next = new int[cur];
        cur = -1;
        int prev = -1;
        for (int i = 0; i < v.Length; i++)
        {
            if (!IsLMS(sa[i], isl)) continue;
            var diff = false;
            for (int d = 0; d < v.Length; d++)
            {
                if (prev == -1 || v[sa[i] + d] != v[prev + d] || isl[sa[i] + d] != isl[prev + d])
                {
                    diff = true; break;
                }
                else if (d > 0 && IsLMS(sa[i] + d, isl)) break;
            }
            if (diff) { cur++; prev = sa[i]; }
            next[ord[sa[i]]] = cur;
        }
        var reord = new int[next.Length];
        for (int i = 0; i < v.Length; i++) if (IsLMS(i, isl)) reord[ord[i]] = i;
        var nextsa = Sais(next, cur);
        CreateEndBucket(v, b);
        for (int i = 0; i < sa.Length; i++) sa[i] = -1;
        for (int i = nextsa.Length - 1; i >= 0; i--) sa[--b[v[reord[nextsa[i]]]]] = reord[nextsa[i]];
        InducedSort(v, sa, mv, b, isl);
        InvertInducedSort(v, sa, mv, b, isl);
        return sa;
    }
    bool IsLMS(int x, bool[] isl) { return x > 0 && isl[x - 1] && !isl[x]; }

    public void BuildLCP()
    {
        var k = 0;
        var h = new int[N];
        for (int i = 0; i < N; i++)
        {
            var j = SA[rank[i] - 1];
            if (k > 0) k--;
            for (; j + k < N && i + k < N; k++) if (S[j + k] != S[i + k]) break;
            h[rank[i] - 1] = k;
        }
        rmq = new SparseTableRMQ(h);
    }
    /// <summary>
    /// s[<paramref name="i"/>:] と s[<paramref name="j"/>:] の最大共通接頭辞を O(loglogN) で計算します。
    /// </summary>
    /// <returns></returns>
    public int GetLCP(int i, int j)
    {
        i = rank[i]; j = rank[j];
        return rmq.Query(Math.Min(i, j), Math.Max(i, j));
    }
    /// <summary>
    /// rankがiのものを返す
    /// </summary>
    public int this[int index] => index == 0 ? N : SA[index - 1];

    /// <summary>
    /// s[i:]のランクを返す
    /// </summary>
    public int Rank(int index) => rank[index];

    #region SparseTableRMQ
    public class SparseTableRMQ
    {
        int n;
        int[] A;
        public SparseTableRMQ(int[] a)
        {
            var k = 1;
            n = a.Length;
            for (int i = 1; i < n; i <<= 1) k++;

            A = new int[n * k];
            for (int i = 0; i < n; i++)
                A[i] = a[i];
            var d = 0;
            for (int i = 1; i < n; i <<= 1, d += n)
            {
                for (int j = 0; j < n; j++)
                    A[d + n + j] = A[d + j];
                for (int j = 0; j < n - i; j++)
                    A[d + n + j] = Math.Min(A[d + j], A[d + j + i]);
            }
        }
        /// <summary>
        /// value of [l,r)
        /// </summary>
        public int Query(int l, int r)
        {
            r--;
            int z = r - l, k = 0, e = 1, s;
            s = ((z & 0xffff0000) != 0 ? 1 : 0) << 4; z >>= s; e <<= s; k |= s;
            s = ((z & 0x0000ff00) != 0 ? 1 : 0) << 3; z >>= s; e <<= s; k |= s;
            s = ((z & 0x000000f0) != 0 ? 1 : 0) << 2; z >>= s; e <<= s; k |= s;
            s = ((z & 0x0000000c) != 0 ? 1 : 0) << 1; z >>= s; e <<= s; k |= s;
            s = ((z & 0x00000002) != 0 ? 1 : 0) << 0; z >>= s; e <<= s; k |= s;
            //var vl = A[l + n * k];
            //var vr = A[r - e + 1 + (n * k)];
            return Math.Min(A[l + (n * k)], A[r + (n * k) - e + 1]);
        }
    }
    #endregion
}