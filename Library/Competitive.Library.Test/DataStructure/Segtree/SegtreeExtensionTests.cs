using AtCoder;

namespace Kzrnm.Competitive.Testing.DataStructure;

public class SegtreeExtensionTests
{
    [Test, MultipleAssertions]
    public async Task ToArray()
    {
        for (int i = 0; i < 20; i++)
        {
            var seg = new Segtree<int, Op>(i);
            for (int j = 0; j < i; j++)
                seg[j] = j;
            await seg.ToArray().Should().BeStrictlyEquivalentTo(Enumerable.Range(0, i));
        }
    }
    readonly struct Op : ISegtreeOperator<int>
    {
        public int Identity => 0;
        public int Operate(int x, int y) => Math.Max(x, y);
    }
}