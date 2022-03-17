using System.Collections.Generic;
using AtCoder.Internal;

namespace Kzrnm.Competitive
{
    /// <summary>
    /// 機能を削った高速なArrayList
    /// </summary>
    public class SList<T> : SimpleList<T>
    {
        public SList() : base() { }
        public SList(int capacity) : base(capacity) { }
        public SList(IEnumerable<T> collection) : base(collection) { }

    }
}
