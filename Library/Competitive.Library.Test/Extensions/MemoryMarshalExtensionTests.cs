using AtCoder;
using System;
using System.Collections.Generic;

namespace Kzrnm.Competitive.Testing.Extensions;

public class MemoryMarshalExtensionTests
{
    [Fact]
    public void ListAsSpan()
    {
        new List<long> {
            43,24,8373,4,98,7,43,28,9470,71,431,45,23014,345,23614,1503,7,3401434,120,42314,3123
        }.AsSpan()[18..].ToArray().ShouldBe([120, 42314, 3123]);
    }

    [Fact]
    public void Cast()
    {
        {
            var array = new long[4]
            {
                -1L, 0b110,long.MaxValue,long.MinValue
            };
            var expected = new uint[8] {
                4294967295u, 4294967295u, 6u, 0u, 4294967295u, 2147483647u, 0u, 2147483648u
            };
            ((Span<long>)array).Cast<long, uint>().ToArray().ShouldBe(expected);
            ((ReadOnlySpan<long>)array).Cast<long, uint>().ToArray().ShouldBe(expected);
        }
        {
            var array = new StaticModInt<Mod1000000007>[4]
            {
                -1,1000000007,1,2,
            };
            var expected = new uint[4] {
                1000000006,0,1,2
            };
            ((Span<StaticModInt<Mod1000000007>>)array).Cast<StaticModInt<Mod1000000007>, uint>().ToArray().ShouldBe(expected);
            ((ReadOnlySpan<StaticModInt<Mod1000000007>>)array).Cast<StaticModInt<Mod1000000007>, uint>().ToArray().ShouldBe(expected);
        }
        {
            DynamicModInt<DynamicModIntId0>.Mod = 1000000007;
            var array = new DynamicModInt<DynamicModIntId0>[4]
            {
                -1,1000000007,1,2,
            };
            var expected = new uint[4] {
                1000000006,0,1,2
            };
            ((Span<DynamicModInt<DynamicModIntId0>>)array).Cast<DynamicModInt<DynamicModIntId0>, uint>().ToArray().ShouldBe(expected);
            ((ReadOnlySpan<DynamicModInt<DynamicModIntId0>>)array).Cast<DynamicModInt<DynamicModIntId0>, uint>().ToArray().ShouldBe(expected);
        }
    }

    [Fact]
    public void GetReference()
    {
        {
            var array = new long[4]
            {
                0,1,2,3
            };
            ((Span<long>)array).GetReference() = -1;
            array.ShouldBe([-1, 1, 2, 3]);
            ((ReadOnlySpan<long>)array).GetReference() = -2;
            array.ShouldBe([-2, 1, 2, 3]);
        }
    }
}