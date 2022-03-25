using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using 凾 = System.Runtime.CompilerServices.MethodImplAttribute;

namespace Kzrnm.Competitive
{
    public static class ExComparer<T>
    {
        class ExpressionComparer<K> : IComparer<T> where K : IComparable<K>
        {
            private class ParameterReplaceVisitor : ExpressionVisitor
            {
                private readonly ParameterExpression from;
                private readonly ParameterExpression to;
                public ParameterReplaceVisitor(ParameterExpression from, ParameterExpression to)
                {
                    this.from = from;
                    this.to = to;
                }
                protected override Expression VisitParameter(ParameterExpression node) => node == from ? to : base.VisitParameter(node);
            }

            readonly Comparison<T> func;
            public ExpressionComparer(Expression<Func<T, K>> expression)
            {
                var paramA = expression.Parameters[0];
                var paramB = Expression.Parameter(typeof(T));
                var f2 = (Expression<Func<T, K>>)new ParameterReplaceVisitor(expression.Parameters[0], paramB).Visit(expression);
                var compExp = Expression.Lambda<Comparison<T>>(Expression.Call(
                        expression.Body,
                        typeof(K).GetMethod(nameof(IComparable<K>.CompareTo), new[] { typeof(K) }),
                        f2.Body),
                        paramA, paramB);
                func = compExp.Compile();
            }
            [凾(256)]
            public int Compare(T x, T y) => func(x, y);
            public override bool Equals(object obj) => obj is ExpressionComparer<K> c && func == c.func;
            public override int GetHashCode() => func.GetHashCode();
        }
        public static IComparer<T> CreateExp<K>(Expression<Func<T, K>> expression) where K : IComparable<K> => new ExpressionComparer<K>(expression);
    }
}
