using Kzrnm.Competitive.IO;
using System.Numerics;

namespace Kzrnm.Competitive
{
    public static class __Console__WGraphBuilder_Index
    {
        public static WGraphBuilder<T, int> GraphWithEdgeIndex<T>(this ConsoleReader cr, int count, int edgeCount, bool isDirected, int based = 1)
             where T : IAdditionOperators<T, T, T>
        {
            var gb = new WGraphBuilder<T, int>(count, isDirected);
            for (var i = 0; i < edgeCount; i++)
                gb.Add(cr.Int() - based, cr.Int() - based, cr.Read<T>(), i);
            return gb;
        }
    }
}
