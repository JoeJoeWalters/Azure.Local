using Azure.Local.Infrastructure.ServiceBus;
using System.Collections.Concurrent;

namespace Azure.Local.Tests.Component.Timesheets.Fakes.ServiceBus
{
    [ExcludeFromCodeCoverage]
    public class FakeServiceBusClient : IServiceBusClient
    {
        private readonly ConcurrentQueue<FakeServiceBusMessage> _messages;

        public FakeServiceBusClient()
        {
            _messages = new ConcurrentQueue<FakeServiceBusMessage>();
        }

        /// <summary>
        /// Gets all messages that have been sent (for test assertions).
        /// </summary>
        public IEnumerable<FakeServiceBusMessage> SentMessages => _messages.ToArray();

        /// <summary>
        /// Gets the count of messages that have been sent.
        /// </summary>
        public int MessageCount => _messages.Count;

        /// <summary>
        /// Clears all sent messages (useful for test setup/teardown).
        /// </summary>
        public void Clear() => _messages.Clear();

        public async Task<bool> SendMessageAsync(string message)
        {
            try
            {
                _messages.Enqueue(new FakeServiceBusMessage(message));
                await Task.Yield(); // Ensures the method is truly asynchronous
                return true;
            }
            catch
            {
                return false;
            }
        }

        public async Task<bool> SendScheduledMessageAsync(string message, DateTimeOffset scheduledEnqueueTime)
        {
            try
            {
                _messages.Enqueue(new FakeServiceBusMessage(message, scheduledEnqueueTime));
                await Task.Yield(); // Ensures the method is truly asynchronous
                return true;
            }
            catch
            {
                return false;
            }
        }

        public async Task<bool> SendMessagesAsync(IEnumerable<string> messages)
        {
            try
            {
                foreach (var message in messages)
                {
                    _messages.Enqueue(new FakeServiceBusMessage(message));
                }

                await Task.Yield(); // Ensures the method is truly asynchronous
                return true;
            }
            catch
            {
                return false;
            }
        }
    }

    /// <summary>
    /// Represents a message stored in the fake Service Bus for test assertions.
    /// </summary>
    public record FakeServiceBusMessage
    {
        public string Body { get; }
        public DateTimeOffset? ScheduledEnqueueTime { get; }
        public DateTimeOffset EnqueuedTime { get; }

        public FakeServiceBusMessage(string body, DateTimeOffset? scheduledEnqueueTime = null)
        {
            Body = body;
            ScheduledEnqueueTime = scheduledEnqueueTime;
            EnqueuedTime = DateTimeOffset.UtcNow;
        }
    }
}
