namespace Kzrnm.Competitive.Testing.DataStructure.String;

public class PalindromeTests
{
    [Test, MultipleAssertions]
    public async Task IsPalindrome()
    {
        var rnd = new Random(227);
        for (int len = 0; len < 20; len++)
        {
            var chrs = new char[len];
            for (int i = 0; i < chrs.Length; i++)
            {
                chrs[i] = chrs[^(i + 1)] = (char)(rnd.Next(26) + 'A');
            }

            await Palindrome.IsPalindrome(new string(chrs)).Should().BeTrue();
            await Palindrome.IsPalindrome(chrs).Should().BeTrue();
            await Palindrome.IsPalindrome(chrs.AsSpan()).Should().BeTrue();
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
                await Palindrome.IsPalindrome(new string(chrs)).Should().BeEqualTo(expected);
                await Palindrome.IsPalindrome(chrs).Should().BeEqualTo(expected);
                await Palindrome.IsPalindrome(chrs.AsSpan()).Should().BeEqualTo(expected);
            }
        }
    }

    [Test]
    [Arguments("", new int[0])]
    [Arguments("abcbaba", new int[] { 1, 1, 3, 1, 2, 2, 1, })]
    [Arguments("aaaaa", new int[] { 1, 2, 3, 2, 1, })]
    public async Task Manacher(string s, int[] expected)
    {
        await Palindrome.Manacher(s).Should().BeStrictlyEquivalentTo(expected);
    }

    [Test, MultipleAssertions]
    [Arguments("", new int[0])]
    [Arguments("abccbc", new int[] { 1, 0, 1, 0, 1, 4, 1, 0, 3, 0, 1, })]
    [Arguments("aaaaa", new int[] { 1, 2, 3, 4, 5, 4, 3, 2, 1, })]
    public async Task Manacher2(string s, int[] expected)
    {
        await Palindrome.Manacher2(s).Should().BeStrictlyEquivalentTo(expected);
        await expected.Length.Should().BeEqualTo(Math.Max(s.Length * 2 - 1, 0));
    }
}