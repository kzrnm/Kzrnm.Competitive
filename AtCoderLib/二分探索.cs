using System;
using System.Collections.Generic;

#region https://qiita.com/drken/items/97e37dd6143e33a64c8c
#endregion
static class 二分探索
{
    public static int BinarySearch(int ok, int ng, Predicate<int> isOK)
    {
        while (Math.Abs(ok - ng) > 1)
        {
            var m = (ok + ng) / 2;
            if (isOK(m)) ok = m;
            else ng = m;
        }
        return ok;
    }



    /* https://bitbucket.org/camypaper/complib/src/master/lib/Algorithms/BinarySearch.cs */
    /**
     * <summary>
     * 与えられた比較関数に従って，<paramref name="a"/> の要素のうち，<paramref name="v"/> 以上の要素であるような最小のインデックスを取得します．
     * </summary>
     * <typeparam name="T"><see cref="IList{T}"/> の要素の型を指定します．</typeparam>
     * <param name="a">対象となるコレクション</param>
     * <param name="v">対象となる要素</param>
     * <param name="f"></param>
     * <returns><paramref name="v"/> 以上の要素であるような最小の o-indexed でのインデックス．</returns>
     * <remarks> <paramref name="a"/> は比較関数に対して昇順であることを仮定しています．この関数は O(log N) で実行されます．</remarks>
    */
    public static int LowerBound<T>(this IList<T> a, T v, Comparison<T> f) => BinarySearch(a, v, Comparer<T>.Create(f), true);

    /**
     * <summary>
     *　デフォルトの比較関数に従って，<paramref name="a"/> の要素のうち，<paramref name="v"/> 以上の要素であるような最小のインデックスを取得します．
     * </summary>
     * <typeparam name="T"><see cref="IList{T}"/> の要素の型を指定します．</typeparam>
     * <param name="a">対象となるコレクション</param>
     * <param name="v">対象となる要素</param>
     * <param name="f"></param>
     * <returns><paramref name="v"/> 以上の要素であるような最小の o-indexed でのインデックス．</returns>
     * <remarks> <paramref name="a"/> は比較関数に対して昇順であることを仮定しています．この関数は O(log N) で実行されます．</remarks>
    */
    public static int LowerBound<T>(this IList<T> a, T v) => BinarySearch(a, v, Comparer<T>.Default, true);

    /**
     * <summary>
     * 与えられた比較関数に従って，<paramref name="a"/> の要素のうち，<paramref name="v"/> より真に大きい要素が現れる最小のインデックスを取得します．
     * </summary>
     * <typeparam name="T"><see cref="IList{T}"/> の要素の型を指定します．</typeparam>
     * <param name="a">対象となるコレクション</param>
     * <param name="v">対象となる要素</param>
     * <param name="f"></param>
     * <returns><paramref name="v"/> 以上の要素であるような最小の o-indexed でのインデックス．</returns>
     * <remarks> <paramref name="a"/> は比較関数に対して昇順であることを仮定しています．この関数は O(log N) で実行されます．</remarks>
    */
    public static int UpperBound<T>(this IList<T> a, T v, Comparison<T> cmp) => BinarySearch(a, v, Comparer<T>.Create(cmp), false);

    /**
     * <summary>
     *　デフォルトの比較関数に従って，<paramref name="a"/> の要素のうち，<paramref name="v"/> より真に大きい要素が現れる最小のインデックスを取得します．
     * </summary>
     * <typeparam name="T"><see cref="IList{T}"/> の要素の型を指定します．</typeparam>
     * <param name="a">対象となるコレクション</param>
     * <param name="v">対象となる要素</param>
     * <param name="f"></param>
     * <returns><paramref name="v"/> 以上の要素であるような最小の o-indexed でのインデックス．</returns>
     * <remarks> <paramref name="a"/> は比較関数に対して昇順であることを仮定しています．この関数は O(log N) で実行されます．</remarks>
    */
    public static int UpperBound<T>(this IList<T> a, T v) => BinarySearch(a, v, Comparer<T>.Default, false);

    private static int BinarySearch<T>(IList<T> a, T v, IComparer<T> cmp, bool isLowerBound)
    {
        int ok = a.Count;
        int ng = -1;
        while (ok - ng > 1)
        {
            var m = (ok + ng) / 2;
            var c = cmp.Compare(a[m], v);
            if (c > 0 || (c == 0 && isLowerBound)) ok = m;
            else ng = m;
        }
        return ok;
    }
}
