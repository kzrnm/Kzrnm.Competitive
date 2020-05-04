using System;
using IEnumerable = System.Collections.IEnumerable;
using IEnumerator = System.Collections.IEnumerator;
using System.Collections.Generic;
using AtCoderProject;

[System.Diagnostics.DebuggerTypeProxy(typeof(ΔDebugView<>))]
[System.Diagnostics.DebuggerDisplay("Count = {" + nameof(Count) + "}")]
class SortedCollection<T> : IList<T>
{
    List<T> list;

    public SortedCollection() : this(Comparer<T>.Default) { }
    public SortedCollection(int capacity) : this(capacity, Comparer<T>.Default) { }
    public SortedCollection(IComparer<T> comparer) { this.Comparer = comparer; list = new List<T>(); }
    public SortedCollection(int capacity, IComparer<T> comparer) { this.Comparer = comparer; list = new List<T>(capacity); }

    public IComparer<T> Comparer { get; }
    public bool IsReadOnly => false;
    public int Count => list.Count;

    public T this[int index]
    {
        get => list[index];
        set => list[index] = value;
    }


    /**
         <summary>
         与えられた比較関数に従って，<paramref name="item"/> であるような最小のインデックスを取得します←ほんとに？．見つからなかった場合は<paramref name="item"/>より大きい最小のインデックスのビット反転を返します.
         </summary>
         <typeparam name="T"><see cref="IList{T}"/> の要素の型を指定します．</typeparam>
         <param name="item">対象となる要素</param>
         <returns><paramref name="item"/> が見つかった場合は0-indexed でのインデックス．見つからなかった場合は<paramref name="item"/>より大きい最小のインデックスのビット反転.</returns>
         <remarks> 比較関数に対して昇順であることを仮定しています．この関数は O(log N) で実行されます．</remarks>
    */
    protected int BinarySearch(T item) => list.BinarySearch(item, this.Comparer);

    private int BinarySearchImpl(T item, bool isLowerBound)
    {
        var l = 0;
        var r = this.Count - 1;
        while (l <= r)
        {
            var m = (l + r) >> 1;
            var res = this.Comparer.Compare(this[m], item);
            if (res < 0 || (res == 0 && !isLowerBound)) l = m + 1;
            else r = m - 1;
        }
        return l;
    }
    /**
    <summary>
    デフォルトの比較関数に従って，<paramref name="a"/> の要素のうち，<paramref name="item"/> 以上の要素であるような最小のインデックスを取得します．
    </summary>
    <typeparam name="T"><see cref="IList{T}"/> の要素の型を指定します．</typeparam>
    <param name="a">対象となるコレクション</param>
    <param name="item">対象となる要素</param>
    <param name="f"></param>
    <returns><paramref name="item"/> 以上の要素であるような最小の o-indexed でのインデックス．</returns>
    <remarks> <paramref name="a"/> は比較関数に対して昇順であることを仮定しています．この関数は O(log N) で実行されます．</remarks>
    */
    public int LowerBound(T item) => BinarySearchImpl(item, true);

    /**
    <summary>
    デフォルトの比較関数に従って，<paramref name="a"/> の要素のうち，<paramref name="v"/> より真に大きい要素が現れる最小のインデックスを取得します．
    </summary>
    <typeparam name="T"><see cref="IList{T}"/> の要素の型を指定します．</typeparam>
    <param name="a">対象となるコレクション</param>
    <param name="v">対象となる要素</param>
    <param name="f"></param>
    <returns><paramref name="v"/> 以上の要素であるような最小の o-indexed でのインデックス．</returns>
    <remarks> <paramref name="a"/> は比較関数に対して昇順であることを仮定しています．この関数は O(log N) で実行されます．</remarks>
    */
    public int UpperBound(T item) => BinarySearchImpl(item, false);

    void ICollection<T>.Add(T item) => this.Add(item);
    public int Add(T item)
    {
        var index = BinarySearch(item);
        if (index < 0)
            index = ~index;
        list.Insert(index, item);
        return index;
    }

    public int IndexOf(T item)
    {
        var index = BinarySearch(item);
        if (index >= 0) return index;
        else return -1;
    }
    public bool Contains(T item) => BinarySearch(item) >= 0;
    public void Clear() => list.Clear();
    public void CopyTo(T[] array, int arrayIndex) => list.CopyTo(array, arrayIndex);

    public bool Remove(T item)
    {
        int index = this.IndexOf(item);
        if (index < 0)
            return false;
        list.RemoveAt(index);
        return true;
    }

    IEnumerator IEnumerable.GetEnumerator() => this.GetEnumerator();
    public IEnumerator<T> GetEnumerator() => list.GetEnumerator();
    public void Insert(int index, T item) => throw new NotImplementedException();
    public void RemoveAt(int index) => list.RemoveAt(index);
}


[System.Diagnostics.DebuggerTypeProxy(typeof(ΔDebugView<>))]
[System.Diagnostics.DebuggerDisplay("Count = {" + nameof(Count) + "}")]
class SortedUniqueCollection<T> : IList<T>
{
    List<T> list;

    public SortedUniqueCollection() : this(Comparer<T>.Default) { }
    public SortedUniqueCollection(int capacity) : this(capacity, Comparer<T>.Default) { }
    public SortedUniqueCollection(IComparer<T> comparer) { this.Comparer = comparer; list = new List<T>(); }
    public SortedUniqueCollection(int capacity, IComparer<T> comparer) { this.Comparer = comparer; list = new List<T>(capacity); }

    public IComparer<T> Comparer { get; }
    public bool IsReadOnly => false;
    public int Count => list.Count;

    public T this[int index]
    {
        get => list[index];
        set => list[index] = value;
    }

    /**
    <summary>
    与えられた比較関数に従って，<paramref name="item"/> であるような最小のインデックスを取得します←ほんとに？．見つからなかった場合は<paramref name="item"/>より大きい最小のインデックスのビット反転を返します.
    </summary>
    <typeparam name="T"><see cref="IList{T}"/> の要素の型を指定します．</typeparam>
    <param name="item">対象となる要素</param>
    <returns><paramref name="item"/> が見つかった場合は0-indexed でのインデックス．見つからなかった場合は<paramref name="item"/>より大きい最小のインデックスのビット反転.</returns>
    <remarks> 比較関数に対して昇順であることを仮定しています．この関数は O(log N) で実行されます．</remarks>
    */
    protected int BinarySearch(T item) => list.BinarySearch(item, this.Comparer);

    void ICollection<T>.Add(T item) => this.Add(item);
    public int Add(T item)
    {
        var index = BinarySearch(item);
        if (index < 0)
            index = ~index;
        else
            return -1;
        list.Insert(index, item);
        return index;
    }

    public int IndexOf(T item)
    {
        var index = BinarySearch(item);
        if (index >= 0) return index;
        else return -1;
    }
    public bool Contains(T item) => BinarySearch(item) >= 0;
    public void Clear() => list.Clear();
    public void CopyTo(T[] array, int arrayIndex) => list.CopyTo(array, arrayIndex);

    public bool Remove(T item)
    {
        int index = this.IndexOf(item);
        if (index < 0)
            return false;
        list.RemoveAt(index);
        return true;
    }

    IEnumerator IEnumerable.GetEnumerator() => this.GetEnumerator();
    public IEnumerator<T> GetEnumerator() => list.GetEnumerator();
    public void Insert(int index, T item) => throw new NotImplementedException();
    public void RemoveAt(int index) => list.RemoveAt(index);
}
