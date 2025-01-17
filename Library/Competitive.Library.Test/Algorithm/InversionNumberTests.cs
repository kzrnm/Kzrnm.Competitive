using System;

namespace Kzrnm.Competitive.Testing.Algorithm
{
    public class InversionNumberTests
    {
        [Fact]
        public void Int32()
        {
            var rnd = new Random(227);
            for (int len = 0; len < 10; len++)
                for (int q = 0; q < 2; q++)
                {
                    var a = new int[len];
                    foreach (ref var v in a.AsSpan())
                        v = rnd.Next(len) + 1;

                    InversionNumber.Inversion(a).ShouldBe(Naive((ReadOnlySpan<int>)a));
                }
        }
        [Fact]
        public void String()
        {
            var rnd = new Random(227);
            for (int len = 0; len < 30; len++)
                for (int q = 0; q < 2; q++)
                {
                    var a = new char[len];
                    foreach (ref var v in a.AsSpan())
                        v = (char)(rnd.Next(26) + 'A');

                    InversionNumber.Inversion(a).ShouldBe(Naive<char>(a));
                }
        }

        static long Naive<T>(ReadOnlySpan<T> a) where T : IComparable<T>
        {
            long s = 0;
            for (int i = 0; i < a.Length; i++)
                for (int j = i + 1; j < a.Length; j++)
                    if (a[i].CompareTo(a[j]) > 0)
                        ++s;
            return s;
        }
    }
}
