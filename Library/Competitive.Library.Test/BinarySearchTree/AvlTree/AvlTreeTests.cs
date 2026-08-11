using AtCoder;
using Kzrnm.Competitive.Internal;
using Kzrnm.Competitive.Internal.Bbst;
using 凾 = System.Runtime.CompilerServices.MethodImplAttribute;

namespace Kzrnm.Competitive.Testing.Collection.BinarySearchTree;

using ClassNode = AvlTreeClassNode<int, Starry>;
using Node = AvlTreeNode<int, Starry>;

[InheritsTests]
public class AvlTreeClassTests : BinarySearchTreeTestsBase<ClassNode, ClassNode.Op>
{
    protected override AvlTreeClass<int, Starry> Create()
        => new AvlTreeClass<int, Starry>();

    protected override AvlTreeClass<int, Starry> Create(IEnumerable<int> values)
        => new AvlTreeClass<int, Starry>(values);
}

[InheritsTests]
[NotInParallel(nameof(AvlTreeTests))]
public class AvlTreeTests : BinarySearchTreeTestsBase<int, Node.Op>
{
    protected override void ClearNode() => ClearNode<Node>();
    protected override AvlTree<int, Starry> Create()
        => new AvlTree<int, Starry>();

    protected override AvlTree<int, Starry> Create(IEnumerable<int> values)
        => new AvlTree<int, Starry>(values);
}


/// <summary>
/// AVL木
/// </summary>
public class AvlTreeClass<T, TOp> : BinarySearchTreeBase<T, AvlTreeClassNode<T, TOp>, AvlTreeClassNode<T, TOp>.Op>
    where TOp : struct, ISegtreeOperator<T>
{
    public AvlTreeClass() { }
    public AvlTreeClass(IEnumerable<T> v) : base(v.ToArray()) { }
    public AvlTreeClass(T[] v) : base(v) { }
    public AvlTreeClass(ReadOnlySpan<T> v) : base(v) { }
    public AvlTreeClass(AvlTreeClassNode<T, TOp> root) : base(root) { }
}

public class AvlTreeClassNode<T, TOp> : IAvlNode<T, AvlTreeClassNode<T, TOp>>
    where TOp : struct, ISegtreeOperator<T>
{
    public AvlTreeClassNode<T, TOp> Left { get; set; }
    public AvlTreeClassNode<T, TOp> Right { get; set; }
    public T Value { get; set; }
    public T Sum { get; set; }
    public int Height { get; set; }
    public int Size { get; set; }

    public AvlTreeClassNode(T v)
    {
        Height = 1;
        Left = Right = null;
        Size = 1;
        Sum = Value = v;
    }

    public struct Op : IAvlOp<T, TOp, AvlTreeClassNode<T, TOp>, AvlTreeClassNode<T, TOp>, Op, PoolClassRefOp<AvlTreeClassNode<T, TOp>>>
    {
        [凾(256)]
        public static AvlTreeClassNode<T, TOp> Create(T v) => new(v);
    }
}