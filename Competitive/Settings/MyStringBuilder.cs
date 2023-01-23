using System;
using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using System.Runtime.CompilerServices;
using System.Text;

namespace Competitive.Runner
{
    class MyStringBuilder : IEnumerable
    {
        public readonly StringBuilder sb = new StringBuilder();
        public int Length => sb.Length;
        public override string ToString() => sb.ToString();
        public MyStringBuilder Add(object o) { sb.AppendLine(o.ToString()); return this; }
        public MyStringBuilder Add(string s) { sb.AppendLine(s); return this; }
        public MyStringBuilder Add(params object[] objs) { sb.AppendLine(string.Join(" ", objs)); return this; }
        public MyStringBuilder Add<T>(IEnumerable<T> objs) { sb.AppendLine(string.Join(" ", objs)); return this; }
        public MyStringBuilder Add<T>(T objs) where T : ITuple
        {
            for (int i = 0; i < objs.Length; i++)
            {
                if (i > 0) sb.Append(' ');
                sb.Append(objs[i].ToString());
            }
            sb.AppendLine();
            return this;
        }
        public MyStringBuilder AddRandom(long minValue, long maxValue)
        {
            // Originai is .NET runtime
            // https://github.com/dotnet/runtime/blob/57bfe474518ab5b7cfe6bf7424a79ce3af9d6657/src/libraries/System.Private.CoreLib/src/System/Random.Xoshiro256StarStarImpl.cs
            // LICENSE: https://github.com/dotnet/runtime/blob/main/LICENSE.TXT

            static ulong UInt64Random()
            {
                Span<byte> resultBytes = stackalloc byte[8];
                HandMadeMain.rnd.NextBytes(resultBytes);
                return BitConverter.ToUInt64(resultBytes);
            }
            static int Log2Ceiling(ulong value)
            {
                int result = BitOperations.Log2(value);
                if (BitOperations.PopCount(value) != 1)
                {
                    result++;
                }
                return result;
            }
            static long Int64Random(long minValue, long maxValue)
            {
                ulong exclusiveRange = (ulong)(maxValue - minValue);

                if (exclusiveRange > 1)
                {
                    int bits = Log2Ceiling(exclusiveRange);
                    while (true)
                    {
                        ulong result = UInt64Random() >> (sizeof(long) * 8 - bits);
                        if (result < exclusiveRange)
                        {
                            return (long)result + minValue;
                        }
                    }
                }

                return minValue;
            }
            Add(Int64Random(minValue, maxValue).ToString());
            return this;
        }
        public MyStringBuilder Clear() { sb.Clear(); return this; }
        IEnumerator IEnumerable.GetEnumerator() { throw new NotSupportedException(); }
    }
}
