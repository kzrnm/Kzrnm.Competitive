﻿using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace AtCoder
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
                this.func = compExp.Compile();
            }
            public int Compare(T x, T y) => func(x, y);
            public override bool Equals(object obj) => obj is ExpressionComparer<K> c && this.func == c.func;
            public override int GetHashCode() => func.GetHashCode();
        }
        class ReverseComparer : IComparer<T>
        {
            private static readonly Comparer<T> orig = Comparer<T>.Default;
            public int Compare(T y, T x) => orig.Compare(x, y);
            public override bool Equals(object obj) => obj is ReverseComparer;
            public override int GetHashCode() => GetType().GetHashCode();
        }
        public static IComparer<T> CreateExp<K>(Expression<Func<T, K>> expression) where K : IComparable<K> => new ExpressionComparer<K>(expression);
        public static readonly IComparer<T> DefaultReverse = new ReverseComparer();
    }
}