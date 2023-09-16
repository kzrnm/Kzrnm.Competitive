using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;

namespace Kzrnm.Competitive.Testing.Number
{
    public class BigIntegerDecimalTest
    {
        static Random rnd = new(11);
        public static IEnumerable<ValueTuple<long>> LongNumbers_Data
        { get; } = SignedConstructor_Data().Select(t => ValueTuple.Create(t.num)).ToArray();
        public static IEnumerable<ValueTuple<ulong>> ULongNumbers_Data
        { get; } = UnsignedConstructor_Data().Select(t => ValueTuple.Create(t.num)).ToArray();

        public static IEnumerable<BigInteger> BigIntegers
        { get; } = LongNumbers_Data.Select(t => new BigInteger(t.Item1)).Concat(BigIntegers_Data_Gen().Select(t => t.Item1)).ToArray();
        static BigIntegerDecimal ConvertToDecimal(BigInteger value) => BigIntegerDecimal.Parse(value.ToString());

        public static IEnumerable<ValueTuple<BigInteger>> BigIntegers_Data_Gen()
        {
            var bytes = new byte[1000];
            rnd.NextBytes(bytes);

            yield return ValueTuple.Create(new BigInteger(bytes.AsSpan()[..10]));
            yield return ValueTuple.Create(new BigInteger(bytes.AsSpan()[..100]));
            yield return ValueTuple.Create(new BigInteger(bytes.AsSpan()[..500]));
            yield return ValueTuple.Create(new BigInteger(bytes.AsSpan()[..1000]));
            yield return ValueTuple.Create(-new BigInteger(bytes.AsSpan()[..10]));
            yield return ValueTuple.Create(-new BigInteger(bytes.AsSpan()[..100]));
            yield return ValueTuple.Create(-new BigInteger(bytes.AsSpan()[..500]));
            yield return ValueTuple.Create(-new BigInteger(bytes.AsSpan()[..1000]));
        }
        public static IEnumerable<(long num, int sign, uint[] digits)> SignedConstructor_Data()
        {
            yield return (+1, +1, null);
            yield return (+10, +10, null);
            yield return (+100, +100, null);
            yield return (+1000, +1000, null);
            yield return (+10000, +10000, null);
            yield return (+100000, +100000, null);
            yield return (+1000000, +1000000, null);
            yield return (+10000000, +10000000, null);
            yield return (+100000000, +100000000, null);
            yield return (+1000000000, +1, new uint[] { 0, 1 });
            yield return (+10000000000, +1, new uint[] { 0, 10 });
            yield return (+100000000000, +1, new uint[] { 0, 100 });
            yield return (+1000000000000, +1, new uint[] { 0, 1000 });
            yield return (+10000000000000, +1, new uint[] { 0, 10000 });
            yield return (+100000000000000, +1, new uint[] { 0, 100000 });
            yield return (+1000000000000000, +1, new uint[] { 0, 1000000 });
            yield return (+10000000000000000, +1, new uint[] { 0, 10000000 });
            yield return (+100000000000000000, +1, new uint[] { 0, 100000000, });
            yield return (+1000000000000000000, +1, new uint[] { 0, 0, 1 });
            yield return (-1, -1, null);
            yield return (-10, -10, null);
            yield return (-100, -100, null);
            yield return (-1000, -1000, null);
            yield return (-10000, -10000, null);
            yield return (-100000, -100000, null);
            yield return (-1000000, -1000000, null);
            yield return (-10000000, -10000000, null);
            yield return (-100000000, -100000000, null);
            yield return (-1000000000, -1, new uint[] { 0, 1 });
            yield return (-10000000000, -1, new uint[] { 0, 10 });
            yield return (-100000000000, -1, new uint[] { 0, 100 });
            yield return (-1000000000000, -1, new uint[] { 0, 1000 });
            yield return (-10000000000000, -1, new uint[] { 0, 10000 });
            yield return (-100000000000000, -1, new uint[] { 0, 100000 });
            yield return (-1000000000000000, -1, new uint[] { 0, 1000000 });
            yield return (-10000000000000000, -1, new uint[] { 0, 10000000 });
            yield return (-100000000000000000, -1, new uint[] { 0, 100000000, });
            yield return (-1000000000000000000, -1, new uint[] { 0, 0, 1 });

            yield return (+9, +9, null);
            yield return (+99, +99, null);
            yield return (+999, +999, null);
            yield return (+9999, +9999, null);
            yield return (+99999, +99999, null);
            yield return (+999999, +999999, null);
            yield return (+9999999, +9999999, null);
            yield return (+99999999, +99999999, null);
            yield return (+999999999, +999999999, null);
            yield return (+9999999999, +1, new uint[] { 999999999, 9 });
            yield return (+99999999999, +1, new uint[] { 999999999, 99 });
            yield return (+999999999999, +1, new uint[] { 999999999, 999 });
            yield return (+9999999999999, +1, new uint[] { 999999999, 9999 });
            yield return (+99999999999999, +1, new uint[] { 999999999, 99999 });
            yield return (+999999999999999, +1, new uint[] { 999999999, 999999 });
            yield return (+9999999999999999, +1, new uint[] { 999999999, 9999999 });
            yield return (+99999999999999999, +1, new uint[] { 999999999, 99999999 });
            yield return (+999999999999999999, +1, new uint[] { 999999999, 999999999 });
            yield return (-9, -9, null);
            yield return (-99, -99, null);
            yield return (-999, -999, null);
            yield return (-9999, -9999, null);
            yield return (-99999, -99999, null);
            yield return (-999999, -999999, null);
            yield return (-9999999, -9999999, null);
            yield return (-99999999, -99999999, null);
            yield return (-999999999, -999999999, null);
            yield return (-9999999999, -1, new uint[] { 999999999, 9 });
            yield return (-99999999999, -1, new uint[] { 999999999, 99 });
            yield return (-999999999999, -1, new uint[] { 999999999, 999 });
            yield return (-9999999999999, -1, new uint[] { 999999999, 9999 });
            yield return (-99999999999999, -1, new uint[] { 999999999, 99999 });
            yield return (-999999999999999, -1, new uint[] { 999999999, 999999 });
            yield return (-9999999999999999, -1, new uint[] { 999999999, 9999999 });
            yield return (-99999999999999999, -1, new uint[] { 999999999, 99999999 });
            yield return (-999999999999999999, -1, new uint[] { 999999999, 999999999 });

            yield return (long.MaxValue, 1, new uint[] { 854775807, 223372036, 9 });
            yield return (uint.MaxValue, 1, new uint[] { 294967295, 4 });
            yield return (int.MaxValue, 1, new uint[] { 147483647, 2 });
            yield return (0, 0, null);
            yield return (-int.MaxValue, -1, new uint[] { 147483647, 2 });
            yield return (int.MinValue, -1, new uint[] { 147483648, 2 });
            yield return (long.MinValue, -1, new uint[] { 854775808, 223372036, 9 });
        }

