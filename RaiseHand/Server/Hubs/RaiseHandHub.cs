using Microsoft.AspNetCore.SignalR;

namespace RaiseHand.Server.Hubs
{
    public class RaiseHandHub : Hub
    {
        public static Dictionary<string, string> _users = new Dictionary<string, string>();
        public override async Task OnConnectedAsync()
        {
            var username = Context.GetHttpContext().Request.Query["username"];

            _users[Context.ConnectionId] = username;

            await SetHandRaised(username, false);
            await base.OnConnectedAsync();
        }

        public override async Task OnDisconnectedAsync(Exception? exception)
        {
            string username = _users.FirstOrDefault(u => u.Key == Context.ConnectionId).Value;
            await RemoveHand(username);
        }

        public async Task SetHandRaised(string user, bool raiseHand)
        {
            await Clients.All.SendAsync("ReceiveHand", user, raiseHand);
        }

        public async Task RemoveHand(string user)
        {
            await Clients.All.SendAsync("RemoveHand", user);
        }
    }
}
