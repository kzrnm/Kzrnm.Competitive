namespace Kzrnm.Competitive.Testing;

class ThousandOfTestcasesAttribute() : SkipAttribute("https://github.com/thomhurst/TUnit/issues/5833")
{
    public override Task<bool> ShouldSkip(TestRegisteredContext context)
    {
        return Task.FromResult(Environment.GetEnvironmentVariable("CI").Equals("true", StringComparison.OrdinalIgnoreCase));
    }
}
