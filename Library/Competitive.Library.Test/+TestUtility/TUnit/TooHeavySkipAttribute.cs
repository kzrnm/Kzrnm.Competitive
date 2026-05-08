namespace Kzrnm.Competitive.Testing;

class TooHeavySkipAttribute() : SkipAttribute("重いので普段は飛ばす")
{
    public override Task<bool> ShouldSkip(TestRegisteredContext context)
    {
        return Task.FromResult(
#if AOT || !LIBRARY
            false
#else
            true
#endif
        );
    }
}
