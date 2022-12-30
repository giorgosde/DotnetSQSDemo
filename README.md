## DotnetSQSDemo
This is a quick demo to showcase AWS SQS integration with .NET

This repo is part of a series focusing on .NET/C#. This repo is meant to be public.

#### Stack and Features:
- .NET 7
- WebApi
- BackgroundService

#### Overview:
- .Net WenApi contains a single POST endpoint allowing posting messages to SQS
- MessageConsumerService to consume and delete* the SQS messages


**As opposed to pub/sub services, in SQS messages deletion is User's concern. Consumed messages are still available until manually or programmatically deleted*

#### Prerequisites:
- AWS Account
- [AWS SQS queue](https://docs.aws.amazon.com/AWSSimpleQueueService/latest/SQSDeveloperGuide/step-create-queue.html)

#### Troubleshooting:
- For connectivity issues ensuer that the queueUrl is set in the `appsettings.json` file  
