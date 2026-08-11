using AtCoder;
using Kzrnm.Competitive.Internal;
using 凾 = System.Runtime.CompilerServices.MethodImplAttribute;

namespace Kzrnm.Competitive.Testing.Collection.BinarySearchTree;

using ClassNode = RedBlackTreeClassNode<int, SingleBbstOp<int>>;
using ClassNodeOp = ImmutableRedBlackTreeClassNodeOp<int, SingleBbstOp<int>>;
using NodeOp = ImmutableRedBlackTreeNodeOp<int, SingleBbstOp<int>>;

[InheritsTests]
public class ImmutableRedBlackTreeClassTests
    : ImmutableBinarySearchTreeTestsBase<ClassNode, ImmutableRedBlackTreeClass<int>.Mk, ClassNodeOp, ImmutableRedBlackTreeClass<int>>
{
    protected override bool UseProd => false;
    protected override ImmutableRedBlackTreeClass<int> Empty
        => ImmutableRedBlackTreeClass<int>.Empty;
    protected override ImmutableRedBlackTreeClass<int> Create(IEnumerable<int> values)
        => ImmutableRedBlackTreeClass<int>.Create(values.ToArray());
}

[InheritsTests]
[NotInParallel(nameof(RedBlackTreeTests))]
public class ImmutableRedBlackTreeTests
    : ImmutableBinarySearchTreeTestsBase<int, ImmutableRedBlackTree<int>.Mk, NodeOp, ImmutableRedBlackTree<int>>
{
    protected override void ClearNode() => ClearNode<RedBlackTreeNode<int, SingleBbstOp<int>>>();
    protected override bool UseProd => false;
    protected override ImmutableRedBlackTree<int> Empty
        => ImmutableRedBlackTree<int>.Empty;
    protected override ImmutableRedBlackTree<int> Create(IEnumerable<int> values)
        => ImmutableRedBlackTree<int>.Create(values.ToArray());
}

/// <summary>
/// 永続赤黒木
/// </summary>
public sealed class ImmutableRedBlackTreeClass<T>
    : ImmutableBinarySearchTreeBase<T, ImmutableRedBlackTreeClass<T>, RedBlackTreeClassNode<T, SingleBbstOp<T>>, ImmutableRedBlackTreeClass<T>.Mk, ImmutableRedBlackTreeClassNodeOp<T, SingleBbstOp<T>>>
{
    public struct Mk : IImmutableBbstMaker<ImmutableRedBlackTreeClass<T>, RedBlackTreeClassNode<T, SingleBbstOp<T>>>
    {
        [凾(256)]
        public static ImmutableRedBlackTreeClass<T> Create(RedBlackTreeClassNode<T, SingleBbstOp<T>> node) => new(node);
    }
    [凾(256)] public static ImmutableRedBlackTreeClass<T> Create(RedBlackTreeClassNode<T, SingleBbstOp<T>> node) => new(node);
    [凾(256)] public static ImmutableRedBlackTreeClass<T> Create() => Empty;
#if NET9_0_OR_GREATER
    [凾(256)] public static ImmutableRedBlackTreeClass<T> Create(params ReadOnlySpan<T> v) => new(v);
#else
        [凾(256)] public static ImmutableRedBlackTreeClass<T> Create(params T[] v) => new(v);
        [凾(256)] public static ImmutableRedBlackTreeClass<T> Create(ReadOnlySpan<T> v) => new(v);
#endif
    ImmutableRedBlackTreeClass(ReadOnlySpan<T> v) : base(v) { }
    public ImmutableRedBlackTreeClass(RedBlackTreeClassNode<T, SingleBbstOp<T>> root) : base(root) { }
}

public struct ImmutableRedBlackTreeClassNodeOp<T, TOp> : IRbtOp<T, TOp, RedBlackTreeClassNode<T, TOp>, RedBlackTreeClassNode<T, TOp>, ImmutableRedBlackTreeClassNodeOp<T, TOp>, PoolClassRefOp<RedBlackTreeClassNode<T, TOp>>>
    where TOp : struct, ISegtreeOperator<T>
{
    [凾(256)]
    public static RedBlackTreeClassNode<T, TOp> Create(T v) => new(v);

    [凾(256)]
    public static RedBlackTreeClassNode<T, TOp> Copy(RedBlackTreeClassNode<T, TOp> t) => t == null ? null : new(t);
}
