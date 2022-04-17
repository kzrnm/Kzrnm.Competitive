using AtCoder;

namespace Kzrnm.Competitive
{
    [IsOperator]
    public interface ISparseTableOperator<T>
    {
        T Operate(T x, T y);
    }
}
