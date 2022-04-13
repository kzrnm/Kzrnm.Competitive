using Kzrnm.Competitive.Internal;
using System;
using System.Collections;
using System.Collections.Generic;
using 凾 = System.Runtime.CompilerServices.MethodImplAttribute;

namespace Kzrnm.Competitive
{
    public static class __CollectionExtension_Chunk
    {
        /// <summary>
        /// コレクションの要素をいくつかの要素ごとに分割したバッファーにする。最後の要素は数が足りない可能性あり
        /// </summary>
        [凾(256)]
        public static ChunkBuffer<T> Chunk<T>(this IEnumerable<T> collection, int bufferSize) => new ChunkBuffer<T>(collection, bufferSize);
    }
    namespace Internal
    {
        public class ChunkBuffer<T> : IEnumerable<T[]>
        {
            private readonly IEnumerable<T> orig;
            private readonly int bufferSize;
            public ChunkBuffer(IEnumerable<T> orig, int bufferSize)
            {
                this.orig = orig;
                this.bufferSize = bufferSize;
            }

            [凾(256)]
            public IEnumerator<T[]> GetEnumerator()
            {
                int index = 0;
                T[] arr = new T[bufferSize];
                foreach (var v in orig)
                {
                    arr[index++] = v;
                    if (index == bufferSize)
                    {
                        yield return arr;
                        arr = new T[bufferSize];
                        index = 0;
                    }
                }
                if (index > 0)
                {
                    Array.Resize(ref arr, index);
                    yield return arr;
                }
            }

            IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
        }
    }
}
