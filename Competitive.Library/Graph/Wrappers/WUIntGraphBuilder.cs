using AtCoder;
using Kzrnm.Competitive.IO;

namespace Kzrnm.Competitive
{
    public class WUIntGraphBuilder : WGraphBuilder<uint, UIntOperator>
    {
        public WUIntGraphBuilder(int count, bool isDirected) : base(count, isDirected) { }
        public static WUIntGraphBuilder Create(int count, PropertyConsoleReader cr, int edgeCount, bool isDirected)
        {
            var gb = new WUIntGraphBuilder(count, isDirected);
            for (var i = 0; i < edgeCount; i++)
                gb.Add(cr.Int0, cr.Int0, (uint)cr.ULong);
            return gb;
        }
        public static WUIntGraphBuilder CreateTree(int count, PropertyConsoleReader cr)
        {
            var gb = new WUIntGraphBuilder(count, false);
            for (var i = 1; i < count; i++)
                gb.Add(cr.Int0, cr.Int0, (uint)cr.ULong);
            return gb;
        }
    }
}
