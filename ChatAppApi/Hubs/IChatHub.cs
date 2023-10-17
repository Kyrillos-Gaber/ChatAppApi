using ChatAppApi.Dtos;

namespace chatapi.Hubs;

public interface IChatHub
{    
    Task UserConnected();
    
    Task ClosePrivateChat();

    Task OnlineUsers(string[] users);

    Task NewMessage(MessageDto message);
    
    Task OpenPrivateChat(MessageDto message);
    
    Task NewPrivateMessage(MessageDto message);
}