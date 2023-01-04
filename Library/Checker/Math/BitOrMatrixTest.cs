using Kzrnm.Competitive.IO;
using System.Collections;

namespace Kzrnm.Competitive.MathNs
{
    internal class BitOrMatrixTest : BaseSolver
    {
        public override string Url => "https://yukicoder.me/problems/no/1340";
        public override ConsoleOutput? Solve(ConsoleReader cr, Utf8ConsoleWriter cw)
        {
            int N = cr;
            int M = cr;
            long T = cr;
            var bits = Global.NewArray(N, () => new BitArray(N));
            for (int i = 0; i < M; i++)
            {
                int a = cr;
                int b = cr;
                bits[a][b] = true;
            }
            return new BitOrMatrix(bits).Pow(T).Value switch
            {
                null => 1,
                var v => v[0].PopCount(),
            };
        }
    }
}
