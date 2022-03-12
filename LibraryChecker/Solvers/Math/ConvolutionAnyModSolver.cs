using Kzrnm.Competitive.IO;

namespace Kzrnm.Competitive.Solvers.DataStructure
{
    public class ConvolutionAnyModSolver : Solver
    {
        public override string Name => "convolution_mod_1000000007";
        public override double TimeoutSecond => 10;
        public override void Solve(ConsoleReader cr, ConsoleWriter cw)
        {
            int N = cr;
            int M = cr;
            uint[] arr = cr.Repeat(N);
            uint[] brr = cr.Repeat(M);
            cw.WriteLineJoin(ConvolutionAnyMod.Convolution(arr, brr, 1000000007));
        }
    }
}
