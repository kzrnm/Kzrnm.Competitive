using AtCoder;
using System;
using System.Linq;
using System.Runtime.CompilerServices;
using 凾 = System.Runtime.CompilerServices.MethodImplAttribute;

namespace Kzrnm.Competitive.Internal
{
    public sealed class StructPool<T> where T : struct
    {
        private StructPool(int size)
        {
            _a = GC.AllocateUninitializedArray<T>(size);
            _s = Enumerable.Range(0, size).ToArray();
            _si = size;
        }
        public readonly static StructPool<T> Default = new(
#if SOURCE_EMBEDDING || !DEBUG
            1 << 12
#else
            4
#endif
            );
        /// <summary>
        /// 構造体を確保する配列
        /// </summary>
        T[] _a;
        /// <summary>
        /// 未使用のインデックスを保持する配列
        /// </summary>
        int[] _s;
        /// <summary>
        /// 未使用のインデックスのサイズ。<see cref="Span{T}"/> のように扱いたい。
        /// </summary>
        int _si;

        /// <summary>
        /// 配列を拡張する
        /// </summary>
        [凾(256)]
        void Grow()
        {
            var a = GC.AllocateUninitializedArray<T>(_a.Length * 2);
            _a.AsSpan().CopyTo(a);

            var ssi = _si + _a.Length;
            if (ssi >= _s.Length)
            {
                var s = GC.AllocateUninitializedArray<int>(a.Length);
                _s.AsSpan(0, _si).CopyTo(s);
                _s = s;
            }
            var t = _s.AsSpan(_si, _a.Length);
            for (int i = 0; i < t.Length; i++)
                t[i] = _a.Length + i;

            _si = ssi;
            _a = a;
        }


        [凾(256)]
        public ref T Get(int i) => ref _a[i];

        [凾(256)]
        public ref T Rent(out int i)
        {
            if (_si <= 0)
                Grow();

            i = _s[--_si];
            return ref _a[i];
        }
        [凾(256)]
        public void Return(int ix)
        {
            if (_si >= _s.Length)
            {
                var s = GC.AllocateUninitializedArray<int>(_a.Length);
                _s.AsSpan().CopyTo(s);
                _s = s;
            }
            _s[_si++] = ix;
        }

        /// <summary>
        /// デバッグ用に中身を削除する。
        /// </summary>
        [SourceExpander.NotEmbeddingSource]
        public void Clear(int size = 4)
        {
            _a = GC.AllocateUninitializedArray<T>(size);
            _s = Enumerable.Range(0, size).ToArray();
            _si = size;
        }
    }

    /// <summary>
    /// プールオブジェクトのジェネリック操作
    /// </summary>
    /// <typeparam name="T">オブジェクト本体</typeparam>
    /// <typeparam name="R">ノード参照</typeparam>
    [IsOperator]
    public interface IPoolRefOp<T, R>
    {
        static abstract R Null { get; }
        static abstract bool IsNull(R t);
        /// <summary>
        /// 参照を取得します。プールが更新されると参照が途切れるため扱いに注意。
        /// </summary>
        /// <remarks>
        /// <code>C.Load(t).Left = N.AnyOperate(t);</code> みたいなことをやるときは注意。左辺が先に実行されるので代入先が古い可能性がある。
        /// 特に Immutable な場合は Get のつもりでもノードが生成されたりするので要注意。
        /// </remarks>
        static abstract ref T Load(in R t);
        /// <summary>
        /// <paramref name="t"/> を解放します。
        /// </summary>
        static virtual void Free(R t) { }
    }

    public struct PoolClassRefOp<T> : IPoolRefOp<T, T>
        where T : class
    {
        public static T Null => null;
        [凾(256)] public static bool IsNull(T t) => t is null;
        [凾(256)] public static ref T Load(in T t) => ref Unsafe.AsRef(t);
    }

    public struct PoolStructRefOp<T> : IPoolRefOp<T, int>
        where T : struct
    {
        public static int Null => -1;
        [凾(256)] public static bool IsNull(int t) => t < 0;
        [凾(256)] public static ref T Load(in int t) => ref StructPool<T>.Default.Get(t);
        [凾(256)] public static void Free(int t) => StructPool<T>.Default.Return(t);
    }
}
