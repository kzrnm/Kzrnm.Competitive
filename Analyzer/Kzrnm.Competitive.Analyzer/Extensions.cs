using System.Collections.Generic;

namespace Kzrnm.Competitive.Analyzer;

internal static class Extensions
{
    extension<TKey, TValue>(KeyValuePair<TKey, TValue> pair)
    {
        public void Deconstruct(out TKey key, out TValue value)
        {
            key = pair.Key;
            value = pair.Value;
        }
    }
}
