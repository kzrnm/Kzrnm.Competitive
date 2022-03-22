using System;
using System.Linq;

namespace Kzrnm.Competitive.Testing
{
    [AttributeUsage(AttributeTargets.All)]
    public sealed class VerifyAttribute : Attribute
    {
        /// <summary>
        /// TestingMain を埋め込むための Attribute
        /// </summary>
        public VerifyAttribute() { }
    }
    public static class TestingMain
    {
        public static void Main()
            => Console.WriteLine(Console.ReadLine().Split().Sum(int.Parse));
    }
}
