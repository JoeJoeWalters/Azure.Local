namespace Azure.Local.Tests.Component.Setup
{
    [ExcludeFromCodeCoverage]
    public static class ScenarioSteps
    {
        public readonly struct StepKeyword;

        public static async Task RunAsync(params Func<StepKeyword, Task>[] steps)
        {
            ArgumentNullException.ThrowIfNull(steps);

            var keyword = default(StepKeyword);
            for (var i = 0; i < steps.Length; i++)
            {
                try
                {
                    await steps[i](keyword);
                }
                catch (Exception ex)
                {
                    throw new InvalidOperationException($"Scenario step {i + 1} failed.", ex);
                }
            }
        }

        public static async Task RunAsync(params Func<Task>[] steps)
        {
            ArgumentNullException.ThrowIfNull(steps);

            for (var i = 0; i < steps.Length; i++)
            {
                try
                {
                    await steps[i]();
                }
                catch (Exception ex)
                {
                    throw new InvalidOperationException($"Scenario step {i + 1} failed.", ex);
                }
            }
        }
    }
}