        [Theory]
        [TupleMemberData(nameof(SignedConstructor_Data))]
        public void SignedConstructor(long num, int expectedSign, uint[] expectedDigits)
        {
            var n = new BigIntegerDecimal(num);
            n.sign.Should().Be(expectedSign);
            n.digits.Should().Equal(expectedDigits);

            if (int.MinValue <= num && num <= int.MaxValue)
            {
                n = new BigIntegerDecimal((int)num);
                n.sign.Should().Be(expectedSign);
                n.digits.Should().Equal(expectedDigits);
            }
        }

        public static IEnumerable<(ulong num, int sign, uint[] digits)> UnsignedConstructor_Data()
        {
            yield return (+1, +1, null);
            yield return (+10, +10, null);
            yield return (+100, +100, null);
            yield return (+1000, +1000, null);
            yield return (+10000, +10000, null);
            yield return (+100000, +100000, null);
            yield return (+1000000, +1000000, null);
            yield return (+10000000, +10000000, null);
            yield return (+100000000, +100000000, null);
            yield return (+1000000000, +1, new uint[] { 0, 1 });
            yield return (+10000000000, +1, new uint[] { 0, 10 });
            yield return (+100000000000, +1, new uint[] { 0, 100 });
            yield return (+1000000000000, +1, new uint[] { 0, 1000 });
            yield return (+10000000000000, +1, new uint[] { 0, 10000 });
            yield return (+100000000000000, +1, new uint[] { 0, 100000 });
            yield return (+1000000000000000, +1, new uint[] { 0, 1000000 });
            yield return (+10000000000000000, +1, new uint[] { 0, 10000000 });
            yield return (+100000000000000000, +1, new uint[] { 0, 100000000, });
            yield return (+1000000000000000000, +1, new uint[] { 0, 0, 1 });

            yield return (+9, +9, null);
            yield return (+99, +99, null);
            yield return (+999, +999, null);
            yield return (+9999, +9999, null);
            yield return (+99999, +99999, null);
            yield return (+999999, +999999, null);
            yield return (+9999999, +9999999, null);
            yield return (+99999999, +99999999, null);
            yield return (+999999999, +999999999, null);
            yield return (+9999999999, +1, new uint[] { 999999999, 9 });
            yield return (+99999999999, +1, new uint[] { 999999999, 99 });
            yield return (+999999999999, +1, new uint[] { 999999999, 999 });
            yield return (+9999999999999, +1, new uint[] { 999999999, 9999 });
            yield return (+99999999999999, +1, new uint[] { 999999999, 99999 });
            yield return (+999999999999999, +1, new uint[] { 999999999, 999999 });
            yield return (+9999999999999999, +1, new uint[] { 999999999, 9999999 });
            yield return (+99999999999999999, +1, new uint[] { 999999999, 99999999 });
            yield return (+999999999999999999, +1, new uint[] { 999999999, 999999999 });
            yield return (+9999999999999999999, +1, new uint[] { 999999999, 999999999, 9 });

            yield return (ulong.MaxValue, 1, new uint[] { 709551615, 446744073, 18 });
            yield return (long.MaxValue, 1, new uint[] { 854775807, 223372036, 9 });
            yield return (uint.MaxValue, 1, new uint[] { 294967295, 4 });
            yield return (int.MaxValue, 1, new uint[] { 147483647, 2 });
            yield return (0, 0, null);
        }

