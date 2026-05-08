
namespace Kzrnm.Competitive.Testing.Graph;

public class 最短経路WarshallFloydTests
{
    [Test, MultipleAssertions]
    public async Task Int()
    {
        var gb = new WIntGraphBuilder(5, true);
        gb.Add(0, 1, 1);
        gb.Add(1, 1, 1);
        gb.Add(0, 2, 10);
        gb.Add(0, 3, 30);
        gb.Add(0, 4, 40);
        gb.Add(1, 2, 5);
        gb.Add(2, 3, 605);
        gb.Add(2, 4, 6);
        gb.Add(4, 3, 6);
        gb.Add(4, 0, 1);
        var res = gb.ToGraph().WarshallFloyd();
        await res[0].Should().BeStrictlyEquivalentTo([0, 1, 6, 18, 12]);
        await res[1].Should().BeStrictlyEquivalentTo([12, 0, 5, 17, 11]);
        await res[2].Should().BeStrictlyEquivalentTo([7, 8, 0, 12, 6]);
        await res[3].Should().BeStrictlyEquivalentTo([1073741823, 1073741823, 1073741823, 0, 1073741823]);
        await res[4].Should().BeStrictlyEquivalentTo([1, 2, 7, 6, 0]);
    }

    [Test, MultipleAssertions]
    public async Task Long()
    {
        var gb = new WLongGraphBuilder(5, true);
        gb.Add(0, 1, 1);
        gb.Add(0, 2, 10);
        gb.Add(0, 3, 30);
        gb.Add(0, 4, 40);
        gb.Add(1, 2, 5);
        gb.Add(2, 3, 605);
        gb.Add(2, 4, 6);
        gb.Add(4, 3, 6);
        gb.Add(4, 0, 1);
        var res = gb.ToGraph().WarshallFloyd();
        await res[0].Should().BeStrictlyEquivalentTo([0L, 1, 6, 18, 12]);
        await res[1].Should().BeStrictlyEquivalentTo([12L, 0, 5, 17, 11]);
        await res[2].Should().BeStrictlyEquivalentTo([7L, 8, 0, 12, 6]);
        await res[3].Should().BeStrictlyEquivalentTo([4611686018427387903L, 4611686018427387903, 4611686018427387903, 0, 4611686018427387903]);
        await res[4].Should().BeStrictlyEquivalentTo([1L, 2, 7, 6, 0]);
    }
}