using Microsoft.AspNetCore.Mvc;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddLogging();
builder.Services.AddSingleton<IMessagingService, MessagingService>();

var app = builder.Build();

var config = app.Services.GetService<IConfiguration>();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
	app.UseSwagger();
	app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app
	.MapPost(
		"messaging/message",
		async ([FromBody]Message message, IMessagingService messagingService, ILogger<Message> logger) =>
		{
			logger.LogInformation("{message}", message);
			try
			{
				var res = await messagingService.SendMessage(message);
				return Results.Ok();
			}
			catch (Exception ex)
			{
				logger.LogError(ex, "Requesting message");
				return Results.Problem();
			}
		})
	.WithName("RequestMessage");

app.Run();
