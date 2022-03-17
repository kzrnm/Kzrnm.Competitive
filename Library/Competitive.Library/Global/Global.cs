using System;
using 凾 = System.Runtime.CompilerServices.MethodImplAttribute;

namespace Kzrnm.Competitive
{
    public static class Global
    {
        #region NewArray
        [凾(256)]
        public static T[] NewArray<T>(int len0, T value) where T : struct => new T[len0].Fill(value);
        [凾(256)]
        public static T[] NewArray<T>(int len0, Func<T> factory)
        {
            var arr = new T[len0];
            for (int i = 0; i < arr.Length; i++) arr[i] = factory();
            return arr;
        }
        [凾(256)]
        public static T[][] NewArray<T>(int len0, int len1, T value) where T : struct
        {
            var arr = new T[len0][];
            for (int i = 0; i < arr.Length; i++) arr[i] = NewArray(len1, value);
            return arr;
        }
        [凾(256)]
        public static T[][] NewArray<T>(int len0, int len1, Func<T> factory)
        {
            var arr = new T[len0][];
            for (int i = 0; i < arr.Length; i++) arr[i] = NewArray(len1, factory);
            return arr;
        }
        [凾(256)]
        public static T[][][] NewArray<T>(int len0, int len1, int len2, T value) where T : struct
        {
            var arr = new T[len0][][];
            for (int i = 0; i < arr.Length; i++) arr[i] = NewArray(len1, len2, value);
            return arr;
        }
        [凾(256)]
        public static T[][][] NewArray<T>(int len0, int len1, int len2, Func<T> factory)
        {
            var arr = new T[len0][][];
            for (int i = 0; i < arr.Length; i++) arr[i] = NewArray(len1, len2, factory);
            return arr;
        }
        [凾(256)]
        public static T[][][][] NewArray<T>(int len0, int len1, int len2, int len3, T value) where T : struct
        {
            var arr = new T[len0][][][];
            for (int i = 0; i < arr.Length; i++) arr[i] = NewArray(len1, len2, len3, value);
            return arr;
        }
        [凾(256)]
        public static T[][][][] NewArray<T>(int len0, int len1, int len2, int len3, Func<T> factory)
        {
            var arr = new T[len0][][][];
            for (int i = 0; i < arr.Length; i++) arr[i] = NewArray(len1, len2, len3, factory);
            return arr;
        }
        #endregion NewArray
    }
}
