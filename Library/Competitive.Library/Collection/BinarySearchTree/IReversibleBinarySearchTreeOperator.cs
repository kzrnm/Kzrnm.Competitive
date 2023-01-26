using AtCoder;
using 凾 = System.Runtime.CompilerServices.MethodImplAttribute;

namespace Kzrnm.Competitive
{
    [IsOperator]
    public interface IReversibleBinarySearchTreeOperator<T, F> : ILazySegtreeOperator<T, F>
    {
        /// <summary>
        /// <paramref name="v"/> を左右反転します。
        /// </summary>
        T Inverse(T v);
    }
    namespace Internal
    {
        public struct SingleRbstOp<T> : IReversibleBinarySearchTreeOperator<T, T>
        {
            public T Identity => default;
            public T FIdentity => default;
            [凾(256)] public T Composition(T nf, T cf) => nf;
            [凾(256)] public T Inverse(T v) => v;
            [凾(256)] public T Mapping(T f, T x) => x;
            [凾(256)] public T Operate(T x, T y) => x;
        }
    }
}
