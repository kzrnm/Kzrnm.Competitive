using Kzrnm.Competitive.Internal;
using Kzrnm.Competitive.Internal.Bbst;
using System.Diagnostics;
using 凾 = System.Runtime.CompilerServices.MethodImplAttribute;

namespace Kzrnm.Competitive.Testing.Collection.BinarySearchTree;

using ClassNode = LazySplayTreeClassNode<int, int, SumOp>;
using Node = LazySplayTreeNode<int, int, SumOp>;

[InheritsTests]
public class LazySplayTreeClassTests : LazyBinarySearchTreeTestsBase<ClassNode, ClassNode.Op>
{
    protected override LazySplayTreeClass<int, int, SumOp> Create() => new();
    protected override LazySplayTreeClass<int, int, SumOp> Create(IEnumerable<int> values) => new(values);
}


[InheritsTests]
[NotInParallel(nameof(LazySplayTreeTests))]
public class LazySplayTreeTests : LazyBinarySearchTreeTestsBase<int, Node.Op>
{
    protected override void ClearNode() => ClearNode<Node>();
    protected override LazySplayTree<int, int, SumOp> Create() => new();
    protected override LazySplayTree<int, int, SumOp> Create(IEnumerable<int> values) => new(values);
}

/// <summary>
/// 遅延伝播反転可能 Splay 木
/// </summary>
[DebuggerDisplay("Count = {" + nameof(Count) + "}")]
public class LazySplayTreeClass<T, F, TOp> : LazyBinarySearchTreeBase<T, F, LazySplayTreeClassNode<T, F, TOp>, LazySplayTreeClassNode<T, F, TOp>.Op>
    where TOp : struct, IReversibleBinarySearchTreeOperator<T, F>
{
    public LazySplayTreeClass() { }
    public LazySplayTreeClass(IEnumerable<T> v) : base(v.ToArray()) { }
    public LazySplayTreeClass(T[] v) : base(v) { }
    public LazySplayTreeClass(ReadOnlySpan<T> v) : base(v) { }
    public LazySplayTreeClass(LazySplayTreeClassNode<T, F, TOp> root) : base(root) { }
}

public class LazySplayTreeClassNode<T, F, TOp> : ISplayTreeNode<T, LazySplayTreeClassNode<T, F, TOp>>, ILazyBbstNode<T, F, LazySplayTreeClassNode<T, F, TOp>>
    where TOp : struct, IReversibleBinarySearchTreeOperator<T, F>
{
    public struct Op : ILazySplayOp<T, F, TOp, LazySplayTreeClassNode<T, F, TOp>, LazySplayTreeClassNode<T, F, TOp>, Op, PoolClassRefOp<LazySplayTreeClassNode<T, F, TOp>>>, ILazyBbstOp<T, F, LazySplayTreeClassNode<T, F, TOp>, Op>
    {
        [凾(256)]
        public static LazySplayTreeClassNode<T, F, TOp> Create(T v) => new(v);
    }

    public LazySplayTreeClassNode<T, F, TOp> Parent { get; set; }
    public T Value { get; set; }
    public T Sum { get; set; }
    public int Size { get; set; }
    public LazySplayTreeClassNode<T, F, TOp> Left { get; set; }
    public LazySplayTreeClassNode<T, F, TOp> Right { get; set; }

    public F Lazy { get; set; }
    public bool Reversed { get; set; }
    public LazySplayTreeClassNode(T v)
    {
        Size = 1;
        Sum = Value = v;
        Lazy = new TOp().FIdentity;
    }

    [SourceExpander.NotEmbeddingSource]
    public override string ToString() => $"Size = {Size}, Value = {Value}, Sum = {Sum}";
}