        [Theory]
        [TupleMemberData(nameof(UnsignedConstructor_Data))]
        public void UnsignedConstructor(ulong num, int expectedSign, uint[] expectedDigits)
        {
            var n = new BigIntegerDecimal(num);
            n.sign.Should().Be(expectedSign);
            n.digits.Should().Equal(expectedDigits);

            if (num <= uint.MaxValue)
            {
                n = new BigIntegerDecimal((uint)num);
                n.sign.Should().Be(expectedSign);
                n.digits.Should().Equal(expectedDigits);
            }
        }

        [Fact]
        public void EqualAndCompare()
        {
            var orig = LongNumbers_Data.Select(t => t.Item1).ToArray();
            Array.Sort(orig);
            var bigs = orig.Select(l => new BigIntegerDecimal(l)).ToArray();

            for (int i = 0; i < bigs.Length; i++)
            {
                bigs[i].CompareTo(bigs[i]).Should().Be(0);
                bigs[i].Equals(bigs[i]).Should().BeTrue();
                BigIntegerDecimal.Max(bigs[i], bigs[i]).Should().Be(bigs[i]);
                BigIntegerDecimal.Min(bigs[i], bigs[i]).Should().Be(bigs[i]);
                for (int j = i + 1; j < bigs.Length; j++)
                {
                    bigs[i].CompareTo(bigs[j]).Should().Be(-1);
                    bigs[i].Equals(bigs[j]).Should().BeFalse();

                    BigIntegerDecimal.Max(bigs[i], bigs[j]).Should().Be(bigs[j]);
                    BigIntegerDecimal.Min(bigs[i], bigs[j]).Should().Be(bigs[i]);

                    (bigs[i] < bigs[j]).Should().BeTrue();
                    (bigs[i] <= bigs[j]).Should().BeTrue();
                    (bigs[i] > bigs[j]).Should().BeFalse();
                    (bigs[i] >= bigs[j]).Should().BeFalse();

                    (bigs[i] == bigs[j]).Should().BeFalse();
                    (bigs[i] != bigs[j]).Should().BeTrue();


                    bigs[j].CompareTo(bigs[i]).Should().Be(1);
                    bigs[j].Equals(bigs[i]).Should().BeFalse();

                    BigIntegerDecimal.Max(bigs[j], bigs[i]).Should().Be(bigs[j]);
                    BigIntegerDecimal.Min(bigs[j], bigs[i]).Should().Be(bigs[i]);

                    (bigs[j] < bigs[i]).Should().BeFalse();
                    (bigs[j] <= bigs[i]).Should().BeFalse();
                    (bigs[j] > bigs[i]).Should().BeTrue();
                    (bigs[j] >= bigs[i]).Should().BeTrue();

                    (bigs[j] == bigs[i]).Should().BeFalse();
                    (bigs[j] != bigs[i]).Should().BeTrue();
                }
            }
        }


