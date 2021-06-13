using System.Collections.Generic;

namespace Kzrnm.Competitive.SetInternals
{
    public interface INodeOperator<T, TCmp, Node> : IComparer<TCmp>
    {
        Node Create(T item, NodeColor color);
        T GetValue(Node node);
        void SetValue(ref Node node, T value);
        TCmp GetCompareKey(T item);
        int Compare(T x, T y);
        int Compare(Node x, Node y);
        int Compare(TCmp value, Node node);
    }
    public enum NodeColor : byte
    {
        Black,
        Red
    }
    enum TreeRotation : byte
    {
        Left = 1,
        Right = 2,
        RightLeft = 3,
        LeftRight = 4,
    }
}
