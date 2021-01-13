using Kzrnm.Competitive.IO;

namespace AtCoder.Solvers.DataStructure
{
    public class WaveletMatrixSolver : ISolver
    {
        public string Name => "range_kth_smallest";

        public void Solve(ConsoleReader cr, ConsoleWriter cw)
        {
            int n = cr;
            int q = cr;
            int[] a = cr.Repeat(n);
            var mat = new WaveletMatrix<int>(a);
            for (int i = 0; i < q; i++)
            {
                int l = cr;
                int r = cr;
                int k = cr;
                cw.WriteLine(mat.KthSmallest(l, r, k));
            }
        }
    }
}

