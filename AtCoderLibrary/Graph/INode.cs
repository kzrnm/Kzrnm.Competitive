using System.Collections.Generic;

namespace AtCoder.Graph
{
    public interface INode
    {
        public int Index { get; }
        public IEnumerable<int> Roots { get; }
        public IEnumerable<int> Children { get; }
        bool IsDirected { get; }
    }
    public interface ITreeNode
    {
        public int Index { get; }
        public int Root { get; }
        public int Depth { get; }
        public IEnumerable<int> Children { get; }
    }
}
