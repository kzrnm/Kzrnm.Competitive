using AtCoder;
using Kzrnm.Competitive.Internal;
using 凾 = System.Runtime.CompilerServices.MethodImplAttribute;

namespace Kzrnm.Competitive.Testing.Collection.BinarySearchTree;

using ClassNode = RedBlackTreeClassNode<int, Starry>;
using Node = RedBlackTreeNode<int, Starry>;

[InheritsTests]
public class RedBlackTreeClassTests : BinarySearchTreeTestsBase<ClassNode, ClassNode.Op>
{
    protected override RedBlackTreeClass<int, Starry> Create()
        => new RedBlackTreeClass<int, Starry>();

    protected override RedBlackTreeClass<int, Starry> Create(IEnumerable<int> values)
        => new RedBlackTreeClass<int, Starry>(values);
}

[InheritsTests]
[NotInParallel(nameof(RedBlackTreeTests))]
public class RedBlackTreeTests : BinarySearchTreeTestsBase<int, Node.__RbtOp>
{
    protected override void ClearNode() => ClearNode<Node>();
    protected override RedBlackTree<int, Starry> Create()
        => new RedBlackTree<int, Starry>();

    protected override RedBlackTree<int, Starry> Create(IEnumerable<int> values)
        => new RedBlackTree<int, Starry>(values);
}






/// <summary>
/// 赤黒木
/// </summary>
public class RedBlackTreeClass<T, TOp> : BinarySearchTreeBase<T, RedBlackTreeClassNode<T, TOp>, RedBlackTreeClassNode<T, TOp>.Op>
    where TOp : struct, ISegtreeOperator<T>
{
    public RedBlackTreeClass() { }
    public RedBlackTreeClass(IEnumerable<T> v) : base(v.ToArray()) { }
    public RedBlackTreeClass(T[] v) : base(v) { }
    public RedBlackTreeClass(ReadOnlySpan<T> v) : base(v) { }
    public RedBlackTreeClass(RedBlackTreeClassNode<T, TOp> root) : base(root) { }
}

public class RedBlackTreeClassNode<T, TOp> : IRbtNode<T, RedBlackTreeClassNode<T, TOp>>
    where TOp : struct, ISegtreeOperator<T>
{
    public RedBlackTreeClassNode(RedBlackTreeClassNode<T, TOp> other)
    {
        Left = other.Left;
        Right = other.Right;
        IsBlack = other.IsBlack;
        Level = other.Level;
        Size = other.Size;
        Sum = other.Sum;
    }

    public RedBlackTreeClassNode(T v)
    {
        IsBlack = true;
        Size = 1;
        Sum = v;
    }
    public bool IsBlack { get; set; }
    public int Level { get; set; }
    public T Sum { get; set; }
    public int Size { get; set; }
    public RedBlackTreeClassNode<T, TOp> Left { get; set; }
    public RedBlackTreeClassNode<T, TOp> Right { get; set; }

    [SourceExpander.NotEmbeddingSource]
    public override string ToString() => ((IRbtNode<T, RedBlackTreeClassNode<T, TOp>>)this).ToStringImpl();

    public struct Op : IRbtOp<T, TOp, RedBlackTreeClassNode<T, TOp>, RedBlackTreeClassNode<T, TOp>, Op, PoolClassRefOp<RedBlackTreeClassNode<T, TOp>>>
    {
        [凾(256)]
        public static RedBlackTreeClassNode<T, TOp> Create(T v) => new(v);
    }
}
