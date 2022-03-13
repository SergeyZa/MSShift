namespace MessagingContracts
{
	public class Address
	{
		public AddressKind Kind { get; set; }
		public string AddressLine { get; set; } = default!;

		public override string ToString()
		{
			return $"{Kind} {AddressLine}";
		}
	}
}