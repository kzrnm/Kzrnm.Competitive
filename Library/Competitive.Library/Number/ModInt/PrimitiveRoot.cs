using AtCoder;
using System;
using System.Collections.Generic;
using System.Numerics;
using 凾 = System.Runtime.CompilerServices.MethodImplAttribute;


namespace Kzrnm.Competitive
{
    /// <summary>
    /// 原始根
    /// </summary>
    public static class PrimitiveRoot
    {
        /// <summary>
        /// <para>素数 <paramref name="p"/> について、mod <paramref name="p"/> での原始根を返します。</para>
        /// <para>原始根: a^k≡1 (mod <paramref name="p"/>) となる最小の k が <paramref name="p"/>-1 である a の値。</para>
        /// </summary>
        /// <remarks>
        /// <para>制約: <paramref name="p"/> は素数</para>
        /// </remarks>
        [凾(256)]
        public static int Solve(int p) => (int)Solve((uint)p);

        /// <summary>
        /// <para>素数 <paramref name="p"/> について、mod <paramref name="p"/> での原始根を返します。</para>
        /// <para>原始根: a^k≡1 (mod <paramref name="p"/>) となる最小の k が <paramref name="p"/>-1 である a の値。</para>
        /// </summary>
        /// <remarks>
        /// <para>制約: <paramref name="p"/> は素数</para>
        /// </remarks>
        [凾(256)]
        public static uint Solve(uint p)
        {
#if NET7_0_OR_GREATER
            ref var result = ref System.Runtime.InteropServices.CollectionsMarshal.GetValueRefOrAddDefault(primitiveRootsCache, p, out var exists);
            if (!exists)
            {
                result = Calculate(p);
            }
            return result;
#else
            if (primitiveRootsCache.TryGetValue(p, out var v))
            {
                return v;
            }

            return primitiveRootsCache[p] = Calculate(p);
#endif
        }

        static readonly Dictionary<uint, uint> primitiveRootsCache = new Dictionary<uint, uint>()
        {
            { 2, 1 },
            { 167772161, 3 },
            { 469762049, 3 },
            { 754974721, 11 },
            { 998244353, 3 }
        };

        static uint Calculate(uint m)
        {
            // ac-library-csharp は StaticModInt で実装している
            Span<uint> divs = stackalloc uint[20];
            divs[0] = 2;
            int cnt = 1;
            var x = m - 1;
            x >>= BitOperations.TrailingZeroCount(x);

            for (uint i = 3; (long)i * i <= x; i += 2)
            {
                if (x % i == 0)
                {
                    divs[cnt++] = i;
                    do
                    {
                        x /= i;
                    } while (x % i == 0);
                }
            }

            if (x > 1)
            {
                divs[cnt++] = x;
            }
            divs = divs[..cnt];

            for (uint g = 2; ; g++)
            {
                foreach (var d in divs)
                    if (MathLib.PowMod(g, (m - 1) / d, (int)m) == 1)
                        goto NEXT;
                return g;
            NEXT:;
            }
        }
    }
}
