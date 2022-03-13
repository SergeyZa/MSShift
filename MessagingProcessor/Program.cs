using MessagingContracts;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text.Json;

var factory = new ConnectionFactory { HostName = "localhost" };
var connection = factory.CreateConnection();
using var channel = connection.CreateModel();

channel.QueueDeclare("messages");
channel.QueueDeclare("messages_retry");
channel.QueueDeclare("messages_error");

var consumer = new EventingBasicConsumer(channel);
consumer.Received +=
	(sender, args) =>
	{
		var message = JsonSerializer.Deserialize<Message>(args.Body.Span);
		if (message == null)
		{
			return;
		}
		Console.WriteLine($"Got message: {message}");
		var messageResult = SendMessage(message);
		switch (messageResult.Status)
		{
			case MessageStatus.ErrorRetry:
				{
					var json = JsonSerializer.SerializeToUtf8Bytes(messageResult);
					channel.BasicPublish(string.Empty, "messages_retry", body: json);
					break;
				}
			case MessageStatus.Error:
				{
					var json = JsonSerializer.SerializeToUtf8Bytes(messageResult);
					channel.BasicPublish(string.Empty, "messages_error", body: json);
					break;
				}
			default:
				Console.WriteLine($"Message sent: {message}");
				break;
		}
	};

MessageResult SendMessage(Message message)
{
	Console.WriteLine($"Sending message: {message}");
	var status = () =>
	{
		var random = Random.Shared.Next(10);
		if (random > 2)
			return MessageStatus.OK;
		if (random > 1)
			return MessageStatus.ErrorRetry;
		return MessageStatus.Error;
	};
	return
		new MessageResult
		{
			Message = message,
			Status = status(),
		};
}

channel.BasicConsume(queue: "messages", autoAck: true, consumer: consumer);

var consumerRetry = new EventingBasicConsumer(channel);
consumerRetry.Received +=
	(sender, args) =>
	{
		var messageResult = JsonSerializer.Deserialize<MessageResult>(args.Body.Span);
		if (messageResult == null)
		{
			return;
		}
		Console.WriteLine($"Got RETRY message: {messageResult}");

		var json = JsonSerializer.SerializeToUtf8Bytes(messageResult.Message);

		channel.BasicPublish(string.Empty, "messages", body: json);
	};
channel.BasicConsume(queue: "messages_retry", autoAck: true, consumer: consumerRetry);

Console.WriteLine("Expecting messages");
Console.ReadLine();

channel.Close();
connection.Close();
