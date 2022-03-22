using System;
using System.Linq;

namespace Kzrnm.Competitive.Testing
{
    [AttributeUsage(AttributeTargets.All)]
    public sealed class VerifyAttribute : Attribute
    {
        /// <summary>
        /// TestingMain を埋め込むための Attribute
        /// 
        /// PROBLEM も一緒に書く
        /// [Verify] // verification-helper: PROBLEM https://judge.yosupo.jp/problem/aplusb
        /// </summary>
        public VerifyAttribute() { }
    }
    public static class TestingMain
    {
        public static void Main()
            => Console.WriteLine(Console.ReadLine().Split().Sum(int.Parse));
    }
}
