// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using 凾 = System.Runtime.CompilerServices.MethodImplAttribute;

namespace Kzrnm.Competitive
{
    public sealed partial class BankersQueue<T> : IImmutableQueue<T>
    {
        /// <summary>
        /// A memory allocation-free enumerator of <see cref="BankersQueue{T}"/>.
        /// </summary>
        [EditorBrowsable(EditorBrowsableState.Advanced)]
        public struct Enumerator
        {
            /// <summary>
            /// The original queue being enumerated.
            /// </summary>
            private readonly BankersQueue<T> _originalQueue;

            /// <summary>
            /// The remaining forwards stack of the queue being enumerated.
            /// </summary>
            private IImmutableStack<T> _remainingForwardsStack;

            /// <summary>
            /// The remaining backwards stack of the queue being enumerated.
            /// Its order is reversed when the field is first initialized.
            /// </summary>
            private IImmutableStack<T> _remainingBackwardsStack;

            /// <summary>
            /// Initializes a new instance of the <see cref="Enumerator"/> struct.
            /// </summary>
            /// <param name="queue">The queue to enumerate.</param>
            internal Enumerator(BankersQueue<T> queue)
            {
                _originalQueue = queue;

                // The first call to MoveNext will initialize these.
                _remainingForwardsStack = null;
                _remainingBackwardsStack = null;
            }

            /// <summary>
            /// The current element.
            /// </summary>
            public T Current
            {
                get
                {
                    if (_remainingForwardsStack == null)
                    {
                        // The initial call to MoveNext has not yet been made.
                        throw new InvalidOperationException();
                    }

                    if (!_remainingForwardsStack.IsEmpty)
                    {
                        return _remainingForwardsStack.Peek();
                    }
                    else if (!_remainingBackwardsStack!.IsEmpty)
                    {
                        return _remainingBackwardsStack.Peek();
                    }
                    else
                    {
                        // We've advanced beyond the end of the queue.
                        throw new InvalidOperationException();
                    }
                }
            }

            /// <summary>
            /// Advances enumeration to the next element.
            /// </summary>
            /// <returns>A value indicating whether there is another element in the enumeration.</returns>
            public bool MoveNext()
            {
                if (_remainingForwardsStack == null)
                {
                    // This is the initial step.
                    // Empty queues have no forwards or backwards
                    _originalQueue.Normalize(true);
                    _remainingForwardsStack = _originalQueue._forwards;
                    _remainingBackwardsStack = _originalQueue._backwards;
                }
                else if (!_remainingForwardsStack.IsEmpty)
                {
                    _remainingForwardsStack = _remainingForwardsStack.Pop();
                }
                else if (!_remainingBackwardsStack!.IsEmpty)
                {
                    _remainingBackwardsStack = _remainingBackwardsStack.Pop();
                }

                return !_remainingForwardsStack.IsEmpty || !_remainingBackwardsStack!.IsEmpty;
            }
        }

        /// <summary>
        /// A memory allocation-free enumerator of <see cref="BankersQueue{T}"/>.
        /// </summary>
        private sealed class EnumeratorObject : IEnumerator<T>
        {
            /// <summary>
            /// The original queue being enumerated.
            /// </summary>
            private readonly BankersQueue<T> _originalQueue;

            /// <summary>
            /// The remaining forwards stack of the queue being enumerated.
            /// </summary>
            private IImmutableStack<T> _remainingForwardsStack;

            /// <summary>
            /// The remaining backwards stack of the queue being enumerated.
            /// Its order is reversed when the field is first initialized.
            /// </summary>
            private IImmutableStack<T> _remainingBackwardsStack;

            /// <summary>
            /// A value indicating whether this enumerator has been disposed.
            /// </summary>
            private bool _disposed;

            /// <summary>
            /// Initializes a new instance of the <see cref="Enumerator"/> struct.
            /// </summary>
            /// <param name="queue">The queue to enumerate.</param>
            internal EnumeratorObject(BankersQueue<T> queue)
            {
                _originalQueue = queue;
            }

            /// <summary>
            /// The current element.
            /// </summary>
            public T Current
            {
                get
                {
                    this.ThrowIfDisposed();
                    if (_remainingForwardsStack == null)
                    {
                        // The initial call to MoveNext has not yet been made.
                        throw new InvalidOperationException();
                    }

                    if (!_remainingForwardsStack.IsEmpty)
                    {
                        return _remainingForwardsStack.Peek();
                    }
                    else if (!_remainingBackwardsStack!.IsEmpty)
                    {
                        return _remainingBackwardsStack.Peek();
                    }
                    else
                    {
                        // We've advanced beyond the end of the queue.
                        throw new InvalidOperationException();
                    }
                }
            }

            /// <summary>
            /// The current element.
            /// </summary>
            object IEnumerator.Current
            {
                get { return this.Current!; }
            }

            /// <summary>
            /// Advances enumeration to the next element.
            /// </summary>
            /// <returns>A value indicating whether there is another element in the enumeration.</returns>
            public bool MoveNext()
            {
                this.ThrowIfDisposed();
                if (_remainingForwardsStack == null)
                {
                    // This is the initial step.
                    // Empty queues have no forwards or backwards
                    _originalQueue.Normalize(true);
                    _remainingForwardsStack = _originalQueue._forwards;
                    _remainingBackwardsStack = _originalQueue._backwards;
                }
                else if (!_remainingForwardsStack.IsEmpty)
                {
                    _remainingForwardsStack = _remainingForwardsStack.Pop();
                }
                else if (!_remainingBackwardsStack!.IsEmpty)
                {
                    _remainingBackwardsStack = _remainingBackwardsStack.Pop();
                }

                return !_remainingForwardsStack.IsEmpty || !_remainingBackwardsStack!.IsEmpty;
            }

            /// <summary>
            /// Restarts enumeration.
            /// </summary>
            public void Reset()
            {
                this.ThrowIfDisposed();
                _remainingBackwardsStack = null;
                _remainingForwardsStack = null;
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
                    throw new ObjectDisposedException("BankersQueue<T>.Enumerator");
                }
            }
        }
    }
}