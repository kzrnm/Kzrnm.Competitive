using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BitArray = System.Collections.BitArray;
using BigInteger = System.Numerics.BigInteger;

#pragma warning disable 

namespace AtCoderProject.Hide
{
    static class BitArrayUtils
    {
        static BitArray ToBitArray(this long l)
            => new BitArray(BitConverter.GetBytes(l));
        static long ToInt64(this BitArray bitArray)
        {
            var array = new byte[8];
            bitArray.CopyTo(array, 0);
            return BitConverter.ToInt64(array, 0);
        }
        static BitArray ToBitArray(this ulong l)
            => new BitArray(BitConverter.GetBytes(l));
        static ulong ToUInt64(this BitArray bitArray)
        {
            var array = new byte[8];
            bitArray.CopyTo(array, 0);
            return BitConverter.ToUInt64(array, 0);
        }
    }
}
