using System;
using System.Linq;

namespace Kzrnm.Competitive.Testing.DataStructure.String
{
    public class PalindromeTests
    {
        [Fact]
        public void IsPalindrome()
        {
            var rnd = new Random(227);
            for (int len = 0; len < 20; len++)
            {
                var chrs = new char[len];
                for (int i = 0; i < chrs.Length; i++)
                {
                    chrs[i] = chrs[^(i + 1)] = (char)(rnd.Next(26) + 'A');
                }

                Palindrome.IsPalindrome(new string(chrs)).Should().BeTrue();
                Palindrome.IsPalindrome(chrs).Should().BeTrue();
                Palindrome.IsPalindrome(chrs.AsSpan()).Should().BeTrue();
            }

            for (int len = 2; len < 20; len++)
            {
                var chrsOrig = new char[len];
                for (int i = 0; i < chrsOrig.Length; i++)
                {
                    chrsOrig[i] = chrsOrig[^(i + 1)] = (char)(rnd.Next(26) + 'A');
                }

                for (int i = 0; i < chrsOrig.Length; i++)
                {
                    var chrs = chrsOrig.ToArray();
                    chrs[i] = char.ToLower(chrs[i]);

                    var expected = 2 * i + 1 == len;
                    Palindrome.IsPalindrome(new string(chrs)).Should().Be(expected);
                    Palindrome.IsPalindrome(chrs).Should().Be(expected);
                    Palindrome.IsPalindrome(chrs.AsSpan()).Should().Be(expected);
                }
            }
        }
    }
}
