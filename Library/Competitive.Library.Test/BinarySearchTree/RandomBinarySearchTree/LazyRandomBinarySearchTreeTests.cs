using Kzrnm.Competitive.Internal;
using Kzrnm.Competitive.Internal.Bbst;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using 凾 = System.Runtime.CompilerServices.MethodImplAttribute;

namespace Kzrnm.Competitive.Testing.Collection.BinarySearchTree;

using ClassNode = LazyRbstClassNode<int, int, SumOp>;

using Node = LazyRbstNode<int, int, SumOp>;

[InheritsTests]
public class LazyRandomBinarySearchTreeClassTests : LazyBinarySearchTreeTestsBase<ClassNode, ClassNode.Op>
{
    protected override LazyRandomBinarySearchTreeClass<int, int, SumOp> Create() => new();
    protected override LazyRandomBinarySearchTreeClass<int, int, SumOp> Create(IEnumerable<int> values) => new(values);
}


[InheritsTests]
[NotInParallel(nameof(LazyRandomBinarySearchTreeTests))]
public class LazyRandomBinarySearchTreeTests : LazyBinarySearchTreeTestsBase<int, Node.__LzyRbtsOp>
{
    protected override void ClearNode() => ClearNode<Node>();
    protected override LazyRandomBinarySearchTree<int, int, SumOp> Create() => new();
    protected override LazyRandomBinarySearchTree<int, int, SumOp> Create(IEnumerable<int> values) => new(values);
}


/// <summary>
/// 遅延伝播乱択平衡二分探索木
/// </summary>
[DebuggerDisplay("Count = {" + nameof(Count) + "}")]
public class LazyRandomBinarySearchTreeClass<T, F, TOp> : LazyBinarySearchTreeBase<T, F, LazyRbstClassNode<T, F, TOp>, LazyRbstClassNode<T, F, TOp>.Op>
    where TOp : struct, IReversibleBinarySearchTreeOperator<T, F>
{
    public LazyRandomBinarySearchTreeClass() { }
    public LazyRandomBinarySearchTreeClass(IEnumerable<T> v) : base(v.ToArray()) { }
    public LazyRandomBinarySearchTreeClass(T[] v) : base(v) { }
    public LazyRandomBinarySearchTreeClass(ReadOnlySpan<T> v) : base(v) { }
    public LazyRandomBinarySearchTreeClass(LazyRbstClassNode<T, F, TOp> root) : base(root) { }
}

public class LazyRbstClassNode<T, F, TOp> : IRbstNode<T, LazyRbstClassNode<T, F, TOp>>, ILazyBbstNode<T, F, LazyRbstClassNode<T, F, TOp>>
    where TOp : struct, IReversibleBinarySearchTreeOperator<T, F>
{
    public struct Op : ILazyRbstOp<T, F, TOp, LazyRbstClassNode<T, F, TOp>, LazyRbstClassNode<T, F, TOp>, Op, PoolClassRefOp<LazyRbstClassNode<T, F, TOp>>>
    {
        [凾(256)]
        public static LazyRbstClassNode<T, F, TOp> Create(T v) => new(v);
    }

    static TOp op => new();

    public LazyRbstClassNode<T, F, TOp> Parent { get; set; }
    public LazyRbstClassNode<T, F, TOp> Left { get; set; }
    public LazyRbstClassNode<T, F, TOp> Right { get; set; }
    public T Value { get; set; }
    public T Sum { get; set; }
    public int Size { get; set; }

    public F Lazy { get; set; }
    public bool Reversed { get; set; }
    public LazyRbstClassNode(T v)
    {
        Size = 1;
        Sum = Value = v;
        Lazy = op.FIdentity;
    }

    [SourceExpander.NotEmbeddingSource]
    public override string ToString() => $"Size = {Size}, Value = {Value}, Sum = {Sum}, Lazy = {Lazy}";
}