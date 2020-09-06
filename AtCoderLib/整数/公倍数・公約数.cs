class 公倍数_公約数
{

    int Gcd(params int[] nums)
    {
        var gcd = nums[0];
        for (var i = 1; i < nums.Length; i++)
            gcd = Gcd(nums[i], gcd);
        return gcd;
    }
    int Gcd(int a, int b) => b > a ? Gcd(b, a) : (b == 0 ? a : Gcd(b, a % b));
    int Lcm(int a, int b) => a / Gcd(a, b) * b;

    int Lcm(params int[] nums)
    {
        var lcm = nums[0];
        for (var i = 1; i < nums.Length; i++)
            lcm = Lcm(lcm, nums[i]);
        return lcm;
    }


    long Gcd(params long[] nums)
    {
        var gcd = nums[0];
        for (var i = 1; i < nums.Length; i++)
            gcd = Gcd(nums[i], gcd);
        return gcd;
    }
    long Gcd(long a, long b) => b > a ? Gcd(b, a) : (b == 0 ? a : Gcd(b, a % b));
    long Lcm(long a, long b) => a / Gcd(a, b) * b;

    long Lcm(params long[] nums)
    {
        var lcm = nums[0];
        for (var i = 1; i < nums.Length; i++)
            lcm = Lcm(lcm, nums[i]);
        return lcm;
    }
}
