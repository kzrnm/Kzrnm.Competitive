using Kzrnm.Competitive.Internal;
using Kzrnm.Competitive.Internal.Bbst;
using 凾 = System.Runtime.CompilerServices.MethodImplAttribute;

namespace Kzrnm.Competitive.Testing.Collection.BinarySearchTree;

using ClassNode = LazyAvlTreeClassNode<int, int, SumOp>;
using Node = LazyAvlTreeNode<int, int, SumOp>;

[InheritsTests]
public class LazyAvlTreeClassTests : LazyBinarySearchTreeTestsBase<ClassNode, ClassNode.Op>
{
    protected override LazyAvlTreeClass<int, int, SumOp> Create()
        => new LazyAvlTreeClass<int, int, SumOp>();

    protected override LazyAvlTreeClass<int, int, SumOp> Create(IEnumerable<int> values)
        => new LazyAvlTreeClass<int, int, SumOp>(values);
}

[InheritsTests]
[NotInParallel(nameof(LazyAvlTreeTests))]
public class LazyAvlTreeTests : LazyBinarySearchTreeTestsBase<int, Node.__LzyAvlOp>
{
    protected override void ClearNode() => ClearNode<Node>();
    protected override LazyAvlTree<int, int, SumOp> Create()
        => new LazyAvlTree<int, int, SumOp>();

    protected override LazyAvlTree<int, int, SumOp> Create(IEnumerable<int> values)
        => new LazyAvlTree<int, int, SumOp>(values);
}

/// <summary>
/// 遅延伝播反転可能AVL木
/// </summary>
public class LazyAvlTreeClass<T, F, TOp> : LazyBinarySearchTreeBase<T, F, LazyAvlTreeClassNode<T, F, TOp>, LazyAvlTreeClassNode<T, F, TOp>.Op>
    where TOp : struct, IReversibleBinarySearchTreeOperator<T, F>
{
    public LazyAvlTreeClass() { }
    public LazyAvlTreeClass(IEnumerable<T> v) : base(v.ToArray()) { }
    public LazyAvlTreeClass(T[] v) : base(v) { }
    public LazyAvlTreeClass(ReadOnlySpan<T> v) : base(v) { }
    public LazyAvlTreeClass(LazyAvlTreeClassNode<T, F, TOp> root) : base(root) { }
}

public class LazyAvlTreeClassNode<T, F, TOp> : IAvlNode<T, LazyAvlTreeClassNode<T, F, TOp>>, ILazyBbstNode<T, F, LazyAvlTreeClassNode<T, F, TOp>>
    where TOp : struct, IReversibleBinarySearchTreeOperator<T, F>
{
    public struct Op : ILazyAvlOp<T, F, TOp, LazyAvlTreeClassNode<T, F, TOp>, LazyAvlTreeClassNode<T, F, TOp>, Op, PoolClassRefOp<LazyAvlTreeClassNode<T, F, TOp>>>
    {
        [凾(256)]
        public static LazyAvlTreeClassNode<T, F, TOp> Create(T v) => new(v);
    }

    public LazyAvlTreeClassNode<T, F, TOp> Left { get; set; }
    public LazyAvlTreeClassNode<T, F, TOp> Right { get; set; }
    public T Value { get; set; }
    public T Sum { get; set; }
    public int Height { get; set; }
    public int Size { get; set; }
    public F Lazy { get; set; }
    public bool Reversed { get; set; }
    public LazyAvlTreeClassNode(T v)
    {
        Height = 1;
        Size = 1;
        Sum = Value = v;
        Lazy = new TOp().FIdentity;
    }

    [SourceExpander.NotEmbeddingSource]
    public override string ToString() => $"Lazy = {Lazy}{(Reversed ? "!" : "")} {base.ToString()}";
}