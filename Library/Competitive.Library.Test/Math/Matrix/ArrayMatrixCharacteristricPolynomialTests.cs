using AtCoder;
using System.Collections.Immutable;
using System.Numerics;
using System.Runtime.InteropServices;

namespace Kzrnm.Competitive.Testing.MathNS.Matrix;

using ModInt = StaticModInt<Mod1000000007>;
public class ArrayMatrixCharacteristricPolynomialTests
{
    public static IEnumerable<ImmutableArray<int>> Size3_Data()
    {
        var rnd = new Random(227);
        for (int q = 0; q < 100; q++)
        {
            var arr = new int[9];
            rnd.NextBytes(MemoryMarshal.AsBytes(arr.AsSpan()));
            yield return ImmutableArray.CreateRange(arr);
        }
        {
            var arr = new int[9];
            var span = arr.AsSpan();
            rnd.NextBytes(MemoryMarshal.AsBytes(arr.AsSpan()));
            span[0..3].CopyTo(span[3..]);
            yield return ImmutableArray.CreateRange(arr);
        }
    }

    [Test, MultipleAssertions]
    [MethodDataSource(nameof(Size3_Data))]
    public async Task Size3(ImmutableArray<int> v)
    {
        var a = new ModInt[]{
        v[3*0+0], v[3*0+1], v[3*0+2],
        v[3*1+0], v[3*1+1], v[3*1+2],
        v[3*2+0], v[3*2+1], v[3*2+2] };

        var expected = new PolynominalMatrix3x3<ModInt>(
            new([-v[3 * 0 + 0], 1]), new([-v[3 * 0 + 1]]), new([-v[3 * 0 + 2]]),
            new([-v[3 * 1 + 0]]), new([-v[3 * 1 + 1], 1]), new([-v[3 * 1 + 2]]),
            new([-v[3 * 2 + 0]]), new([-v[3 * 2 + 1]]), new([-v[3 * 2 + 2], 1]))
            .Determinant();
        await new ArrayMatrix<ModInt>(a, 3, 3).CharacteristricPolynomial().Should().BeEquivalentOrderTo(expected.Coefficients);
        await new ArrayMatrix<ModInt>(a, 3, 3).CharacteristricPolynomial(false).Should().BeEquivalentOrderTo((-expected).Coefficients);
    }

    public static IEnumerable<ImmutableArray<int>> Size4_Data()
    {
        var rnd = new Random(227);
        for (int q = 0; q < 100; q++)
        {
            var arr = new int[16];
            rnd.NextBytes(MemoryMarshal.AsBytes(arr.AsSpan()));
            yield return ImmutableArray.CreateRange(arr);
        }
        {
            var arr = new int[16];
            var span = arr.AsSpan();
            rnd.NextBytes(MemoryMarshal.AsBytes(arr.AsSpan()));
            span[0..4].CopyTo(span[4..]);
            yield return ImmutableArray.CreateRange(arr);
        }
    }

    [Test]
    [MethodDataSource(nameof(Size4_Data))]
    public async Task Size4(ImmutableArray<int> v)
    {
        var a = new ModInt[]{
        v[4*0+0], v[4*0+1], v[4*0+2], v[4*0+3],
        v[4*1+0], v[4*1+1], v[4*1+2], v[4*1+3],
        v[4*2+0], v[4*2+1], v[4*2+2], v[4*2+3],
        v[4*3+0], v[4*3+1], v[4*3+2], v[4*3+3] };
        var expected = new PolynominalMatrix4x4<ModInt>(
            new([-v[4 * 0 + 0], 1]), new([-v[4 * 0 + 1]]), new([-v[4 * 0 + 2]]), new([-v[4 * 0 + 3]]),
            new([-v[4 * 1 + 0]]), new([-v[4 * 1 + 1], 1]), new([-v[4 * 1 + 2]]), new([-v[4 * 1 + 3]]),
            new([-v[4 * 2 + 0]]), new([-v[4 * 2 + 1]]), new([-v[4 * 2 + 2], 1]), new([-v[4 * 2 + 3]]),
            new([-v[4 * 3 + 0]]), new([-v[4 * 3 + 1]]), new([-v[4 * 3 + 2]]), new([-v[4 * 3 + 3], 1]))
            .Determinant();
        await new ArrayMatrix<ModInt>(a, 4, 4).CharacteristricPolynomial().Should().BeEquivalentOrderTo(expected.Coefficients);
    }

