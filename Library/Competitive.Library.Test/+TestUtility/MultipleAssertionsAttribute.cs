using TUnit.Core.Interfaces;

namespace Kzrnm.Competitive.Testing;

[AttributeUsage(AttributeTargets.Method)]
public class MultipleAssertionsAttribute : Attribute, ITestRegisteredEventReceiver
{
    public ValueTask OnTestRegistered(TestRegisteredContext context)
    {
        context.SetTestExecutor(new MultipleAssertionsExectutor());
        return ValueTask.CompletedTask;
    }

    private class MultipleAssertionsExectutor : GenericAbstractExecutor
    {
        protected override async ValueTask ExecuteAsync(Func<ValueTask> action)
        {
            using (Assert.Multiple())
            {
                await action();
            }
        }
    }
}