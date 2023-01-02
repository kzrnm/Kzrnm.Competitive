using Kzrnm.Competitive.Internal;
using System;
using System.Collections.Generic;
using 凾 = System.Runtime.CompilerServices.MethodImplAttribute;

namespace Kzrnm.Competitive
{
    // competitive-verifier: TITLE 永続Set
    public class PersistentSet<T> : PersistentSetBase<T, T, PersistentSet<T>, PersistentSet<T>.PSOperator>, ICollection<T>
    {
        public struct PSOperator : IPersistentSetNodeOperator<T, T>, IPersistentSetOperator<T, T, PersistentSet<T>, PSOperator>
        {
            [凾(256)]
            public PersistentSet<T> Empty(bool isMulti, IComparer<T> comparer)
            => new PersistentSet<T>(PersistentSetNode<T, T, PSOperator>.EmptyNode, isMulti, comparer ?? Comparer<T>.Default);

            [凾(256)]
            public PersistentSet<T> Create(PersistentSetNode<T, T, PSOperator> root, bool isMulti, IComparer<T> comparer)
                => new PersistentSet<T>(root, isMulti, comparer);

            [凾(256)] public T GetCompareKey(T value) => value;
        }
        private PersistentSet(PersistentSetNode<T, T, PSOperator> root, bool isMulti, IComparer<T> comparer)
            : base(root, isMulti, comparer) { }

        bool ICollection<T>.Contains(T item)
        {
            throw new NotSupportedException();
        }
    }
}