    class PolynominalMatrix3x3<T> where T : INumberBase<T>
    {
        internal readonly Polynomial<ModInt>
            V00, V01, V02,
            V10, V11, V12,
            V20, V21, V22;
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Style", "IDE0290:プライマリ コンストラクターの使用", Justification = "いらん")]
        public PolynominalMatrix3x3(
            Polynomial<ModInt> V00, Polynomial<ModInt> V01, Polynomial<ModInt> V02,
            Polynomial<ModInt> V10, Polynomial<ModInt> V11, Polynomial<ModInt> V12,
            Polynomial<ModInt> V20, Polynomial<ModInt> V21, Polynomial<ModInt> V22
        )
        {
            this.V00 = V00; this.V01 = V01; this.V02 = V02;
            this.V10 = V10; this.V11 = V11; this.V12 = V12;
            this.V20 = V20; this.V21 = V21; this.V22 = V22;
        }

        public Polynomial<ModInt> Determinant()
        {
            return
             (V00 * V11 * V22
             + V10 * V02 * V21
             + V20 * V01 * V12)
             -
             (V00 * V12 * V21
             + V10 * V01 * V22
             + V20 * V02 * V11);
        }
    }
    class PolynominalMatrix4x4<T> where T : INumberBase<T>
    {
        internal readonly Polynomial<ModInt>
            V00, V01, V02, V03,
            V10, V11, V12, V13,
            V20, V21, V22, V23,
            V30, V31, V32, V33;

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Style", "IDE0290:プライマリ コンストラクターの使用", Justification = "いらん")]
        public PolynominalMatrix4x4(
            Polynomial<ModInt> V00, Polynomial<ModInt> V01, Polynomial<ModInt> V02, Polynomial<ModInt> V03,
            Polynomial<ModInt> V10, Polynomial<ModInt> V11, Polynomial<ModInt> V12, Polynomial<ModInt> V13,
            Polynomial<ModInt> V20, Polynomial<ModInt> V21, Polynomial<ModInt> V22, Polynomial<ModInt> V23,
            Polynomial<ModInt> V30, Polynomial<ModInt> V31, Polynomial<ModInt> V32, Polynomial<ModInt> V33
        )
        {
            this.V00 = V00; this.V01 = V01; this.V02 = V02; this.V03 = V03;
            this.V10 = V10; this.V11 = V11; this.V12 = V12; this.V13 = V13;
            this.V20 = V20; this.V21 = V21; this.V22 = V22; this.V23 = V23;
            this.V30 = V30; this.V31 = V31; this.V32 = V32; this.V33 = V33;
        }

        public Polynomial<ModInt> Determinant()
        {
            var r0c0 = (
             V11 * V22 * V33
             + V21 * V13 * V32
             + V31 * V12 * V23)
             -
             (V11 * V23 * V32
             + V21 * V12 * V33
             + V31 * V13 * V22);
            var r1c0 = (
             V10 * V23 * V32
             + V20 * V12 * V33
             + V30 * V13 * V22)
             -
             (V10 * V22 * V33
             + V20 * V13 * V32
             + V30 * V12 * V23);
            var r2c0 = (
             V10 * V21 * V33
             + V20 * V13 * V31
             + V30 * V11 * V23)
             -
             (V10 * V23 * V31
             + V20 * V11 * V33
             + V30 * V13 * V21);
            var r3c0 = (
             V10 * V22 * V31
             + V20 * V11 * V32
             + V30 * V12 * V21)
             -
             (V10 * V21 * V32
             + V20 * V12 * V31
             + V30 * V11 * V22);
            return V00 * r0c0 + V01 * r1c0 + V02 * r2c0 + V03 * r3c0;
        }
    }
}