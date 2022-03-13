namespace MessagingContracts
{
	public class Message
	{
		public Address From { get; set; } = default!;
		public Address To { get; set; } = default!;
		public string Subject { get; set; } = default!;
		public Content Content { get; set; } = default!;

		public override string ToString()
		{
			return $"{To} {Subject}";
		}
	}
}