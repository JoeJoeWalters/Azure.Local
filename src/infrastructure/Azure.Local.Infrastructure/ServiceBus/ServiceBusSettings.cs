using System.Diagnostics.CodeAnalysis;

namespace Azure.Local.Infrastructure.ServiceBus
{
    [ExcludeFromCodeCoverage]
    public class ServiceBusSettings
    {
        public string ConnectionString { get; set; } = string.Empty;
        public string QueueName { get; set; } = string.Empty;
    }
}
