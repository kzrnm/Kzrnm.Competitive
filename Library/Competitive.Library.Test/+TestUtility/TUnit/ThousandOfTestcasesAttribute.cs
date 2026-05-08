namespace Kzrnm.Competitive.Testing;

class ThousandOfTestcasesAttribute() : SkipAttribute("https://github.com/thomhurst/TUnit/issues/5833")
{
    public override Task<bool> ShouldSkip(TestRegisteredContext context)
    {
        return Task.FromResult(
#if AOT
            false
#else
            true
#endif

            && "true".Equals(Environment.GetEnvironmentVariable("CI"), StringComparison.OrdinalIgnoreCase));
    }
}