        [Fact]
        public void EqualAndCompareTypes()
        {
            Inner<sbyte>();
            Inner<short>();
            Inner<int>();
            Inner<long>();
            Inner<char>();
            Inner<nint>();

            Inner<byte>();
            Inner<ushort>();
            Inner<uint>();
            Inner<ulong>();
            Inner<nuint>();
            static void Inner<T>() where T : INumber<T>
            {
                var orig = LongNumbers_Data.Select(t => T.CreateSaturating(t.Item1)).Distinct().ToArray();
                Array.Sort(orig);
                var bigs = orig.Select(BigIntegerDecimal.CreateChecked).ToArray();

                for (int i = 0; i < bigs.Length; i++)
                {
                    bigs[i].CompareTo(orig[i]).Should().Be(0);
                    bigs[i].Equals(orig[i]).Should().BeTrue();
                    for (int j = i + 1; j < bigs.Length; j++)
                    {
                        bigs[i].CompareTo(orig[j]).Should().Be(-1);
                        bigs[i].Equals(orig[j]).Should().BeFalse();

                        bigs[j].CompareTo(orig[i]).Should().Be(1);
                        bigs[j].Equals(orig[i]).Should().BeFalse();
                    }
                }
            }
        }



        [Theory]
        [TupleMemberData(nameof(LongNumbers_Data))]
        public void Convert(long num)
        {
            try
            {
                var v = checked((sbyte)num);
                try
                {
                    ((sbyte)(BigIntegerDecimal)v).Should().Be((sbyte)num);
                }
                catch (OverflowException)
                {
                    Assert.True(false);
                }
            }
            catch (OverflowException)
            {
                num.Invoking(v => (sbyte)(BigIntegerDecimal)v).Should().Throw<OverflowException>();
            }
            try
            {
                var v = checked((short)num);
                try
                {
                    ((short)(BigIntegerDecimal)v).Should().Be((short)num);
                }
                catch (OverflowException)
                {
                    Assert.True(false);
                }
            }
            catch (OverflowException)
            {
                num.Invoking(v => (short)(BigIntegerDecimal)v).Should().Throw<OverflowException>();
            }
            try
            {
                var v = checked((int)num);
                try
                {
                    ((int)(BigIntegerDecimal)v).Should().Be((int)num);
                }
                catch (OverflowException)
                {
                    Assert.True(false);
                }
            }
            catch (OverflowException)
            {
                num.Invoking(v => (int)(BigIntegerDecimal)v).Should().Throw<OverflowException>();
            }
            try
            {
                var v = checked((long)num);
                try
                {
                    ((long)(BigIntegerDecimal)v).Should().Be((long)num);
                }
                catch (OverflowException)
                {
                    Assert.True(false);
                }
            }
            catch (OverflowException)
            {
                num.Invoking(v => (long)(BigIntegerDecimal)v).Should().Throw<OverflowException>();
            }
            try
            {
                var v = checked((char)num);
                try
                {
                    ((char)(BigIntegerDecimal)v).Should().Be((char)num);
                }
                catch (OverflowException)
                {
                    Assert.True(false);
                }
            }
            catch (OverflowException)
            {
                num.Invoking(v => (char)(BigIntegerDecimal)v).Should().Throw<OverflowException>();
            }
            try
            {
                var v = checked((nint)num);
                try
                {
                    ((nint)(BigIntegerDecimal)v).Should().Be((nint)num);
                }
                catch (OverflowException)
                {
                    Assert.True(false);
                }
            }
            catch (OverflowException)
            {
                num.Invoking(v => (nint)(BigIntegerDecimal)v).Should().Throw<OverflowException>();
            }
            try
            {
                var v = checked((byte)num);
                try
                {
                    ((byte)(BigIntegerDecimal)v).Should().Be((byte)num);
                }
                catch (OverflowException)
                {
                    Assert.True(false);
                }
            }
            catch (OverflowException)
            {
                num.Invoking(v => (byte)(BigIntegerDecimal)v).Should().Throw<OverflowException>();
            }
            try
            {
                var v = checked((ushort)num);
                try
                {
                    ((ushort)(BigIntegerDecimal)v).Should().Be((ushort)num);
                }
                catch (OverflowException)
                {
                    Assert.True(false);
                }
            }
            catch (OverflowException)
            {
                num.Invoking(v => (ushort)(BigIntegerDecimal)v).Should().Throw<OverflowException>();
            }
            try
            {
                var v = checked((uint)num);
                try
                {
                    ((uint)(BigIntegerDecimal)v).Should().Be((uint)num);
                }
                catch (OverflowException)
                {
                    Assert.True(false);
                }
            }
            catch (OverflowException)
            {
                num.Invoking(v => (uint)(BigIntegerDecimal)v).Should().Throw<OverflowException>();
            }
            try
            {
                var v = checked((ulong)num);
                try
                {
                    ((ulong)(BigIntegerDecimal)v).Should().Be((ulong)num);
                }
                catch (OverflowException)
                {
                    Assert.True(false);
                }
            }
            catch (OverflowException)
            {
                num.Invoking(v => (ulong)(BigIntegerDecimal)v).Should().Throw<OverflowException>();
            }
            try
            {
                var v = checked((nuint)num);
                try
                {
                    ((nuint)(BigIntegerDecimal)v).Should().Be((nuint)num);
                }
                catch (OverflowException)
                {
                    Assert.True(false);
                }
            }
            catch (OverflowException)
            {
                num.Invoking(v => (nuint)(BigIntegerDecimal)v).Should().Throw<OverflowException>();
            }
        }

