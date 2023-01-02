using System.Collections.Generic;

namespace Kzrnm.Competitive.Internal
{
    public interface IPersistentSetNodeOperator<T, TKey>
    {
        TKey GetCompareKey(T value);
    }
    public interface IPersistentSetOperator<T, TKey, TSet, TNOp>
        where TNOp : struct, IPersistentSetNodeOperator<T, TKey>
    {
        /// <summary>
        /// 初期状態の <typeparamref name="TSet"/> を作成します。
        /// </summary>
        TSet Empty(bool isMulti, IComparer<TKey> comparer);
        /// <summary>
        /// 初期状態の <typeparamref name="TSet"/> を作成します。
        /// </summary>
        TSet Create(PersistentSetNode<T, TKey, TNOp> root, bool isMulti, IComparer<TKey> comparer);
    }
}
