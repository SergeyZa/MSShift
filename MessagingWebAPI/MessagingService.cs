using RabbitMQ.Client;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace MessagingWebAPI;

public class MessagingService : IMessagingService
{
	private readonly ILogger<MessagingService> _logger;


	private IConnectionFactory? _connectionFactory;
	private IConnection? _connection;
	private IModel? _channel;

	private IModel Channel
	{
		get
		{
			if (_channel == null)
			{
				_connectionFactory = new ConnectionFactory
				{
					HostName = "localhost",
				};
				_connection = _connectionFactory.CreateConnection();
				_channel = _connection.CreateModel();
				//_channel.QueueDeclare("messages");
			}
			return _channel;
		}
	}

	public MessagingService(ILogger<MessagingService> logger)
	{
		_logger = logger;
	}

	public Task<bool> SendMessage(Message message)
	{
		_logger.LogInformation($"Queueing message: {message}");

		var json = JsonSerializer.SerializeToUtf8Bytes(message);

		Channel.BasicPublish(string.Empty, "messages", body: json);

		return Task.FromResult(true);
	}
}
