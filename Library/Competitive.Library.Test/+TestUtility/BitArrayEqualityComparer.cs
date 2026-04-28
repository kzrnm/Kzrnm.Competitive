using System.Collections;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.InteropServices;

namespace Kzrnm.Competitive.Testing;

public class BitArrayEqualityComparer : IEqualityComparer<BitArray>
{
    private BitArrayEqualityComparer() { }
    public static BitArrayEqualityComparer Default => new();
    public bool Equals(BitArray x, BitArray y) => x.SequenceEqual(y);
    public int GetHashCode([DisallowNull] BitArray obj)
    {
        var hc = new HashCode();
        hc.AddBytes(MemoryMarshal.AsBytes(obj.AsSpan()));
        return hc.ToHashCode();
    }
}