        [Theory]
        [TupleMemberData(nameof(SignedConstructor_Data))]
        public void String(long num, int expectedSign, uint[] expectedDigits)
        {
            var n = BigIntegerDecimal.Parse(num.ToString());
            n.sign.Should().Be(expectedSign);
            n.digits.Should().Equal(expectedDigits);

            BigIntegerDecimal.TryParse(num.ToString(), out var m).Should().BeTrue();
            m.Should().Be(n);

            var str = num.ToString();
            n.ToString().Should().Be(str);

            var buffer = new char[str.Length];
            n.TryFormat(buffer, out var charsWritten).Should().BeTrue();
            charsWritten.Should().Be(str.Length);
            buffer.Should().StartWith(str.ToCharArray());
            n.TryFormat(new char[str.Length - 1], out _).Should().BeFalse();
        }

        [Fact]
        public void StringLarge()
        {
            for (int i = 1; i <= 100; i++)
            {
                var str = string.Concat(Enumerable.Repeat("123456789444555666987654321", i));
                var n = BigIntegerDecimal.Parse(str);
                n.sign.Should().Be(1, "index: {0}", i);
                n.digits.Should().Equal(Enumerable.Repeat(new uint[] { 987654321, 444555666, 123456789, }, i).SelectMany(a => a), "index: {0}", i);
                n.ToString().Should().Be(str, "index: {0}", i);

                var m = BigIntegerDecimal.Parse("-" + str);
                m.sign.Should().Be(-1, "index: {0}", i);
                m.digits.Should().Equal(Enumerable.Repeat(new uint[] { 987654321, 444555666, 123456789, }, i).SelectMany(a => a), "index: {0}", i);
                m.ToString().Should().Be("-" + str, "index: {0}", i);
            }
        }


        [Theory]
        [InlineData("12 3")]
        [InlineData("12 12345678901234567890")]
        public void ParseFail(string value)
        {
            _ = value.Invoking(v => BigIntegerDecimal.Parse(v)).Should().Throw<FormatException>();
            BigIntegerDecimal.TryParse(value, out var m).Should().BeFalse();
        }

