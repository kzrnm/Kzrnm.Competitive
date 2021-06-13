using Kzrnm.Competitive.IO;

namespace Kzrnm.Competitive.Solvers.DataStructure
{
    public class BinaryTrieSolver : ISolver
    {
        public string Name => "set_xor_min";

        public void Solve(ConsoleReader cr, ConsoleWriter cw)
        {
            int N = cr;
            var bt = new BinaryTrie(30);
            for (int q = 0; q < N; q++)
            {
                int t = cr;
                uint x = cr;
                if (t == 0)
                {
                    if (bt.Count(x) == 0) bt.Add(x);
                }
                else if (t == 1)
                {
                    if (bt.Count(x) != 0) bt.Remove(x);
                }
                else
                    cw.WriteLine(bt.MinElement(x).Num);
            }
        }
    }
}
