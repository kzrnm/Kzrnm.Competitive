using AtCoder;
using 凾 = System.Runtime.CompilerServices.MethodImplAttribute;

namespace Kzrnm.Competitive.Internal
{
    [IsOperator]
    public interface ISetOperator<T, TCmp, Node, TOp>
    {
        static abstract Node Create(T item, NodeColor color);
        static abstract T GetValue(Node node);
        static abstract TCmp GetCompareKey(TOp comparer, T item);
    }

    [IsOperator]
    public interface ISetBinarySearchOperator
    {
        /// <summary>
        /// 左側を返す
        /// </summary>
        bool ReturnLeft { get; }

        /// <summary>
        /// 左側に潜る
        /// </summary>
        bool IntoLeft(int order);
    }
    public readonly struct SetLower : ISetBinarySearchOperator
    {
        public bool ReturnLeft => false;
        [凾(256)]
        public bool IntoLeft(int order) => order <= 0;
    }
    public readonly struct SetUpper : ISetBinarySearchOperator
    {
        public bool ReturnLeft => false;
        [凾(256)]
        public bool IntoLeft(int order) => order < 0;
    }
    public readonly struct SetLowerRev : ISetBinarySearchOperator
    {
        public bool ReturnLeft => true;
        [凾(256)]
        public bool IntoLeft(int order) => order < 0;
    }
    public readonly struct SetUpperRev : ISetBinarySearchOperator
    {
        public bool ReturnLeft => true;
        [凾(256)]
        public bool IntoLeft(int order) => order <= 0;
    }
}
