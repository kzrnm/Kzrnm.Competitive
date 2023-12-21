using Kzrnm.Competitive.IO;

namespace Kzrnm.Competitive
{
    public static class __Console__GraphBuilder_Index
    {
        public static GraphBuilder<int> GraphWithEdgeIndex(this ConsoleReader cr, int count, int edgeCount, bool isDirected, int based = 1)
        {
            var gb = new GraphBuilder<int>(count, isDirected);
            for (var i = 0; i < edgeCount; i++)
                gb.Add(cr.Int() - based, cr.Int() - based, i);
            return gb;
        }
    }
}
