using Amazon.SQS.Model;
using Amazon.SQS;
using Microsoft.Extensions.Hosting;
using System.Threading.Tasks;
using System.Threading;
using System;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace Messaging.Api.BackgroundServices;

public class MessageConsumerService : BackgroundService
{
    private readonly ILogger<MessageConsumerService> _logger;
    private readonly IAmazonSQS _sqsClient;
    private readonly IConfiguration _configuration;

    public MessageConsumerService(
        ILogger<MessageConsumerService> logger,
        IAmazonSQS sqsClient,
        IConfiguration configuration)
    {
        _logger = logger;
        _sqsClient = sqsClient;
        _configuration = configuration;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _logger.LogInformation("MessageConsumer running");

        var queueUrl = _configuration["AWS:SQSQueueUrl"];
        while (!stoppingToken.IsCancellationRequested)
        {
            var response = await _sqsClient.ReceiveMessageAsync(new ReceiveMessageRequest()
            {
                QueueUrl = queueUrl
            });
            
            foreach (var message in response.Messages)
            {
                Console.WriteLine($"Received Message: {message.Body}");

                _ = await _sqsClient.DeleteMessageAsync(queueUrl, message.ReceiptHandle);
            }
        }
    }
}
