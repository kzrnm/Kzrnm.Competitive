using Kzrnm.Competitive.IO;
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
        internal static Random rnd = new Random();
        private static int[] RandomArray(int length, int min, int maxExclusive) { var arr = new int[length]; for (int i = 0; i < arr.Length; i++) arr[i] = rnd.Next(min, maxExclusive); return arr; }
        static MyStringBuilder Build()
        {
            var sb = new MyStringBuilder
            {

            };

            return sb;
        }
    }
}