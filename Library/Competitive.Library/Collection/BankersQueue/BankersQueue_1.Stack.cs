// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
using System.Collections;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Diagnostics;

namespace Kzrnm.Competitive
{
    public sealed partial class BankersQueue<T> : IImmutableQueue<T>
    {
        internal class LazyReverseStack : IImmutableStack<T>
        {
            private readonly IImmutableStack<T> _heads;
            private readonly ImmutableStack<T> _origin;
            private ImmutableStack<T> _reversed;
            public LazyReverseStack(IImmutableStack<T> heads)
            {
                this._heads = heads;
                this._origin = null;
            }
            public LazyReverseStack(IImmutableStack<T> heads, ImmutableStack<T> origin)
            {
                this._heads = heads;
                this._origin = origin;
            }

            public ImmutableStack<T> Reversed
            {
                get
                {
                    // Although this is a lazy-init pattern, no lock is required because
                    // this instance is immutable otherwise, and a double-assignment from multiple
                    // threads is harmless.
                    _reversed ??= Build(_heads, _origin);

                    Debug.Assert(_reversed != null);
                    return _reversed;

                    static ImmutableStack<T> Build(IImmutableStack<T> heads, ImmutableStack<T> s)
                    {
                        if (s == null && heads is ImmutableStack<T> h)
                        {
                            return h;
                        }
                        ImmutableStack<T> r = s.Clear();
                        for (ImmutableStack<T> f = s; !f.IsEmpty; f = f.Pop())
                        {
                            r = r.Push(f.Peek());
                        }
                        for (ImmutableStack<T> f = Reverse(heads); !f.IsEmpty; f = f.Pop())
                        {
                            r = r.Push(f.Peek());
                        }

                        Debug.Assert(r != null);
                        Debug.Assert(r.IsEmpty == (s.IsEmpty && heads.IsEmpty));
                        return r;
                    }

                    static ImmutableStack<T> Reverse(IImmutableStack<T> s)
                    {
                        if (s == null)
                        {
                            return ImmutableStack<T>.Empty;
                        }
                        ImmutableStack<T> r = ImmutableStack<T>.Empty;
                        for (IImmutableStack<T> f = s; !f.IsEmpty; f = f.Pop())
                        {
                            r = r.Push(f.Peek());
                        }

                        Debug.Assert(r != null);
                        Debug.Assert(r.IsEmpty == s.IsEmpty);
                        return r;
                    }
                }
            }

            public bool IsEmpty => _heads.IsEmpty && (_origin?.IsEmpty != false);

            public IImmutableStack<T> Clear()
            {
                return _heads.Clear();
            }

            public IEnumerator<T> GetEnumerator()
            {
                return ((IEnumerable<T>)Reversed).GetEnumerator();
            }

            public T Peek()
            {
                return Reversed.Peek();
            }

            public ref readonly T PeekRef()
            {
                return ref Reversed.PeekRef();
            }

            public IImmutableStack<T> Pop()
            {
                return ((IImmutableStack<T>)Reversed).Pop();
            }

            public IImmutableStack<T> Push(T value)
            {
                return ((IImmutableStack<T>)Reversed).Push(value);
            }

            IEnumerator IEnumerable.GetEnumerator()
            {
                return ((IEnumerable)Reversed).GetEnumerator();
            }
        }
    }
}