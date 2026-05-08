namespace Kzrnm.Competitive.Testing;

class TooHeavySkipAttribute() : SkipAttribute("https://github.com/thomhurst/TUnit/issues/5833")
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
