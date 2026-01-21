namespace Azure.Local.Infrastructure.Messaging
{
    /// <summary>
    /// Service Bus client abstraction for sending messages to queues/topics.
    /// Follows the same pattern as IRepository for testability.
    /// </summary>
    public interface IServiceBusClient
    {
        /// <summary>
        /// Sends a single message to the configured queue/topic.
        /// </summary>
        /// <param name="message">The message content to send.</param>
        /// <returns>True if the message was sent successfully, false otherwise.</returns>
        Task<bool> SendMessageAsync(string message);

        /// <summary>
        /// Sends a single message with a scheduled enqueue time.
        /// </summary>
        /// <param name="message">The message content to send.</param>
        /// <param name="scheduledEnqueueTime">The time at which the message should be available for processing.</param>
        /// <returns>True if the message was scheduled successfully, false otherwise.</returns>
        Task<bool> SendScheduledMessageAsync(string message, DateTimeOffset scheduledEnqueueTime);

        /// <summary>
        /// Sends multiple messages to the configured queue/topic as a batch.
        /// </summary>
        /// <param name="messages">The collection of message contents to send.</param>
        /// <returns>True if all messages were sent successfully, false otherwise.</returns>
        Task<bool> SendMessagesAsync(IEnumerable<string> messages);
    }
}
