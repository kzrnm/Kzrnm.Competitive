using Kzrnm.Competitive.IO;
using Kzrnm.Competitive.Testing;
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
        static int[] RandomArray(int length, int min, int maxExclusive) => rnd.NextIntArray(length, min, maxExclusive);
        static MyStringBuilder BuildEdges(MyStringBuilder sb, int max, int count)
        {
            ++max;
            for (int i = 0; i < count; i++)
            {
                var (a, b) = rnd.Choice2(1, max);
                sb.Add(a, b);
            }
            return sb;
        }
        static MyStringBuilder BuildRandomTree(MyStringBuilder sb, int size)
        {
            sb.Add(size);
            for (int i = 2; i <= size; i++)
                sb.Add(i, rnd.Next(1, i));
            return sb;
        }
        static MyStringBuilder BuildBinaryTree(MyStringBuilder sb, int size)
        {
            sb.Add(size);
            for (int i = 2; i <= size; i++)
                sb.Add(i, i / 2);
            return sb;
        }
    }
}