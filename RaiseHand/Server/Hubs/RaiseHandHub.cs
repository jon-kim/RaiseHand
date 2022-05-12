using Microsoft.AspNetCore.SignalR;
using RaiseHand.Shared;
using System.Diagnostics;

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
            await SendAllHands(Context.ConnectionId);
            await base.OnConnectedAsync();
        }

        public override async Task OnDisconnectedAsync(Exception? exception)
        {
            //string username = _users.FirstOrDefault(u => u.Key == Context.ConnectionId).Value;
            //string username = _connections.FirstOrDefault(u => u.Key == Context.ConnectionId).Value.Username;
            await RemoveHand(_connections[Context.ConnectionId].Username);
            _connections.Remove(Context.ConnectionId);
        }

        public async Task SetHandRaised(string user, bool raiseHand)
        {
            await Clients.All.SendAsync("ReceiveHand", user, raiseHand);
            _connections[Context.ConnectionId].HandRaised = raiseHand;
        }

        public async Task SendAllHands(string connectionID)
        {
            foreach (KeyValuePair<string, UserHand> keyValuePair in _connections)
                if (keyValuePair.Key != connectionID && keyValuePair.Value.HandRaised)
                    await Clients.Client(connectionID).SendAsync("ReceiveHand", keyValuePair.Value.Username, keyValuePair.Value.HandRaised);
        }

        public async Task RemoveHand(string user)
        {
            await Clients.All.SendAsync("RemoveHand", user);
        }
    }
}
