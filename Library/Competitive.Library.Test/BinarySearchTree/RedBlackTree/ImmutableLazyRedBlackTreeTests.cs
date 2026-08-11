using Kzrnm.Competitive.Internal;
using 凾 = System.Runtime.CompilerServices.MethodImplAttribute;

namespace Kzrnm.Competitive.Testing.Collection.BinarySearchTree;

using ClassNode = LazyRedBlackTreeClassNode<int, int, SumOp>;
using ClassNodeOp = ImmutableLazyRedBlackTreeClassNodeOp<int, int, SumOp>;
using NodeOp = ImmutableLazyRedBlackTreeNodeOp<int, int, SumOp>;

[InheritsTests]
[NotInParallel(nameof(LazyRedBlackTreeTests))]
public class ImmutableLazyRedBlackTreeClassTests
    : ImmutableLazyBinarySearchTreeTestsBase<ClassNode, ImmutableLazyRedBlackTreeClass<int, int, SumOp>.Mk, ClassNodeOp, ImmutableLazyRedBlackTreeClass<int, int, SumOp>>
{
    protected override ImmutableLazyRedBlackTreeClass<int, int, SumOp> Empty
        => ImmutableLazyRedBlackTreeClass<int, int, SumOp>.Empty;
    protected override ImmutableLazyRedBlackTreeClass<int, int, SumOp> Create(IEnumerable<int> values)
        => ImmutableLazyRedBlackTreeClass<int, int, SumOp>.Create(values.ToArray());
}

[InheritsTests]
[NotInParallel(nameof(LazyRedBlackTreeTests))]
public class ImmutableLazyRedBlackTreeTests
    : ImmutableLazyBinarySearchTreeTestsBase<int, ImmutableLazyRedBlackTree<int, int, SumOp>.Mk, NodeOp, ImmutableLazyRedBlackTree<int, int, SumOp>>
{
    protected override void ClearNode() => ClearNode<LazyRedBlackTreeNode<int, int, SumOp>>();
    protected override ImmutableLazyRedBlackTree<int, int, SumOp> Empty
        => ImmutableLazyRedBlackTree<int, int, SumOp>.Empty;
    protected override ImmutableLazyRedBlackTree<int, int, SumOp> Create(IEnumerable<int> values)
        => ImmutableLazyRedBlackTree<int, int, SumOp>.Create(values.ToArray());
}

/// <summary>
/// 永続遅延伝播反転可能赤黒木
/// </summary>
public sealed class ImmutableLazyRedBlackTreeClass<T, F, TOp>
    : ImmutableLazyBinarySearchTreeBase<T, F, ImmutableLazyRedBlackTreeClass<T, F, TOp>, LazyRedBlackTreeClassNode<T, F, TOp>, ImmutableLazyRedBlackTreeClass<T, F, TOp>.Mk, ImmutableLazyRedBlackTreeClassNodeOp<T, F, TOp>>
    where TOp : struct, IReversibleBinarySearchTreeOperator<T, F>
{
    public struct Mk : IImmutableBbstMaker<ImmutableLazyRedBlackTreeClass<T, F, TOp>, LazyRedBlackTreeClassNode<T, F, TOp>>
    {
        [凾(256)]
        public static ImmutableLazyRedBlackTreeClass<T, F, TOp> Create(LazyRedBlackTreeClassNode<T, F, TOp> node) => new(node);
    }
    [凾(256)] public static ImmutableLazyRedBlackTreeClass<T, F, TOp> Create(ImmutableLazyRedBlackTreeClass<T, F, TOp> other) => new(other.root);
    [凾(256)] public static ImmutableLazyRedBlackTreeClass<T, F, TOp> Create() => Empty;
    [凾(256)] public static ImmutableLazyRedBlackTreeClass<T, F, TOp> Create(params ReadOnlySpan<T> v) => new(v);
    private ImmutableLazyRedBlackTreeClass(ReadOnlySpan<T> v) : base(v) { }
    public ImmutableLazyRedBlackTreeClass(LazyRedBlackTreeClassNode<T, F, TOp> root) : base(root) { }
}


public struct ImmutableLazyRedBlackTreeClassNodeOp<T, F, TOp> : ILazyRbtOp<T, F, TOp, LazyRedBlackTreeClassNode<T, F, TOp>, LazyRedBlackTreeClassNode<T, F, TOp>, ImmutableLazyRedBlackTreeClassNodeOp<T, F, TOp>, PoolClassRefOp<LazyRedBlackTreeClassNode<T, F, TOp>>>
    where TOp : struct, IReversibleBinarySearchTreeOperator<T, F>
{
    [凾(256)]
    public static LazyRedBlackTreeClassNode<T, F, TOp> Create(T v) => new(v);

    [凾(256)]
    public static LazyRedBlackTreeClassNode<T, F, TOp> Copy(LazyRedBlackTreeClassNode<T, F, TOp> t) => t == null ? null : new(t);
}