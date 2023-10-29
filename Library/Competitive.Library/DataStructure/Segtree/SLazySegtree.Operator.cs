using AtCoder;

namespace Kzrnm.Competitive
{
    /// <summary>
    /// モノイドを定義するインターフェイスです。
    /// </summary>
    /// <typeparam name="T">操作を行う型。</typeparam>
    /// <typeparam name="F">写像の型。</typeparam>
    [IsOperator]
    public interface ISLazySegtreeOperator<T, F>
    {
        /// <summary>
        /// <c>Operate(x, Identity) = x</c> を満たす単位元。
        /// </summary>
        T Identity { get; }
        /// <summary>
        /// <c>Mapping(FIdentity, x) = x</c> を満たす恒等写像。
        /// </summary>
        F FIdentity { get; }
        /// <summary>
        /// 結合律 Operate(Operate(a, b), c) = Operate(a, Operate(b, c)) を満たす写像。
        /// </summary>
        T Operate(T x, T y);
        /// <summary>
        /// 写像　<paramref name="f"/> を 大きさ <paramref name="size"/> の <paramref name="x"/> に作用させる関数。
        /// </summary>
        T Mapping(F f, T x, int size);
        /// <summary>
        /// 写像　<paramref name="nf"/> を既存の写像 <paramref name="cf"/> に対して合成した写像 <paramref name="nf"/>∘<paramref name="cf"/>。
        /// </summary>
        F Composition(F nf, F cf);
    }
}