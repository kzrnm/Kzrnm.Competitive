using AtCoder;

namespace Kzrnm.Competitive.Testing.Extensions;

public class MemoryMarshalExtensionTests
{
    [Test]
    public async Task ListAsSpan()
    {
        await new List<long> {
            43,24,8373,4,98,7,43,28,9470,71,431,45,23014,345,23614,1503,7,3401434,120,42314,3123
        }.AsSpan()[18..].ToArray().Should().BeStrictlyEquivalentTo([120L, 42314, 3123]);
    }

    struct DId;
    [Test, MultipleAssertions]
    public async Task Cast()
    {
        {
            var array = new long[4]
            {
                -1L, 0b110,long.MaxValue,long.MinValue
            };
            var expected = new uint[8] {
                4294967295u, 4294967295u, 6u, 0u, 4294967295u, 2147483647u, 0u, 2147483648u
            };
            await ((Span<long>)array).Cast<long, uint>().ToArray().Should().BeStrictlyEquivalentTo(expected);
            await ((ReadOnlySpan<long>)array).Cast<long, uint>().ToArray().Should().BeStrictlyEquivalentTo(expected);
        }
        {
            var array = new StaticModInt<Mod1000000007>[4]
            {
                -1,1000000007,1,2,
            };
            var expected = new uint[4] {
                1000000006,0,1,2
            };
            await ((Span<StaticModInt<Mod1000000007>>)array).Cast<StaticModInt<Mod1000000007>, uint>().ToArray().Should().BeStrictlyEquivalentTo(expected);
            await ((ReadOnlySpan<StaticModInt<Mod1000000007>>)array).Cast<StaticModInt<Mod1000000007>, uint>().ToArray().Should().BeStrictlyEquivalentTo(expected);
        }
        {
            DynamicModInt<DId>.Mod = 1000000007;
            var array = new DynamicModInt<DId>[4]
            {
                -1,1000000007,1,2,
            };
            var expected = new uint[4] {
                1000000006,0,1,2
            };
            await ((Span<DynamicModInt<DId>>)array).Cast<DynamicModInt<DId>, uint>().ToArray().Should().BeStrictlyEquivalentTo(expected);
            await ((ReadOnlySpan<DynamicModInt<DId>>)array).Cast<DynamicModInt<DId>, uint>().ToArray().Should().BeStrictlyEquivalentTo(expected);
        }
    }

    [Test, MultipleAssertions]
    public async Task GetReference()
    {
        {
            var array = new long[4]
            {
                0,1,2,3
            };
            ((Span<long>)array).GetReference() = -1;
            await array.Should().BeStrictlyEquivalentTo([-1L, 1, 2, 3]);
            ((ReadOnlySpan<long>)array).GetReference() = -2;
            await array.Should().BeStrictlyEquivalentTo([-2L, 1, 2, 3]);
        }
    }
}