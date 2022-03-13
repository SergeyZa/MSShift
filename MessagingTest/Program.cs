using Bogus;
using MessagingWebApi;
using Microsoft.Extensions.Configuration;

var config =
	new ConfigurationBuilder()
		.AddJsonFile("appsettings.json", false, true)
		.Build();

var url = config.GetValue<string>("messagingBaseUrl", " https://localhost:20220");

var fakeAddress =
	new Faker<Address>()
		.RuleFor(a => a.Kind, f => f.PickRandom<AddressKind>())
		.RuleFor(a => a.AddressLine, f => f.Internet.Email());

var fakeMessage =
	new Faker<Message>()
		.RuleFor(m => m.To, f => fakeAddress.Generate())
		.RuleFor(m => m.Subject, f => f.Lorem.Sentence(3));

var httpClient = new HttpClient();
var messagingClient = new MessagingWebApiClient(url, httpClient);

for (var i = 0; i < 10; i++)
{
	await messagingClient.RequestMessageAsync(fakeMessage.Generate());
}
