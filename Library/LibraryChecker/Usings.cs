#if NET7_0_OR_GREATER
global using LongFenwickTree = AtCoder.FenwickTree<long>;
global using Mod998244353AffineTransformation = Kzrnm.Competitive.AffineTransformation<AtCoder.StaticModInt<AtCoder.Mod998244353>>;
global using Mod998244353ArrayMatrix = Kzrnm.Competitive.ArrayMatrix<AtCoder.StaticModInt<AtCoder.Mod998244353>>;
global using LongSums = Kzrnm.Competitive.Sums<long>;
global using LongWaveletMatrix2DWithFenwickTree = Kzrnm.Competitive.WaveletMatrix2DWithFenwickTree<int, long>;
global using LongWaveletMatrix2DWithSums = Kzrnm.Competitive.WaveletMatrix2DWithSums<int, long>;
global using WLongGraphBuilder = Kzrnm.Competitive.WGraphBuilder<long>;
global using WULongGraphBuilder = Kzrnm.Competitive.WGraphBuilder<ulong>;
#else
global using Mod998244353AffineTransformation = Kzrnm.Competitive.AffineTransformation<AtCoder.StaticModInt<AtCoder.Mod998244353>, AtCoder.StaticModIntOperator<AtCoder.Mod998244353>>;
global using Mod998244353ArrayMatrix = Kzrnm.Competitive.ArrayMatrix<AtCoder.StaticModInt<AtCoder.Mod998244353>, AtCoder.StaticModIntOperator<AtCoder.Mod998244353>>;
global using LongWaveletMatrix2DWithFenwickTree = Kzrnm.Competitive.WaveletMatrix2DWithFenwickTree<int, long, AtCoder.LongOperator>;
global using LongWaveletMatrix2DWithSums = Kzrnm.Competitive.WaveletMatrix2DWithSums<int, long, AtCoder.LongOperator>;
#endif