        [Theory]
        [TupleMemberData(nameof(LongNumbers_Data))]
        public void Log(long value)
        {
            if (double.IsNaN(Math.Log(value)))
            {
                BigIntegerDecimal.Log10(new(value)).Should().Be(double.NaN);
                BigIntegerDecimal.Log(new(value)).Should().Be(double.NaN);
                BigIntegerDecimal.Log(new(value), 3).Should().Be(double.NaN);
            }
            else
            {
                BigIntegerDecimal.Log10(new(value)).Should().Be(Math.Log10(value));
                BigIntegerDecimal.Log(new(value)).Should().BeApproximately(Math.Log(value), 1e-11);
                BigIntegerDecimal.Log(new(value), 3).Should().BeApproximately(Math.Log(value, 3), 1e-11);
            }
        }

        [Theory]
        [TupleMemberData(nameof(LongNumbers_Data))]
        public void UnaryOpeator(long value)
        {
            var big = (BigIntegerDecimal)value;
            ((long)+big).Should().Be(value);
            if (value != long.MinValue)
                ((long)-big).Should().Be(-value);
        }

        [Fact]
        public void Add()
        {
            var orig = BigIntegers.ToArray();
            Array.Sort(orig);
            var bigs = orig.Select(ConvertToDecimal).ToArray();

            for (int i = 0; i < bigs.Length; i++)
            {
                (bigs[i] + bigs[i]).ToString().Should().Be((orig[i] + orig[i]).ToString());
                for (int j = i + 1; j < bigs.Length; j++)
                {
                    var expected = (orig[i] + orig[j]).ToString();
                    (bigs[i] + bigs[j]).ToString().Should().Be(expected);
                    (bigs[j] + bigs[i]).ToString().Should().Be(expected);
                }
            }
        }

        [Fact]
        public void Subtract()
        {
            var orig = BigIntegers.ToArray();
            Array.Sort(orig);
            var bigs = orig.Select(ConvertToDecimal).ToArray();

            for (int i = 0; i < bigs.Length; i++)
            {
                for (int j = 0; j < bigs.Length; j++)
                {
                    (bigs[i] - bigs[j]).ToString().Should().Be((orig[i] - orig[j]).ToString());
                }
            }
        }

        [Fact]
        public void Multiply()
        {
            var orig = BigIntegers.ToArray();
            Array.Sort(orig);
            var bigs = orig.Select(ConvertToDecimal).ToArray();

            for (int i = 0; i < bigs.Length; i++)
            {
                (bigs[i] * bigs[i]).ToString().Should().Be((orig[i] * orig[i]).ToString());
                for (int j = i + 1; j < bigs.Length; j++)
                {
                    var expected = (orig[i] * orig[j]).ToString();
                    (bigs[i] * bigs[j]).ToString().Should().Be(expected);
                    (bigs[j] * bigs[i]).ToString().Should().Be(expected);
                }
            }
        }

        [Fact]
        public void Divide()
        {
            var orig = BigIntegers.ToArray();
            Array.Sort(orig);
            var bigs = orig.Select(ConvertToDecimal).ToArray();
            ConvertToDecimal(orig[1]);

            for (int i = 0; i < bigs.Length; i++)
            {
                for (int j = 0; j < bigs.Length; j++)
                {
                    if (orig[j].IsZero)
                    {
                        bigs[i].Invoking(d => d / bigs[j]).Should().Throw<DivideByZeroException>();
                        bigs[i].Invoking(d => d % bigs[j]).Should().Throw<DivideByZeroException>();
                    }
                    else
                    {
                        var expectedQuotient = (orig[i] / orig[j]).ToString();
                        var expectedRemainder = (orig[i] % orig[j]).ToString();
                        (bigs[i] / bigs[j]).ToString().Should().Be(expectedQuotient);
                        (bigs[i] % bigs[j]).ToString().Should().Be(expectedRemainder);

                        var quo = BigIntegerDecimal.DivRem(bigs[i], bigs[j], out var rem);
                        quo.ToString().Should().Be(expectedQuotient);
                        rem.ToString().Should().Be(expectedRemainder);
                    }
                }
            }
        }
    }
}
