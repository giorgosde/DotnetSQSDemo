using Amazon.SQS;
using Amazon.SQS.Model;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Text.Json;
using System.Threading.Tasks;

namespace Messaging.Api.Controllers;

[ApiController]
[Route("[controller]")]
public class MessagingController : ControllerBase
{
    private readonly ILogger<MessagingController> _logger;
    private readonly IAmazonSQS _sqsClient;
    private readonly string _queueUrl;

    public MessagingController(
        ILogger<MessagingController> logger,
        IAmazonSQS sqsClient,
        IConfiguration configuration)
    {
        _logger = logger;
        _sqsClient = sqsClient;
        _queueUrl = configuration["AWS:SQSQueueUrl"];
    }

    [HttpPost]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Post([FromBody] object data)
    {
        try
        {
            var response = await _sqsClient.SendMessageAsync(new SendMessageRequest()
            {
                MessageBody = JsonSerializer.Serialize(data),
                QueueUrl = _queueUrl
            });

            return Ok(response);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Purge()
    {
        try
        {
            var response = await _sqsClient.PurgeQueueAsync(_queueUrl);
            return Ok(response);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }
}
