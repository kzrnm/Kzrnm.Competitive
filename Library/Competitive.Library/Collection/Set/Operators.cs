using AtCoder;
using AtCoder.Internal;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Numerics;
using System.Runtime.CompilerServices;
using 凾 = System.Runtime.CompilerServices.MethodImplAttribute;

namespace Kzrnm.Competitive.Internal
{
    [IsOperator]
    public interface ISetOperator<T, TCmp, Node> : IComparer<T>
    {
        Node Create(T item, NodeColor color);
        T GetValue(Node node);
        TCmp GetCompareKey(T item);
    }

    [IsOperator]
    public interface ISetBinarySearchOperator
    {
        /// <summary>
        /// 左側を返す
        /// </summary>
        bool ReturnLeft { get; }

        /// <summary>
        /// 左側に潜る
        /// </summary>
        bool IntoLeft(int order);
    }
    public struct L : ISetBinarySearchOperator
    {
        public bool ReturnLeft => false;
        [凾(256)]
        public bool IntoLeft(int order) => order <= 0;
    }
    public struct U : ISetBinarySearchOperator
    {
        public bool ReturnLeft => false;
        [凾(256)]
        public bool IntoLeft(int order) => order < 0;
    }
    public struct LR : ISetBinarySearchOperator
    {
        public bool ReturnLeft => true;
        [凾(256)]
        public bool IntoLeft(int order) => order < 0;
    }
    public struct UR : ISetBinarySearchOperator
    {
        public bool ReturnLeft => true;
        [凾(256)]
        public bool IntoLeft(int order) => order <= 0;
    }

}