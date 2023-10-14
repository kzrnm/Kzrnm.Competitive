using System;
using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using System.Runtime.CompilerServices;

namespace Kzrnm.Competitive
{
    // competitive-verifier: TITLE BitsEnumerator:GenericMath
    public struct BitsEnumerator<T> : IEnumerable<int>, IEnumerator<int> where T : IBinaryInteger<T>
    {
        T num;
        public BitsEnumerator(T num) { this.num = num; Current = -1; }
        public BitsEnumerator<T> GetEnumerator() => this;
        public int Current { get; private set; }
        [MethodImpl(256)]
        public bool MoveNext()
        {
            if (T.IsZero(num)) return false;
            var l = int.CreateTruncating(T.TrailingZeroCount(num));

            // l == AllBitsSet のときにオーバーフローしないようにする
            Current += l; ++Current;
            num >>>= l; num >>>= 1;
            return true;
        }
        object IEnumerator.Current => Current;
        public int[] ToArray()
        {
            var res = new int[int.CreateTruncating(T.PopCount(num))];
            for (int i = 0; i < res.Length; i++)
            {
                MoveNext();
                res[i] = Current;
            }
            return res;
        }
        void IEnumerator.Reset() => throw new NotSupportedException();
        void IDisposable.Dispose() { }
        IEnumerator<int> IEnumerable<int>.GetEnumerator() => this;
        IEnumerator IEnumerable.GetEnumerator() => this;
    }
}
