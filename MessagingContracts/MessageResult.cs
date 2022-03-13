namespace MessagingContracts;

public class MessageResult
{
	public MessageStatus Status { get; set; }
	public Message Message { get; set; } = default!;

	public override string ToString()
	{
		return $"{Status} {Message}";
	}
}
