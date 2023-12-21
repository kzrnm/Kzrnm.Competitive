using Kzrnm.Competitive.IO;
using System.Numerics;

namespace Kzrnm.Competitive
{
    public static class __Console__WGraphBuilder
    {
        public static WGraphBuilder<T> Graph<T>(this ConsoleReader cr, int count, int edgeCount, bool isDirected, int based = 1)
             where T : IAdditionOperators<T, T, T>
        {
            var gb = new WGraphBuilder<T>(count, isDirected);
            for (var i = 0; i < edgeCount; i++)
                gb.Add(cr.Int() - based, cr.Int() - based, cr.Read<T>());
            return gb;
        }
        public static WGraphBuilder<T> Tree<T>(this ConsoleReader cr, int count, int based = 1)
             where T : IAdditionOperators<T, T, T>
        {
            var gb = new WGraphBuilder<T>(count, false);
            for (var i = 1; i < count; i++)
                gb.Add(cr.Int() - based, cr.Int() - based, cr.Read<T>());
            return gb;
        }
    }
}
