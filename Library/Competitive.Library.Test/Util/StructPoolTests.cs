
using Kzrnm.Competitive.Internal;
using System.Runtime.CompilerServices;

namespace Kzrnm.Competitive.Testing.Util;

public class StructPoolTests
{
    [Test]
    public async Task Usage()
    {
        var pool = GetStructPool();
        Viewer(pool).Array.AsSpan().Clear();
        using (Assert.Multiple())
        {
            await Viewer(pool).Array.Should().BeStrictlyEquivalentTo([0, 0], EqualityComparer<long>.Default);
            await Viewer(pool).Stack.Should().BeStrictlyEquivalentTo([0, 1], EqualityComparer<int>.Default);
            await Viewer(pool).StackSize.Should().BeEqualTo(2);
        }

        int ix;
        pool.Rent(out ix);
        await ix.Should().BeEqualTo(1);
        pool.Get(1) = 1;
        using (Assert.Multiple())
        {
            await Viewer(pool).Array.Should().BeStrictlyEquivalentTo([0, 1], EqualityComparer<long>.Default);
            await Viewer(pool).Stack.Should().BeStrictlyEquivalentTo([0, 1], EqualityComparer<int>.Default);
            await Viewer(pool).StackSize.Should().BeEqualTo(1);
        }

        pool.Rent(out ix);
        await ix.Should().BeEqualTo(0);
        pool.Get(0) = 2;
        using (Assert.Multiple())
        {
            await Viewer(pool).Array.Should().BeStrictlyEquivalentTo([2, 1], EqualityComparer<long>.Default);
            await Viewer(pool).Stack.Should().BeStrictlyEquivalentTo([0, 1], EqualityComparer<int>.Default);
            await Viewer(pool).StackSize.Should().BeEqualTo(0);
        }

        pool.Return(1);
        using (Assert.Multiple())
        {
            await Viewer(pool).Array.Should().BeStrictlyEquivalentTo([2, 1], EqualityComparer<long>.Default);
            await Viewer(pool).Stack.Should().BeStrictlyEquivalentTo([1, 1], EqualityComparer<int>.Default);
            await Viewer(pool).StackSize.Should().BeEqualTo(1);
        }

        pool.Rent(out ix);
        await ix.Should().BeEqualTo(1);
        using (Assert.Multiple())
        {
            await Viewer(pool).Array.Should().BeStrictlyEquivalentTo([2, 1], EqualityComparer<long>.Default);
            await Viewer(pool).Stack.Should().BeStrictlyEquivalentTo([1, 1], EqualityComparer<int>.Default);
            await Viewer(pool).StackSize.Should().BeEqualTo(0);
        }

        pool.Rent(out ix);
        await ix.Should().BeEqualTo(3);
        Viewer(pool).Array.AsSpan(2).Clear();
        pool.Get(3) = 3;
        using (Assert.Multiple())
        {
            await Viewer(pool).Array.Should().BeStrictlyEquivalentTo([2, 1, 0, 3], EqualityComparer<long>.Default);
            await Viewer(pool).Stack.Should().BeStrictlyEquivalentTo([2, 3, 0, 0], EqualityComparer<int>.Default);
            await Viewer(pool).StackSize.Should().BeEqualTo(1);
        }

        pool.Return(1);
        using (Assert.Multiple())
        {
            await Viewer(pool).Array.Should().BeStrictlyEquivalentTo([2, 1, 0, 3], EqualityComparer<long>.Default);
            await Viewer(pool).Stack.Should().BeStrictlyEquivalentTo([2, 1, 0, 0], EqualityComparer<int>.Default);
            await Viewer(pool).StackSize.Should().BeEqualTo(2);
        }
    }

    static StructPoolViewer Viewer(StructPool<long> pool) => new(pool);
    static StructPool<long> GetStructPool() => StructPoolAccessor<long>.Get(2);
    ref struct StructPoolViewer
    {
        public StructPoolViewer(StructPool<long> pool)
        {
            Array = ref StructPoolAccessor<long>.Array(pool);
            Stack = ref StructPoolAccessor<long>.Stack(pool);
            StackSize = ref StructPoolAccessor<long>.StackSize(pool);
        }
        public ref long[] Array;
        public ref int[] Stack;
        public ref int StackSize;
    }
    static class StructPoolAccessor<T> where T : struct
    {
        [UnsafeAccessor(UnsafeAccessorKind.Constructor)]
        public static extern StructPool<T> Get(int size);

        [UnsafeAccessor(UnsafeAccessorKind.Field, Name = "_a")]
        public static extern ref T[] Array(StructPool<T> pool);
        [UnsafeAccessor(UnsafeAccessorKind.Field, Name = "_s")]
        public static extern ref int[] Stack(StructPool<T> pool);
        [UnsafeAccessor(UnsafeAccessorKind.Field, Name = "_si")]
        public static extern ref int StackSize(StructPool<T> pool);
    }
}