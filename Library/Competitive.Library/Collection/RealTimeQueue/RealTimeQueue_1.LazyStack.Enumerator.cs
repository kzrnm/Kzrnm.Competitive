// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using 凾 = System.Runtime.CompilerServices.MethodImplAttribute;

namespace Kzrnm.Competitive
{
    public sealed partial class RealTimeQueue<T>
    {
        /// <summary>
        /// Enumerates a stack with no memory allocations.
        /// </summary>
        [EditorBrowsable(EditorBrowsableState.Advanced)]
        internal struct LazyStackEnumerator
        {
            /// <summary>
            /// The original stack being enumerated.
            /// </summary>
            private readonly LazyStack _originalStack;

            /// <summary>
            /// The remaining stack not yet enumerated.
            /// </summary>
            private LazyStack _remainingStack;

            /// <summary>
            /// Initializes a new instance of the <see cref="LazyStackEnumerator"/> struct.
            /// </summary>
            /// <param name="stack">The stack to enumerator.</param>
            internal LazyStackEnumerator(LazyStack stack)
            {

                _originalStack = stack;
                _remainingStack = null;
            }

            /// <summary>
            /// Gets the current element.
            /// </summary>
            public T Current
            {
                [凾(256)]
                get
                {
                    if (_remainingStack == null || _remainingStack.IsEmpty)
                    {
                        return ThrowInvalidOperationException();
                    }
                    else
                    {
                        return _remainingStack.Peek();
                    }
                }
            }

            /// <summary>
            /// Moves to the first or next element.
            /// </summary>
            /// <returns>A value indicating whether there are any more elements.</returns>
            [凾(256)]
            public bool MoveNext()
            {
                if (_remainingStack == null)
                {
                    // initial move
                    _remainingStack = _originalStack;
                }
                else if (!_remainingStack.IsEmpty)
                {
                    _remainingStack = _remainingStack.Pop();
                }

                return !_remainingStack.IsEmpty;
            }
        }

        /// <summary>
        /// Enumerates a stack with no memory allocations.
        /// </summary>
        private sealed class LazyStackEnumeratorObject : IEnumerator<T>
        {
            /// <summary>
            /// The original stack being enumerated.
            /// </summary>
            private readonly LazyStack _originalStack;

            /// <summary>
            /// The remaining stack not yet enumerated.
            /// </summary>
            private LazyStack _remainingStack;

            /// <summary>
            /// A flag indicating whether this enumerator has been disposed.
            /// </summary>
            private bool _disposed;

            /// <summary>
            /// Initializes a new instance of the <see cref="LazyStackEnumeratorObject"/> class.
            /// </summary>
            /// <param name="stack">The stack to enumerator.</param>
            internal LazyStackEnumeratorObject(LazyStack stack)
            {

                _originalStack = stack;
            }

            /// <summary>
            /// Gets the current element.
            /// </summary>
            public T Current
            {
                [凾(256)]
                get
                {
                    this.ThrowIfDisposed();
                    if (_remainingStack == null || _remainingStack.IsEmpty)
                    {
                        return ThrowInvalidOperationException();
                    }
                    else
                    {
                        return _remainingStack.Peek();
                    }
                }
            }

            /// <summary>
            /// Gets the current element.
            /// </summary>
            object IEnumerator.Current
            {
                get { return this.Current; }
            }


            /// <summary>
            /// Moves to the first or next element.
            /// </summary>
            /// <returns>A value indicating whether there are any more elements.</returns>
            [凾(256)]
            public bool MoveNext()
            {
                this.ThrowIfDisposed();

                if (_remainingStack == null)
                {
                    // initial move
                    _remainingStack = _originalStack;
                }
                else if (!_remainingStack.IsEmpty)
                {
                    _remainingStack = _remainingStack.Pop();
                }

                return !_remainingStack.IsEmpty;
            }

            /// <summary>
            /// Resets the position to just before the first element in the list.
            /// </summary>
            public void Reset()
            {
                this.ThrowIfDisposed();
                _remainingStack = null;
            }

            /// <summary>
            /// Disposes this instance.
            /// </summary>
            public void Dispose()
            {
                _disposed = true;
            }

            /// <summary>
            /// Throws an <see cref="ObjectDisposedException"/> if this
            /// enumerator has already been disposed.
            /// </summary>
            private void ThrowIfDisposed()
            {
                if (_disposed)
                {

                }
            }
        }
    }
}
