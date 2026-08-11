using AtCoder;
using Kzrnm.Competitive.Internal;
using Kzrnm.Competitive.Internal.Bbst;
using System.Diagnostics;
using 凾 = System.Runtime.CompilerServices.MethodImplAttribute;

namespace Kzrnm.Competitive.Testing.Collection.BinarySearchTree;

using ClassNode = SplayTreeNodeClass<int, Starry>;
using Node = SplayTreeNode<int, Starry>;

[InheritsTests]
public class SplayTreeClassTests : BinarySearchTreeTestsBase<ClassNode, ClassNode.Op>
{
    protected override SplayTreeClass<int, Starry> Create() => new();
    protected override SplayTreeClass<int, Starry> Create(IEnumerable<int> values) => new(values);
}

[InheritsTests]
[NotInParallel(nameof(SplayTreeTests))]
public class SplayTreeTests : BinarySearchTreeTestsBase<int, Node.Op>
{
    protected override void ClearNode() => ClearNode<Node>();
    protected override SplayTree<int, Starry> Create() => new();
    protected override SplayTree<int, Starry> Create(IEnumerable<int> values) => new(values);
}

/// <summary>
/// Splay 木
/// </summary>
[DebuggerDisplay("Count = {" + nameof(Count) + "}")]
public class SplayTreeClass<T, TOp> : BinarySearchTreeBase<T, SplayTreeNodeClass<T, TOp>, SplayTreeNodeClass<T, TOp>.Op>
    where TOp : struct, ISegtreeOperator<T>
{
    public SplayTreeClass() { }
    public SplayTreeClass(IEnumerable<T> v) : base(v.ToArray()) { }
    public SplayTreeClass(T[] v) : base(v) { }
    public SplayTreeClass(ReadOnlySpan<T> v) : base(v) { }
    public SplayTreeClass(SplayTreeNodeClass<T, TOp> root) : base(root) { }
}
public class SplayTreeNodeClass<T, TOp> : ISplayTreeNode<T, SplayTreeNodeClass<T, TOp>>
    where TOp : struct, ISegtreeOperator<T>
{
    public struct Op : ISplayTreePusher<T, SplayTreeNodeClass<T, TOp>, SplayTreeNodeClass<T, TOp>, Op, PoolClassRefOp<SplayTreeNodeClass<T, TOp>>>
    {
        [凾(256)]
        public static SplayTreeNodeClass<T, TOp> Create(T v) => new(v);

        [凾(256)]
        public static T Prod(T x, T y) => op.Operate(x, y);

        [凾(256)]
        public static void Push(SplayTreeNodeClass<T, TOp> t)
        {
        }

        [凾(256)]
        public static T Sum(SplayTreeNodeClass<T, TOp> t)
            => t != null ? t.Sum : op.Identity;
    }

    static TOp op => new();

    public SplayTreeNodeClass<T, TOp> Parent { get; set; }
    public SplayTreeNodeClass<T, TOp> Left { get; set; }
    public SplayTreeNodeClass<T, TOp> Right { get; set; }
    public T Value { get; set; }
    public T Sum { get; set; }
    public int Size { get; set; }

    public SplayTreeNodeClass(T v)
    {
        Size = 1;
        Sum = Value = v;
    }

    [SourceExpander.NotEmbeddingSource]
    [凾(256)]
    public override string ToString() => $"Size = {Size}, Value = {Value}, Sum = {Sum}";
}