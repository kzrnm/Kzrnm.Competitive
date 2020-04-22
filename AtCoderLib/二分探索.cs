using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace AtCoderProject.Hide
{
    // https://bitbucket.org/camypaper/complib/src/master/lib/Algorithms/BinarySearch.cs

    static class 二分探索
    {
        /// <summary>
        /// 与えられた<paramref name="predicate"/> の結果がtrueであるような最大の数値を二分法で取得します．
        /// </summary>
        /// <param name="l">最小値</param>
        /// <param name="r">最大値</param>
        /// <param name="predicate">判定条件</param>
        /// <returns><paramref name="predicate"/> がtrueを返す最大の o-indexed でのインデックス．</returns>
        /// <remarks> <paramref name="predicate"/> はある範囲以下の全ての数でtrueとなりある範囲以上でfalseとなると仮定します．この関数は O(log N) で実行されます．</remarks>
        static int Bisection(int l, int r, Predicate<int> predicate)
        {
            while (l < r)
            {
                var m = (l + r + 1) >> 1;
                if (predicate(m)) l = m;
                else r = m - 1;
            }
            return l;
        }


        static int BinarySearch<T>(IList<T> a, T v, IComparer<T> cmp, bool isLowerBound)
        {
            var l = 0;
            var r = a.Count - 1;
            while (l <= r)
            {
                var m = (l + r) >> 1;
                var res = cmp.Compare(a[m], v);
                if (res < 0 || (res == 0 && !isLowerBound)) l = m + 1;
                else r = m - 1;
            }
            return l;
        }

        /// <summary>
        /// 与えられた比較関数に従って，<paramref name="a"/> の要素のうち，<paramref name="v"/> 以上の要素であるような最小のインデックスを取得します．
        /// </summary>
        /// <typeparam name="T"><see cref="IList{T}"/> の要素の型を指定します．</typeparam>
        /// <param name="a">対象となるコレクション</param>
        /// <param name="v">対象となる要素</param>
        /// <param name="f"></param>
        /// <returns><paramref name="v"/> 以上の要素であるような最小の o-indexed でのインデックス．</returns>
        /// <remarks> <paramref name="a"/> は比較関数に対して昇順であることを仮定しています．この関数は O(log N) で実行されます．</remarks>
        public static int LowerBound<T>(this IList<T> a, T v, Comparison<T> f) => BinarySearch(a, v, Comparer<T>.Create(f), true);

        /// <summary>
        ///　デフォルトの比較関数に従って，<paramref name="a"/> の要素のうち，<paramref name="v"/> 以上の要素であるような最小のインデックスを取得します．
        /// </summary>
        /// <typeparam name="T"><see cref="IList{T}"/> の要素の型を指定します．</typeparam>
        /// <param name="a">対象となるコレクション</param>
        /// <param name="v">対象となる要素</param>
        /// <param name="f"></param>
        /// <returns><paramref name="v"/> 以上の要素であるような最小の o-indexed でのインデックス．</returns>
        /// <remarks> <paramref name="a"/> は比較関数に対して昇順であることを仮定しています．この関数は O(log N) で実行されます．</remarks>
        public static int LowerBound<T>(this IList<T> a, T v) => BinarySearch(a, v, Comparer<T>.Default, true);

        /// <summary>
        /// 与えられた比較関数に従って，<paramref name="a"/> の要素のうち，<paramref name="v"/> より真に大きい要素が現れる最小のインデックスを取得します．
        /// </summary>
        /// <typeparam name="T"><see cref="IList{T}"/> の要素の型を指定します．</typeparam>
        /// <param name="a">対象となるコレクション</param>
        /// <param name="v">対象となる要素</param>
        /// <param name="f"></param>
        /// <returns><paramref name="v"/> 以上の要素であるような最小の o-indexed でのインデックス．</returns>
        /// <remarks> <paramref name="a"/> は比較関数に対して昇順であることを仮定しています．この関数は O(log N) で実行されます．</remarks>
        public static int UpperBound<T>(this IList<T> a, T v, Comparison<T> cmp) => BinarySearch(a, v, Comparer<T>.Create(cmp), false);

        /// <summary>
        ///　デフォルトの比較関数に従って，<paramref name="a"/> の要素のうち，<paramref name="v"/> より真に大きい要素が現れる最小のインデックスを取得します．
        /// </summary>
        /// <typeparam name="T"><see cref="IList{T}"/> の要素の型を指定します．</typeparam>
        /// <param name="a">対象となるコレクション</param>
        /// <param name="v">対象となる要素</param>
        /// <param name="f"></param>
        /// <returns><paramref name="v"/> 以上の要素であるような最小の o-indexed でのインデックス．</returns>
        /// <remarks> <paramref name="a"/> は比較関数に対して昇順であることを仮定しています．この関数は O(log N) で実行されます．</remarks>
        public static int UpperBound<T>(this IList<T> a, T v) => BinarySearch(a, v, Comparer<T>.Default, false);
    }

}
