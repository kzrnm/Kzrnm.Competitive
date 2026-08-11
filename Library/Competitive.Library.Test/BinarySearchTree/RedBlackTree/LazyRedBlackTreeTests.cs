using Kzrnm.Competitive.Internal;
using 凾 = System.Runtime.CompilerServices.MethodImplAttribute;

namespace Kzrnm.Competitive.Testing.Collection.BinarySearchTree;

using ClassNode = LazyRedBlackTreeClassNode<int, int, SumOp>;
using Node = LazyRedBlackTreeNode<int, int, SumOp>;

[InheritsTests]
public class LazyRedBlackTreeClassTests : LazyBinarySearchTreeTestsBase<ClassNode, ClassNode.Op>
{
    protected override LazyRedBlackTreeClass<int, int, SumOp> Create()
        => new LazyRedBlackTreeClass<int, int, SumOp>();

    protected override LazyRedBlackTreeClass<int, int, SumOp> Create(IEnumerable<int> values)
        => new LazyRedBlackTreeClass<int, int, SumOp>(values);
}

[InheritsTests]
[NotInParallel(nameof(LazyRedBlackTreeTests))]
public class LazyRedBlackTreeTests : LazyBinarySearchTreeTestsBase<int, Node.Op>
{
    protected override void ClearNode() => ClearNode<Node>();
    protected override LazyRedBlackTree<int, int, SumOp> Create()
        => new LazyRedBlackTree<int, int, SumOp>();

    protected override LazyRedBlackTree<int, int, SumOp> Create(IEnumerable<int> values)
        => new LazyRedBlackTree<int, int, SumOp>(values);
}


/// <summary>
/// 遅延伝播反転可能赤黒木
/// </summary>
public class LazyRedBlackTreeClass<T, F, TOp> : LazyBinarySearchTreeBase<T, F, LazyRedBlackTreeClassNode<T, F, TOp>, LazyRedBlackTreeClassNode<T, F, TOp>.Op>
    where TOp : struct, IReversibleBinarySearchTreeOperator<T, F>
{
    public LazyRedBlackTreeClass() { }
    public LazyRedBlackTreeClass(IEnumerable<T> v) : base(v.ToArray()) { }
    public LazyRedBlackTreeClass(T[] v) : base(v) { }
    public LazyRedBlackTreeClass(ReadOnlySpan<T> v) : base(v) { }
    public LazyRedBlackTreeClass(LazyRedBlackTreeClassNode<T, F, TOp> root) : base(root) { }
}

public class LazyRedBlackTreeClassNode<T, F, TOp> : IRbtNode<T, LazyRedBlackTreeClassNode<T, F, TOp>>, ILazyBbstNode<T, F, LazyRedBlackTreeClassNode<T, F, TOp>>
    where TOp : struct, IReversibleBinarySearchTreeOperator<T, F>
{
    public LazyRedBlackTreeClassNode(LazyRedBlackTreeClassNode<T, F, TOp> other)
    {
        Left = other.Left;
        Right = other.Right;
        IsBlack = other.IsBlack;
        Level = other.Level;
        Size = other.Size;
        Sum = other.Sum;
        Reversed = other.Reversed;
        Lazy = other.Lazy;
    }

    public LazyRedBlackTreeClassNode(T v)
    {
        IsBlack = true;
        Size = 1;
        Sum = v;
        Lazy = new TOp().FIdentity;
    }

    public LazyRedBlackTreeClassNode<T, F, TOp> Left { get; set; }
    public LazyRedBlackTreeClassNode<T, F, TOp> Right { get; set; }
    public bool IsBlack { get; set; }
    public int Level { get; set; }
    public T Sum { get; set; }
    public int Size { get; set; }

    public F Lazy { get; set; }
    public bool Reversed { get; set; }

    [SourceExpander.NotEmbeddingSource]
    public override string ToString() => $"Lazy = {Lazy}{(Reversed ? "!" : "")} {((IRbtNode<T, LazyRedBlackTreeClassNode<T, F, TOp>>)this).ToStringImpl()}";

    public struct Op : ILazyRbtOp<T, F, TOp, LazyRedBlackTreeClassNode<T, F, TOp>, LazyRedBlackTreeClassNode<T, F, TOp>, Op, PoolClassRefOp<LazyRedBlackTreeClassNode<T, F, TOp>>>
    {
        [凾(256)]
        public static LazyRedBlackTreeClassNode<T, F, TOp> Create(T v) => new(v);
    }
}