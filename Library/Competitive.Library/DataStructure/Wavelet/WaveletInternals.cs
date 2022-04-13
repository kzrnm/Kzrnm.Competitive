using System;
using System.Collections.Generic;
using System.Numerics;
using System.Text;
using 凾 = System.Runtime.CompilerServices.MethodImplAttribute;

namespace Kzrnm.Competitive.Internal
{
    internal class SuccinctIndexableDictionary
    {
        readonly uint[] bit, sum;
        public SuccinctIndexableDictionary(int length)
        {
            var block = (length + 31) >> 5;
            bit = new uint[block];
            sum = new uint[block];
        }

        [凾(256)]
        public void Set(int k)
        {
            bit[k >> 5] |= 1U << (k & 0x1F);
        }
        public void Build()
        {
            sum[0] = 0U;
            for (int i = 1; i < sum.Length; i++)
            {
                sum[i] = sum[i - 1] + (uint)BitOperations.PopCount(bit[i - 1]);
            }
        }

        public bool this[int k]
        {
            [凾(256)]
            get => ((bit[k >> 5] >> (k & 0x1F)) & 1) != 0;
        }

        [凾(256)]
        public int Rank(int k) => (int)(sum[k >> 5] + (uint)BitOperations.PopCount(bit[k >> 5] & ((1U << (k & 0x1F)) - 1)));

        [凾(256)]
        public int Rank(bool val, int k) => val ? Rank(k) : k - Rank(k);
    }
}
