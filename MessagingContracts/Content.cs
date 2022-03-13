namespace MessagingContracts
{
	public class Content
	{
		public MimeType ContentType { get; set; } = MimeType.Plain;
		public string Body { get; set; } = default!;
	}
}