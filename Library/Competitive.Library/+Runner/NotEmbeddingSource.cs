using System;
using System.Diagnostics;
#if LIBRARY
namespace SourceExpander
{
    [Conditional("COMPILE_TIME_ONLY")]
    [AttributeUsage(AttributeTargets.All, Inherited = false, AllowMultiple = true)]
    internal sealed class NotEmbeddingSourceAttribute : Attribute { }
}
#endif