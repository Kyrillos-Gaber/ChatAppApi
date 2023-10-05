namespace chatapi.Hubs;

public interface IChatHub
{
    Task ReceiveMessage(string str);
}