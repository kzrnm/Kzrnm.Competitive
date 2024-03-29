using Kzrnm.Competitive.IO;

namespace Kzrnm.Competitive
{
    public static class __Console__GraphBuilder
    {
        public static GraphBuilder Graph(this ConsoleReader cr, int count, int edgeCount, bool isDirected, int based = 1)
        {
            var gb = new GraphBuilder(count, isDirected);
            for (var i = 0; i < edgeCount; i++)
                gb.Add(cr - based, cr - based);
            return gb;
        }
        public static GraphBuilder Tree(this ConsoleReader cr, int count, int based = 1)
        {
            var gb = new GraphBuilder(count, false);
            for (var i = 1; i < count; i++)
                gb.Add(cr - based, cr - based);
            return gb;
        }
        /// <summary>
        ///  <paramref name="cr"/> から読み取ります。
        /// </summary>
        /// <example>
        /// <paramref name="based"/> が 1 のとき、1 2 2 3 という入力からは、1→2, 2→3, 2→4, 3→5 という辺を生成します。
        /// </example>
        public static GraphBuilder TreeParent(this ConsoleReader cr, int count, int based = 1)
        {
            var gb = new GraphBuilder(count, false);
            for (var i = 1; i < count; i++)
                gb.Add(i, cr - based);
            return gb;
        }
    }
}
