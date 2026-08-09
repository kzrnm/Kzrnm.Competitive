using System;
using System.Diagnostics;
using 凾 = System.Runtime.CompilerServices.MethodImplAttribute;

namespace Kzrnm.Competitive.Internal
{
    public sealed class StructPool<T> where T : struct
    {
        private StructPool(int size)
        {
            _a = GC.AllocateUninitializedArray<T>(size);
            _c = new int[size];
            var s = GC.AllocateUninitializedArray<int>(size);
            for (int i = s.Length - 1; i >= 0; i--)
                s[i] = i;
            _s = s;
            _si = size;
        }
        public readonly static StructPool<T> Default = new(8);
        /// <summary>
        /// 構造体を確保する配列
        /// </summary>
        T[] _a;
        /// <summary>
        /// _a の参照カウンタ
        /// </summary>
        int[] _c;

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
            Array.Resize(ref _c, a.Length);

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
        public int Rent()
        {
            if (_si <= 0)
                Grow();

            var ix = _s[--_si];
            ++_c[ix];
            return ix;
        }
        [凾(256)]
        public int Rent(int ix)
        {
            Debug.Assert(_c[ix] > 0);
            ++_c[ix];
            return ix;
        }
        [凾(256)]
        public void Return(int ix)
        {
            Debug.Assert(_c[ix] > 0);
            if (--_c[ix] == 0)
            {
                if (_si >= _s.Length)
                {
                    var s = GC.AllocateUninitializedArray<int>(_a.Length);
                    _s.AsSpan().CopyTo(s);
                    _s = s;
                }
                _s[_si++] = ix;
            }
        }
    }
}
