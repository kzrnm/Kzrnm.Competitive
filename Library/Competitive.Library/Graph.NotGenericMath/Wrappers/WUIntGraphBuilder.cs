using AtCoder;
using Kzrnm.Competitive.IO;

namespace Kzrnm.Competitive
{
    // competitive-verifier: TITLE uint重み付きグラフのBuilder
    public class WUIntGraphBuilder : WGraphBuilder<uint, UIntOperator>
    {
        public WUIntGraphBuilder(int count, bool isDirected) : base(count, isDirected) { }
        public static WUIntGraphBuilder Create(int count, ConsoleReader cr, int edgeCount, bool isDirected)
        {
            var gb = new WUIntGraphBuilder(count, isDirected);
            for (var i = 0; i < edgeCount; i++)
                gb.Add(cr.Int0(), cr.Int0(), cr.UInt());
            return gb;
        }
        public static WGraphBuilder<uint, int, UIntOperator> CreateWithEdgeIndex(int count, ConsoleReader cr, int edgeCount, bool isDirected)
        {
            var gb = new WGraphBuilder<uint, int, UIntOperator>(count, isDirected);
            for (var i = 0; i < edgeCount; i++)
                gb.Add(cr.Int0(), cr.Int0(), cr.UInt(), i);
            return gb;
        }
        public static WUIntGraphBuilder CreateTree(int count, ConsoleReader cr)
        {
            var gb = new WUIntGraphBuilder(count, false);
            for (var i = 1; i < count; i++)
                gb.Add(cr.Int0(), cr.Int0(), cr.UInt());
            return gb;
        }
    }
}
