using chatapi.Services;
using ChatAppApi.Dtos;
using Microsoft.AspNetCore.SignalR;
using System.Runtime.CompilerServices;

namespace chatapi.Hubs;
public sealed class ChatHub : Hub
{
    private readonly ChatService _chatService;

    public ChatHub(ChatService chatService)
    {
        _chatService = chatService;
    }

    public override async Task OnConnectedAsync()
    {
        await Groups.AddToGroupAsync(Context.ConnectionId, "Come2Chat");
        await Clients.Caller.SendAsync("UserConnected");
    }

    public override async Task OnDisconnectedAsync(Exception? ex)
    {
        await Groups.RemoveFromGroupAsync(Context.ConnectionId, "Come2Chat");
        var user = _chatService.GetUserByConnectionId(Context.ConnectionId);
        _chatService.RemoveUser(user);
        await DisplayOnlineUsers();
        await base.OnDisconnectedAsync(ex);
    }

    public async Task AddUserConnectionId(string name)
    {
        _chatService.AddUserConnectionId(name, Context.ConnectionId);
        await DisplayOnlineUsers();
    }

    private async Task DisplayOnlineUsers()
    {
        var onlineUsers = _chatService.GetOnlineUsers();
        await Clients.Groups("Come2Chat").SendAsync("OnlineUsers", onlineUsers);
    }

    public async Task ReceiveMessage(MessageDto message)
    {
        await Clients.Group("Come2Chat").SendAsync("NewMessage", message);
    }

    public async Task CreatePrivateChat(MessageDto message)
    {
        string privateGroupName = GetPrivateGroupName(message.From, message.To!);
        await Groups.AddToGroupAsync(Context.ConnectionId, privateGroupName);
        var toConnectionId = _chatService.GetConnectionIdByUser(message.To!);

        await Clients.Client(toConnectionId).SendAsync("OpenPrivateChat", message);
    }

    public async Task ReceivePrivateMessage(MessageDto message)
    {
        string privateGroupName = GetPrivateGroupName(message.From, message.To!);
        await Clients.Group(privateGroupName).SendAsync("NewPrivateMessage", message);
    }

    private string GetPrivateGroupName(string from, string to)
    {
        Console.WriteLine(string.CompareOrdinal(from, to));
        var stringCompare = string.CompareOrdinal(from, to) < 0;
        return stringCompare ? $"{from}-{to}" : $"{to}-{from}";
    }

    public async Task RemovePrivateChat(string from, string to)
    {
        string privateGroupName = GetPrivateGroupName(from, to);
        await Clients.Group(privateGroupName).SendAsync("ClosePrivateChat");
        await Groups.RemoveFromGroupAsync(Context.ConnectionId, privateGroupName);
        string toConnectionId = _chatService.GetConnectionIdByUser(to);
        await Groups.RemoveFromGroupAsync(toConnectionId, privateGroupName);
    }

}