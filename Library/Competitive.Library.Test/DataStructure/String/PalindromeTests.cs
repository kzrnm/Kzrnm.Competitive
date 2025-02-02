using System;
using System.Linq;

namespace Kzrnm.Competitive.Testing.DataStructure.String;

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

            Palindrome.IsPalindrome(new string(chrs)).ShouldBeTrue();
            Palindrome.IsPalindrome(chrs).ShouldBeTrue();
            Palindrome.IsPalindrome(chrs.AsSpan()).ShouldBeTrue();
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
                Palindrome.IsPalindrome(new string(chrs)).ShouldBe(expected);
                Palindrome.IsPalindrome(chrs).ShouldBe(expected);
                Palindrome.IsPalindrome(chrs.AsSpan()).ShouldBe(expected);
            }
        }
    }

    [Theory]
    [InlineData("", new int[0])]
    [InlineData("abcbaba", new int[] { 1, 1, 3, 1, 2, 2, 1, })]
    [InlineData("aaaaa", new int[] { 1, 2, 3, 2, 1, })]
    public void Manacher(string s, int[] expected)
    {
        Palindrome.Manacher(s).ShouldBe(expected);
    }

    [Theory]
    [InlineData("", new int[0])]
    [InlineData("abccbc", new int[] { 1, 0, 1, 0, 1, 4, 1, 0, 3, 0, 1, })]
    [InlineData("aaaaa", new int[] { 1, 2, 3, 4, 5, 4, 3, 2, 1, })]
    public void Manacher2(string s, int[] expected)
    {
        Palindrome.Manacher2(s).ShouldBe(expected);
        expected.Length.ShouldBe(Math.Max(s.Length * 2 - 1, 0));
    }
}
