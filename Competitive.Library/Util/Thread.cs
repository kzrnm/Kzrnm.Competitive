using System;

namespace AtCoder.Threading
{
    public static class Thread
    {
        /// <summary>
        /// スタックサイズを指定して関数を実行する。Stack overflow を回避できるかも。
        /// </summary>
        /// <typeparam name="T">計算結果</typeparam>
        /// <param name="func">実行する関数</param>
        /// <param name="stackMegaByte">スタックサイズをMBで指定</param>
        public static T LargeStack<T>(Func<T> func, int stackMegaByte)
        {
            T res = default;
            var t = new System.Threading.Thread(() => res = func(), stackMegaByte << 20);
            t.Start();
            t.Join();
            return res;
        }

        /// <summary>
        /// スタックサイズを指定して関数を実行する。Stack overflow を回避できるかも。
        /// </summary>
        /// <param name="func">実行する関数</param>
        /// <param name="stackMegaByte">スタックサイズをMBで指定</param>
        public static void LargeStack(System.Threading.ThreadStart func, int stackMegaByte)
        {
            var t = new System.Threading.Thread(func, stackMegaByte << 20);
            t.Start();
            t.Join();
        }
    }
}
