using Amazon.SQS.Model;
using Amazon.SQS;
using Microsoft.Extensions.Hosting;
using System.Threading.Tasks;
using System.Threading;
using Microsoft.Extensions.Configuration;
using Serilog;

namespace Messaging.Api.BackgroundServices;

public class MessageConsumerService : BackgroundService
{
    private readonly ILogger _logger;
    private readonly IAmazonSQS _sqsClient;
    private readonly IConfiguration _configuration;

    public MessageConsumerService(
        ILogger logger,
        IAmazonSQS sqsClient,
        IConfiguration configuration)
    {
        _logger = logger;
        _sqsClient = sqsClient;
        _configuration = configuration;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _logger.Information("MessageConsumer running");

        var queueUrl = _configuration["AWS:SQSQueueUrl"];
        while (!stoppingToken.IsCancellationRequested)
        {
            var response = await _sqsClient.ReceiveMessageAsync(new ReceiveMessageRequest()
            {
                QueueUrl = queueUrl
            }, stoppingToken);
            
            foreach (var message in response.Messages)
            {
                // For demo purposes, display and immediately delete the message
                _logger.Information($"Received Message: {message.Body}");

                _ = await _sqsClient.DeleteMessageAsync(queueUrl, message.ReceiptHandle, stoppingToken);
            }
        }
    }
}
