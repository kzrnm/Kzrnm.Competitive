using System.Collections.Generic;
using BitArray = System.Collections.BitArray;

namespace Kzrnm.Competitive
{
    public class BitArrayComparer : IComparer<BitArray>
    {
        private readonly bool IsReverse;
        public BitArrayComparer(bool isReverse = false)
        {
            IsReverse = isReverse;
        }
        public static BitArrayComparer Default => new BitArrayComparer(false);
        public static BitArrayComparer Reverse => new BitArrayComparer(true);
        public int Compare(BitArray x, BitArray y)
        {
            if (IsReverse)
                (x, y) = (y, x);
            for (int i = 0; i < x.Length && i < y.Length; i++)
            {
                var cmp = x[i].CompareTo(y[i]);
                if (cmp != 0)
                    return cmp;
            }
            return x.Length.CompareTo(y.Length);
        }
    }
}
