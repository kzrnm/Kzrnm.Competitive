using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kzrnm.Competitive.Testing;

public static class BitArrayCase
{
    public static TheoryData<string> LongBinaryTexts()
    {
        return new(Inner().Distinct());
        static IEnumerable<string> Inner()
        {
            for (int i = 0; i < 20; i++)
            {
                yield return System.Convert.ToString(i, 2);
            }
            for (int i = 0; i < 20; i++)
            {
                yield return System.Convert.ToString(i + (1L << 32) - 10, 2);
            }
            for (int i = 0; i < 20; i++)
            {
                yield return System.Convert.ToString(i + (1L << 40), 2);
            }

            var rnd = new Xoshiro256(227);
            for (int i = 0; i < 1000; i++)
            {
                int len = rnd.NextInt32(2, 1000);
                var chrs = new char[len];
                for (int j = 0; j < chrs.Length; j++)
                {
                    chrs[j] = (char)(rnd.NextInt32(2) + '0');
                }
                yield return new string(chrs);
            }
        }
    }

}
