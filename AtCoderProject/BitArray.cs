using System;
using System.Collections.Generic;
using IEnumerable = System.Collections.IEnumerable;
using IEnumerator = System.Collections.IEnumerator;
using System.Linq;

#pragma warning disable 

namespace AtCoderProject.Hide
{
    struct BitArray : IEquatable<BitArray>, IEnumerable<bool>
    {
        private readonly long num;
        public BitArray(long num) { this.num = num; }
        public bool this[int index] => ((num >> index) & 1) != 0;

        public static BitArray operator &(BitArray bits, long r) => new BitArray(bits.num & r);
        public static BitArray operator |(BitArray bits, long r) => new BitArray(bits.num | r);
        public static BitArray operator ^(BitArray bits, long r) => new BitArray(bits.num ^ r);
        public static BitArray operator +(BitArray bits, long r) => new BitArray(bits.num + r);
        public static BitArray operator -(BitArray bits, long r) => new BitArray(bits.num - r);
        public static implicit operator BitArray(long num) => new BitArray(num);
        public static implicit operator long(BitArray bits) => bits.num;

        public override string ToString() => Convert.ToString(num, 2).PadLeft(sizeof(long) * 8, '0');
        public bool Equals(BitArray other) => this.num == other.num;
        public override bool Equals(object obj)
        {
            if (obj is BitArray)
                return this.Equals((BitArray)obj);
            return false;
        }
        public override int GetHashCode() => this.num.GetHashCode();
        public IEnumerator<bool> GetEnumerator()
        {
            const int len = sizeof(long) * 8;
            for (var i = 0; i < len; i++)
                yield return ((num >> i) & 1) == 1;
        }
        IEnumerator IEnumerable.GetEnumerator() => this.GetEnumerator();
    }

}
