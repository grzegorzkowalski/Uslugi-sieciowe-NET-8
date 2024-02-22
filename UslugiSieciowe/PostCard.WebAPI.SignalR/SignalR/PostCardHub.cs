using Microsoft.AspNetCore.SignalR;

namespace SignalrImplementation.SignalR
{
    public class PostCardHub : Hub
    {
        public async Task SendMail(string user, string email, string prompt)
        {
            var connectionID = Context.ConnectionId;
            await Clients.Caller.SendAsync("Connected", $"{user}, twój identyfikator połączenia {connectionID}");
            await CreateGroup(email);
            await SendMessageToGroup(email, "Dziękujemy za przesłanie wiadomości");
        }
        public async Task CreateGroup(string groupName)
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, groupName);
            await Clients.Caller.SendAsync("ReceiveMessage", $"Prywatna grupa utworzona: {groupName}");
        }
        public async Task SendMessageToGroup(string groupName, string message)
        {
            await Clients.Group(groupName).SendAsync("ReceiveMessage", message);
        }
        public async Task SendCard(string groupName, string link)
        {
            await Clients.Group(groupName).SendAsync("ReceiveCard", link);
        }
    }
}
