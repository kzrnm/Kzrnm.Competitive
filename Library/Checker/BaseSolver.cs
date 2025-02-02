using Kzrnm.Competitive.IO;

namespace Kzrnm.Competitive;

internal abstract class BaseSolver : CompetitiveVerifier.ProblemSolver
{
    public override void Solve()
    {
        using var cw = ConsoleOutput.cw = new Utf8ConsoleWriter();
        Solve(new ConsoleReader(), cw);
    }
    public abstract ConsoleOutput? Solve(ConsoleReader cr, Utf8ConsoleWriter cw);
}
