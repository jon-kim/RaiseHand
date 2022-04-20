using Microsoft.AspNetCore.SignalR;
using RaiseHand.Shared;

namespace RaiseHand.Server.Hubs
{
    public class RaiseHandHub : Hub
    {
        //public static Dictionary<string, string> _users = new Dictionary<string, string>();
        public static Dictionary<string, UserHand> _connections = new Dictionary<string, UserHand>();
        public override async Task OnConnectedAsync()
        {
            var username = Context.GetHttpContext().Request.Query["username"];

            //_users[Context.ConnectionId] = username;
            _connections[Context.ConnectionId] = new UserHand(username);

            //await SetHandRaised(username, false);
            await SendAllHands();
            await base.OnConnectedAsync();
        }

        public override async Task OnDisconnectedAsync(Exception? exception)
        {
            //string username = _users.FirstOrDefault(u => u.Key == Context.ConnectionId).Value;
            string username = _connections.FirstOrDefault(u => u.Key == Context.ConnectionId).Value.Username;
            await RemoveHand(username);
        }

        public async Task SetHandRaised(string user, bool raiseHand)
        {
            await Clients.All.SendAsync("ReceiveHand", user, raiseHand);
        }

        public async Task SendAllHands()
        {
            await Clients.All.SendAsync("ReceiveAllHands", _connections.Values.ToArray());
        }

        public async Task RemoveHand(string user)
        {
            await Clients.All.SendAsync("RemoveHand", user);
        }
    }
}
