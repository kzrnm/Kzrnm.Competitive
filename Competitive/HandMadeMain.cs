﻿using Kzrnm.Competitive.IO;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;

namespace Competitive.Runner
{
    static partial class HandMadeMain
    {
        static string BasePath = CurrentPath();
        internal static Random rnd = new Random(227);
        static MyStringBuilder Build()
        {
            var sb = new MyStringBuilder
            {

            };

            return sb;
        }
    }

    // Utilメソッド
    static partial class HandMadeMain
    {
        static int[] RandomArray(int length, int min, int maxExclusive) { var arr = new int[length]; for (int i = 0; i < arr.Length; i++) arr[i] = rnd.Next(min, maxExclusive); return arr; }
        static MyStringBuilder BuildRandomTree(MyStringBuilder sb, int size)
        {
            sb.Add(size);
            for (int i = 2; i <= size; i++)
                sb.Add(i, rnd.Next(1, i));
            return sb;
        }
    }
}