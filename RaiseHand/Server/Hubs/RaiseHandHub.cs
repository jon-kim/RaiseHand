using Microsoft.AspNetCore.SignalR;

namespace RaiseHand.Server.Hubs
{
    public class RaiseHandHub : Hub
    {
        public static Dictionary<string, string> _users = new Dictionary<string, string>();   
        public override async Task OnConnectedAsync()
        {
            var username = Context.GetHttpContext().Request.Query["username"];

            _users.Add(Context.ConnectionId, username);

            await AddMessageToChat(username, false);
            await base.OnConnectedAsync();
        }

        public override async Task OnDisconnectedAsync(Exception? exception)
        {

        }

        public async Task AddMessageToChat(string user, bool raiseHand)
        {
            await Clients.All.SendAsync("ReceiveMessage", user, raiseHand);

        }
    }
}
