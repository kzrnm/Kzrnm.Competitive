using AtCoder;
using Kzrnm.Competitive.Internal;
using Kzrnm.Competitive.Internal.Bbst;
using System.Diagnostics;
using 凾 = System.Runtime.CompilerServices.MethodImplAttribute;

namespace Kzrnm.Competitive.Testing.Collection.BinarySearchTree;

using ClassNode = RbstClassNode<int, Starry>;
using Node = RbstNode<int, Starry>;

[InheritsTests]
public class RandomBinarySearchTreeClassTests : BinarySearchTreeTestsBase<ClassNode, ClassNode.Op>
{
    protected override RandomBinarySearchTreeClass<int, Starry> Create() => new();
    protected override RandomBinarySearchTreeClass<int, Starry> Create(IEnumerable<int> values) => new(values);
}

[InheritsTests]
[NotInParallel(nameof(RandomBinarySearchTreeTests))]
public class RandomBinarySearchTreeTests : BinarySearchTreeTestsBase<int, Node.Op>
{
    protected override void ClearNode() => ClearNode<Node>();
    protected override RandomBinarySearchTree<int, Starry> Create() => new();
    protected override RandomBinarySearchTree<int, Starry> Create(IEnumerable<int> values) => new(values);
}


/// <summary>
/// 乱択平衡二分探索木
/// </summary>
[DebuggerDisplay("Count = {" + nameof(Count) + "}")]
public class RandomBinarySearchTreeClass<T, TOp> : BinarySearchTreeBase<T, RbstClassNode<T, TOp>, RbstClassNode<T, TOp>.Op>
    where TOp : struct, ISegtreeOperator<T>
{
    public RandomBinarySearchTreeClass() { }
    public RandomBinarySearchTreeClass(IEnumerable<T> v) : base(v.ToArray()) { }
    public RandomBinarySearchTreeClass(T[] v) : base(v) { }
    public RandomBinarySearchTreeClass(ReadOnlySpan<T> v) : base(v) { }
    public RandomBinarySearchTreeClass(RbstClassNode<T, TOp> root) : base(root) { }
}

public class RbstClassNode<T, TOp> : IRbstNode<T, RbstClassNode<T, TOp>>
    where TOp : struct, ISegtreeOperator<T>
{
    public struct Op : IRbstOp<T, TOp, RbstClassNode<T, TOp>, RbstClassNode<T, TOp>, Op, PoolClassRefOp<RbstClassNode<T, TOp>>>
    {
        [凾(256)]
        public static RbstClassNode<T, TOp> Create(T v) => new(v);
    }
    public RbstClassNode<T, TOp> Parent { get; set; }
    public RbstClassNode<T, TOp> Left { get; set; }
    public RbstClassNode<T, TOp> Right { get; set; }
    public T Value { get; set; }
    public T Sum { get; set; }
    public int Size { get; set; }

    public RbstClassNode(T v)
    {
        Size = 1;
        Sum = Value = v;
    }

    [SourceExpander.NotEmbeddingSource]
    public override string ToString() => $"Size = {Size}, Value = {Value}, Sum = {Sum}";
}