namespace MessagingWebAPI;

public interface IMessagingService
{
	Task<bool> SendMessage(Message message);
}
