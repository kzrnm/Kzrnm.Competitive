using System;
using 凾 = System.Runtime.CompilerServices.MethodImplAttribute;

namespace Kzrnm.Competitive
{
    public static class NthRoots
    {
        /// <summary>
        /// <paramref name="num"/> の <paramref name="n"/> 乗根を整数に切り捨てた値
        /// </summary>
        [凾(256)]
        public static ulong IntegerRoot(ulong num, long n)
        {
            if (num <= 1 || n == 1) return num;
            return new Op(num, n).BinarySearch(0, num);
        }

        private readonly struct Op : IOk<ulong>
        {
            public Op(ulong a, long k)
            {
                this.a = a;
                this.k = k;
            }
            public readonly ulong a;
            public readonly long k;
            [凾(256)]
            public bool Ok(ulong x)
            {
                var y = k;
                ulong res = ((y & 1) != 0) ? x : 1;
                for (y >>= 1; y > 0; y >>= 1)
                {
                    if (x > uint.MaxValue) return false;
                    x *= x;
                    if ((y & 1) != 0)
                    {
                        if (ulong.MaxValue / res < x) return false;
                        unchecked
                        {
                            try
                            {
                                res *= x;
                            }
                            catch (OverflowException)
                            {
                                return false;
                            }
                        }
                    }
                    if (x > a || res > a) return false;
                }
                return res <= a;
            }
        }
    }
}
