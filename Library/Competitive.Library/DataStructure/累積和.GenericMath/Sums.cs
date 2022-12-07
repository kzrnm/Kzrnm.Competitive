using System.Collections.Generic;
using System.Numerics;
using 凾 = System.Runtime.CompilerServices.MethodImplAttribute;

namespace Kzrnm.Competitive
{
    /// <summary>
    /// 累積和を求めます。
    /// </summary>
    public class Sums<T>
        where T : IAdditionOperators<T, T, T>, ISubtractionOperators<T, T, T>
    {
        private readonly T[] impl;
        public int Length => impl.Length - 1;
        public Sums(T[] arr, T defaultValue = default)
        {
            impl = new T[arr.Length + 1];
            impl[0] = defaultValue;
            for (var i = 0; i < arr.Length; i++)
                impl[i + 1] = impl[i] + arr[i];
        }
        public Sums(IList<T> collection)
        {
            impl = new T[collection.Count + 1];
            for (var i = 0; i < collection.Count; i++)
                impl[i + 1] = impl[i] + collection[i];
        }
        [凾(256)] public T Slice(int from, int length) => impl[from + length] - impl[from];
        public T this[int toExclusive] { [凾(256)] get => impl[toExclusive]; }
        public T this[int from, int toExclusive] { [凾(256)] get => impl[toExclusive] - impl[from]; }
    }
}
