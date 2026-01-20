using Azure.Messaging.ServiceBus;
using Microsoft.Extensions.Options;
using System.Diagnostics.CodeAnalysis;

namespace Azure.Local.Infrastructure.ServiceBus
{
    [ExcludeFromCodeCoverage]
    public class AzureServiceBusClient : IServiceBusClient
    {
        private readonly ServiceBusClient _client;
        private readonly ServiceBusSender _sender;

#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider adding the 'required' modifier or declaring as nullable.
        public AzureServiceBusClient(IOptions<ServiceBusSettings> connectionOptions)
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider adding the 'required' modifier or declaring as nullable.
        {
            // Initialize Service Bus client and sender but protect it from taking the app down
            // if it is running component tests without Service Bus available.
            try
            {
                var settings = connectionOptions.Value;

                if (string.IsNullOrEmpty(settings.ConnectionString))
                {
                    throw new InvalidOperationException("Service Bus connection string is not configured.");
                }

                if (string.IsNullOrEmpty(settings.QueueName))
                {
                    throw new InvalidOperationException("Service Bus queue name is not configured.");
                }

                _client = new ServiceBusClient(settings.ConnectionString);
                _sender = _client.CreateSender(settings.QueueName);
            }
            catch (ServiceBusException ex)
            {
                // Handle Service Bus specific exceptions
                throw new InvalidOperationException("Failed to initialize Service Bus client.", ex);
            }
            catch (Exception ex) when (ex is not InvalidOperationException)
            {
                // Handle general exceptions
                throw new InvalidOperationException("An error occurred while initializing the Service Bus client.", ex);
            }
        }

        public async Task<bool> SendMessageAsync(string message)
        {
            try
            {
                var serviceBusMessage = new ServiceBusMessage(message);
                await _sender.SendMessageAsync(serviceBusMessage);
                return true;
            }
            catch (ServiceBusException)
            {
                return false;
            }
        }

        public async Task<bool> SendScheduledMessageAsync(string message, DateTimeOffset scheduledEnqueueTime)
        {
            try
            {
                var serviceBusMessage = new ServiceBusMessage(message)
                {
                    ScheduledEnqueueTime = scheduledEnqueueTime
                };
                await _sender.SendMessageAsync(serviceBusMessage);
                return true;
            }
            catch (ServiceBusException)
            {
                return false;
            }
        }

        public async Task<bool> SendMessagesAsync(IEnumerable<string> messages)
        {
            try
            {
                using ServiceBusMessageBatch messageBatch = await _sender.CreateMessageBatchAsync();

                foreach (var message in messages)
                {
                    if (!messageBatch.TryAddMessage(new ServiceBusMessage(message)))
                    {
                        // If the batch is full, send it and create a new one
                        await _sender.SendMessagesAsync(messageBatch);
                        using var newBatch = await _sender.CreateMessageBatchAsync();

                        if (!newBatch.TryAddMessage(new ServiceBusMessage(message)))
                        {
                            // Message too large for an empty batch
                            throw new InvalidOperationException($"Message is too large to fit in a batch.");
                        }
                    }
                }

                // Send remaining messages
                if (messageBatch.Count > 0)
                {
                    await _sender.SendMessagesAsync(messageBatch);
                }

                return true;
            }
            catch (ServiceBusException)
            {
                return false;
            }
        }
    }
}
