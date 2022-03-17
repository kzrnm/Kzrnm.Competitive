// verification-helper: IGNORE
using Kzrnm.Competitive.IO;
using Kzrnm.Competitive.LibraryChecker;
using System.Linq;
using System.Reflection;

namespace Kzrnm.Competitive
{
    class Program
    {
        static void Main()
        {
            var solvers = CompetitiveSolvers.GetSolvers(Assembly.GetExecutingAssembly())
                .OfType<Solver>()
                .ToDictionary(d => d.Name);

            var cr = new ConsoleReader();
            using var cw = new ConsoleWriter();

            cw.WriteLine("input problem name:").Flush();
            Solver solver;
            while (!solvers.TryGetValue(cr.String(), out solver))
            {
                cw.WriteLine("problem not found.")
                    .WriteLine("input problem name:").Flush();
            }

            solver.Solve(cr, cw);
        }
    }
}
