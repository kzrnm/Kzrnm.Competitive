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
        public MyStringBuilder Add(params object[] objs) { sb.AppendJoin(' ', objs).AppendLine(); return this; }
        public MyStringBuilder Add<T>(IEnumerable<T> objs) { sb.AppendJoin(' ', objs).AppendLine(); return this; }
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
        /// <summary>
        /// <paramref name="minValue"/> 以上、<paramref name="maxValueExclusive"/> 未満の値を 1 つ追加します。
        /// </summary>
        public MyStringBuilder AddRandom(long minValue, long maxValueExclusive)
            => AddRandom(1, minValue, maxValueExclusive);
        /// <summary>
        /// <paramref name="minValue"/> 以上、<paramref name="maxValueExclusive"/> 未満の値を <paramref name="count"/> 個追加します。
        /// </summary>
        public MyStringBuilder AddRandom(int count, long minValue, long maxValueExclusive)
        {
            while (--count >= 0)
                Add(HandMadeMain.rnd.NextInt64(minValue, maxValueExclusive).ToString());
            return this;
        }
        public MyStringBuilder Clear() { sb.Clear(); return this; }
        IEnumerator IEnumerable.GetEnumerator() { throw new NotSupportedException(); }
    }
}
