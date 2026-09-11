using System.Collections.Generic;

namespace Kzrnm.Competitive
{
    public static class __LinkedListExtension
    {
        /// <summary>
        /// <paramref name="list"/> のノードを列挙します。
        /// </summary>
        public static LinkedListNode<T>[] EnumerateNodes<T>(this LinkedList<T> list)
        {
            var n = list.First;
            var r = new LinkedListNode<T>[list.Count];
            for (int i = 0; i < r.Length; i++, n = n.Next) r[i] = n;
            return r;
        }
    }
}