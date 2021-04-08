using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using Kzrnm.Competitive.Linq;

namespace Kzrnm.Competitive
{
    public static class __CollectionExtension_Buffer
    {
        /// <summary>
        /// コレクションの要素をいくつかの要素ごとに分割したバッファーにする。最後の要素は数が足りない可能性あり
        /// </summary>
        public static Buffer<T> Buffered<T>(this IEnumerable<T> collection, int bufferSize) => new Buffer<T>(collection, bufferSize);
    }
    namespace Linq
    {
        public class Buffer<T> : IEnumerable<T[]>
        {
            private readonly IEnumerable<T> orig;
            private readonly int bufferSize;
            public Buffer(IEnumerable<T> orig, int bufferSize)
            {
                this.orig = orig;
                this.bufferSize = bufferSize;
            }

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

            IEnumerator IEnumerable.GetEnumerator() => this.GetEnumerator();
        }
    }
}
