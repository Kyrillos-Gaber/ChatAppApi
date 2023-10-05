namespace chatapi.Services;

public class ChatService
{
    public static readonly Dictionary<string, string> Users = new();

    public bool AddUserToList(string newUser)
    {
        lock (Users)
        {
            foreach (var user in Users)
                if (user.Key.ToLower() == newUser.ToLower())
                    return false;

            Users.Add(newUser, null!);
            return true;
        }
    }

    public void AddUserConnectionId(string user, string connectionId)
    {
        lock (Users)
            if (Users.ContainsKey(user))
                Users[user] = connectionId;
    }

    public string GetUserByConnectionId(string connectionId)
    {
        lock (Users)
            return Users.Where(u => u.Value == connectionId)
                .Select(u => u.Key)
                .FirstOrDefault()!;
    }

    public string GetConnectionIdByUser(string user)
    {
        lock (Users)
            return Users.Where(u => u.Key == user)
                .Select(u => u.Value)
                .FirstOrDefault()!;
    }

    public void RemoveUser(string user)
    {
        lock (Users)
            if (Users.ContainsKey(user))
                Users.Remove(user);
    }

    public string[] GetOnlineUsers()
    {
        lock (Users)
            return Users.OrderBy(u => u.Key)
                .Select(u => u.Key)
                .ToArray();
    }

}
