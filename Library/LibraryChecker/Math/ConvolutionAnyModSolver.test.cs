using Kzrnm.Competitive.IO;

namespace Kzrnm.Competitive.Solvers.DataStructure
{
    public class ConvolutionAnyModSolver
    {
        static void Main() => new ConvolutionAnyModSolver().Solve(new ConsoleReader(), new ConsoleWriter());
        // verification-helper: PROBLEM https://judge.yosupo.jp/problem/convolution_mod_1000000007
        public double TimeoutSecond => 10;
        public void Solve(ConsoleReader cr, ConsoleWriter cw)
        {
            int N = cr;
            int M = cr;
            uint[] arr = cr.Repeat(N);
            uint[] brr = cr.Repeat(M);
            cw.WriteLineJoin(ConvolutionAnyMod.Convolution(arr, brr, 1000000007));
        }
    }
}
